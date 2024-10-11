using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace QuizApp
{
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private string username;
        private Quiz q;
        public string Answer { get; set; }

        private waitingPage wp;
        private Finished Finished;

        public MainWindow()
        {
            LoginPage lp = new LoginPage();

            bool? result = lp.ShowDialog();
            if (result == true)
            {
                username = lp.username;
                InitializeComponent();
                q = new Quiz(this);
                MainFrame.Navigate(q);
                MessageTextBox.Text = $"{username} Enter your message here...";
                ConnectToServer();
            }
            else
            {
                this.Close();
            }

        }

        private async void ConnectToServer()
        {
            try
            {

                client = new TcpClient("localhost", 5580);
                NetworkStream stream = client.GetStream();
                reader = new StreamReader(stream, Encoding.UTF8);
                writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                await Task.Run(() => ListenForMessages());
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error connecting to server: " + ex.Message);
                this.Close();
            }
        }

        private async void ListenForMessages()
        {
            try
            {

                while (true)
                {

                    string message = await reader.ReadLineAsync();
                    if (message != null)
                    {

                        Dispatcher.Invoke(() => HandleMessage(message));
                    }
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => MessageBox.Show("Error reading from server: " + ex.Message));
                this.Close();
            }
        }

        private void HandleMessage(string message)
        {
            if (message.StartsWith("Enter your name:"))
            {
                writer.WriteLine(username);
            }
            else if (message == "WAITING")
            {
                this.Hide();
                wp = new waitingPage();
                wp.Show();

                
            }
            else if (message.StartsWith("QUESTION:"))
            {
                string question = message.Substring(9);
                q.SetQuestion(question);
            }
            else if (message.StartsWith("CHOICES:"))
            {
                string choices = message.Substring("CHOICES:".Length);
                string[] choiceArray = choices.Split(',');

                q.SetChoices(choiceArray);

            }
            else if (message.StartsWith("ANSWER:"))
            {
                string answer = message.Substring("ANSWER:".Length);

                Answer = answer;

            }
            else if (message.StartsWith("TOO SLOW0"))
            {
                ShowSlowerPage();

            }
            else if (message.StartsWith("TOO SLOW1"))
            {
                ShowUserWasWrong();

            }
            else if (message.StartsWith("SCORE:"))
            {
                string[] parts = message.Split(':');
                string playerName = parts[1];
                string score = parts[2];
                if(int.Parse(score) > 0 )
                {
                    if(P1N.Text == playerName)
                    {
                        P1Pts.Text = score;
                    }
                    else
                    {
                        P2Pts.Text = score;

                    }
                }
            }
            else if (message.StartsWith("CHAT:"))
            {
                AppendMessage(message.Substring(5));
            }
            else if (message.StartsWith("Maximum"))
            {
                this.Close();
                MessageBox.Show(message);
            }
            else if (message.StartsWith("Quiz finished:"))
            {
                string[] parts = message.Split(new string[] { "Quiz finished: Winner: ", " with score ", ", Loser: ", " with score " }, StringSplitOptions.None);


                Finished = new Finished(parts[1], parts[2], parts[3], parts[4]);
                this.Close();
                Finished.Show();
            }
            else if (message.StartsWith("GAME_STARTED "))
            {
                if(wp != null)
                {
                    wp.Close();
                }

                string playersInfo = message.Substring("GAME_STARTED PLAYER1:".Length);
                string[] playerNames = playersInfo.Split(new string[] { ", PLAYER2:" }, StringSplitOptions.None);

                P1N.Text = playerNames[0];
                P1Pts.Text = "0";
                P2N.Text = playerNames[1];
                P2Pts.Text = "0";


                this.Show();
            }
        }

        private void AppendMessage(string message)
        {
            string[] parts = message.Split(new[] { ": " }, 2, StringSplitOptions.None);
            if (parts.Length == 2)
            {
                string user = parts[0];
                string msg = parts[1];

                var paragraph = new Paragraph();

                // Add username with color
                var usernameRun = new Run(user + ": ")
                {
                    Foreground = user == username ? Brushes.Blue : Brushes.Green,
                    FontWeight = FontWeights.Bold
                };
                paragraph.Inlines.Add(usernameRun);

                // Add message with normal color
                var messageRun = new Run(msg)
                {
                    Foreground = Brushes.Black
                };
                paragraph.Inlines.Add(messageRun);

                // Set the margin to reduce space between paragraphs
                paragraph.Margin = new Thickness(0);
                paragraph.Padding = new Thickness(0);

                ChatRichTextBox.Document.Blocks.Add(paragraph);
            }
            else
            {
                var paragraph = new Paragraph(new Run(message))
                {
                    Margin = new Thickness(0),
                    Padding = new Thickness(0)
                };

                ChatRichTextBox.Document.Blocks.Add(paragraph);
            }

            // Scroll to the end of the RichTextBox
            ChatRichTextBox.ScrollToEnd();
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageTextBox.Text;
            if (!string.IsNullOrWhiteSpace(message))
            {
                try
                {
                    await writer.WriteLineAsync("CHAT:" + message);

                  
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error sending message: " + ex.Message);
                    this.Close();
                }
            }
        }

        public async void AnswerButton_Click(string selectedAnswer)
        {

            try
            {
                await writer.WriteLineAsync("ANSWER:" + selectedAnswer);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending answer: " + ex.Message);
                this.Close();
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
        private void MinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private bool isDragging = false;
        private Point offset;

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            offset = e.GetPosition(this);
            this.CaptureMouse();
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
                this.ReleaseMouseCapture();
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentPos = e.GetPosition(this);
                this.Left += currentPos.X - offset.X;
                this.Top += currentPos.Y - offset.Y;
            }
        }

        public bool checkIfCorrect(string userChoice)
        {
                if(userChoice == Answer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

        }

        public async void ShowWrongPage()
        {
            MainFrame.Navigate(new WrongPage());

            await Task.Delay(1500);

            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }
        public async void ShowUserWasWrong()
        {
            MainFrame.Navigate(new UserWasWrong());

            await Task.Delay(1500);

            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }

        public async void ShowSlowerPage()
        {
            MainFrame.Navigate(new SlowerPage());

            // Wait for 3 seconds
            await Task.Delay(1500);

            // Navigate back to the previous page
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }

        public async void ShowCorrectPage()
        {
            MainFrame.Navigate(new CorrectPage());

            // Wait for 3 seconds
            await Task.Delay(1500);

            // Navigate back to the previous page
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }


        private void MessageTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MessageTextBox.Text == $"{username} Enter your message here...")
            {
                MessageTextBox.Text = "";
                MessageTextBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void MessageTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MessageTextBox.Text))
            {
                MessageTextBox.Text = $"{username} Enter your message here...";
                MessageTextBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

    }
}
