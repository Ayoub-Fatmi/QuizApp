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
    /// Interaction logic for Quiz.xaml
    /// </summary>
    public partial class Quiz : Page
    {

        private MainWindow mainWindow;

        public Quiz(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        public void SetQuestion(string question)
        {
            QuestionTextBlock.Text = question;
        }

        public void SetChoices(string[] choices)
        {
            ChoiceButton1.Content = choices[0];
            ChoiceButton2.Content = choices[1];
            ChoiceButton3.Content = choices[2];
            ChoiceButton4.Content = choices[3];
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string selectedAnswer="";
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                selectedAnswer = clickedButton.Content.ToString();
            }

            mainWindow.AnswerButton_Click(selectedAnswer);


            if (mainWindow.checkIfCorrect(selectedAnswer)) {   
                mainWindow.ShowCorrectPage();
            }
            else
            {
                mainWindow.ShowWrongPage();
            }
        }
    }
}
