using System.Windows;
using System.Windows.Input;


namespace QuizApp
{
    /// <summary>
    /// Interaction logic for Finished.xaml
    /// </summary>
    public partial class Finished : Window
    {
        public Finished(string winner, string WS, string loser, string LS)
        {

            InitializeComponent();

            this.winner.Text = winner;
            this.loser.Text = loser;
            this.WS.Text = WS;
            this.LS.Text = LS;
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
