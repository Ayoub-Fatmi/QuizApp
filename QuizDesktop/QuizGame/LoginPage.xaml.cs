using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace QuizApp
{
    public partial class LoginPage : Window
    {
        public string username {  get; set; }
        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            username = UsernameTextBox.Text;

            if (!string.IsNullOrWhiteSpace(username))
            {
                DialogResult = true;
                Close();
            }else
            {
                MessageBox.Show("Please enter a username.");
            }
        }
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
    }
}
