using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizApp
{
    /// <summary>
    /// Interaction logic for waitingPage.xaml
    /// </summary>
    public partial class waitingPage : Window
    {
        public waitingPage()
        {
            InitializeComponent();
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
