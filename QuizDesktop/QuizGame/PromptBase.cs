using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace QuizApp
{
    public static class PromptBase
    {
        public static string ShowDialog(string text, string caption)
        {
            Window prompt = new Window
            {
                Width = 300,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Title = caption
            };
            StackPanel stackPanel = new StackPanel();
            TextBlock textBlock = new TextBlock { Text = text, Margin = new Thickness(10) };
            TextBox textBox = new TextBox { Margin = new Thickness(10) };
            Button confirmation = new Button { Content = "Ok", Width = 75, Margin = new Thickness(10) };
            confirmation.Click += (sender, e) => { prompt.DialogResult = true; prompt.Close(); };
            stackPanel.Children.Add(textBlock);
            stackPanel.Children.Add(textBox);
            stackPanel.Children.Add(confirmation);
            prompt.Content = stackPanel;
            prompt.ShowDialog();
            return textBox.Text;
        }
    }
}
