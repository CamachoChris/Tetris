using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Highscores
{
    public partial class EnterHighscoreNameWindow : Window
    {
        public int CurrentRank;
        public string CurrentName = "";

        public EnterHighscoreNameWindow(int currentRank)
        {
            CurrentRank = currentRank;
            InitializeComponent();
            HighscoreText.Text = $"You made it into the highscore.\nRank {CurrentRank}\n Please enter your name.";
        }

        private void HandleButtonInput()
        {
            MessageBoxResult result = MessageBox.Show(this, $@"Shall this be your name? {NameBox.Text}", "Name correct?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                CurrentName = NameBox.Text;
                this.Close();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameBox.Text != "")
                HandleButtonInput();
        }

        private void NameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && NameBox.Text != "")
                HandleButtonInput();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Owner.Activate();
        }
    }
}
