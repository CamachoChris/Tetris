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
    /// <summary>
    /// Interaction logic for HighscoreWindow.xaml
    /// </summary>
    public partial class HighscoreWindow : Window
    {
        private const int MAXHIGHSCORELENGTH = 10;
        private List<TextBlock> nameTextBlocks = new List<TextBlock>();
        private List<TextBlock> scoreTextBlocks = new List<TextBlock>();
        private TextBlock titleTextBlock = new TextBlock();

        private int _bestScore;
        private int _worstScore;

        public string HighscoreFilename;

        public Highscore CurrentHighscore;

        public HighscoreWindow(int bestScore, int worstScore, string title, string filename)
        {
            InitializeComponent();

            _bestScore = bestScore;
            _worstScore = worstScore;
            titleTextBlock.Text = title;
            HighscoreFilename = filename;

            CurrentHighscore = new Highscore(_bestScore, _worstScore, titleTextBlock.Text);
            CurrentHighscore.LoadFromFile(HighscoreFilename);
        }

        public void TryAdd(int currentScore)
        {
            int earnedRank = CurrentHighscore.GetHighscorePosition(currentScore);
            if (earnedRank < MAXHIGHSCORELENGTH)
            {
                EnterHighscoreNameWindow enterHighscoreName = new EnterHighscoreNameWindow(earnedRank + 1);
                enterHighscoreName.Owner = this;
                enterHighscoreName.ShowDialog();
                if (enterHighscoreName.CurrentName != "")
                {
                    CurrentHighscore.Add(enterHighscoreName.CurrentName, currentScore);
                    LoadHighscoreToScoreboard();
                    CurrentHighscore.SaveToFile(HighscoreFilename);
                }
            }
        }

        private void InitScoreboard()
        {
            for (int i = 0; i < MAXHIGHSCORELENGTH + 1; i++)
            {
                Label nextLabel = new Label();

                if (i == 0)
                {
                    titleTextBlock.FontWeight = FontWeights.Bold;
                    nextLabel.Content = titleTextBlock;
                    nextLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
                    HighscoreStackPanel.Children.Add(nextLabel);
                }
                else
                {
                    if (i != 1)
                        nextLabel.BorderThickness = new Thickness(0, 1, 0, 0);
                    HighscoreStackPanel.Children.Add(nextLabel);

                    Grid labelGrid = new Grid();
                    labelGrid.Height = 20;

                    ColumnDefinition columnLeft = new ColumnDefinition();
                    columnLeft.Width = new GridLength(25);
                    labelGrid.ColumnDefinitions.Add(columnLeft);

                    ColumnDefinition columnMiddle = new ColumnDefinition();
                    columnMiddle.Width = new GridLength(140);
                    labelGrid.ColumnDefinitions.Add(columnMiddle);

                    ColumnDefinition columnRight = new ColumnDefinition();
                    columnRight.Width = new GridLength(30);
                    labelGrid.ColumnDefinitions.Add(columnRight);

                    nextLabel.BorderBrush = Brushes.Black;
                    nextLabel.Content = labelGrid;

                    TextBlock textBlockLeft = new TextBlock();
                    textBlockLeft.HorizontalAlignment = HorizontalAlignment.Right;
                    Grid.SetColumn(textBlockLeft, 0);
                    labelGrid.Children.Add(textBlockLeft);

                    TextBlock textBlockMiddle = new TextBlock();
                    Grid.SetColumn(textBlockMiddle, 1);
                    labelGrid.Children.Add(textBlockMiddle);

                    TextBlock textBlockRight = new TextBlock();
                    textBlockRight.HorizontalAlignment = HorizontalAlignment.Right;
                    Grid.SetColumn(textBlockRight, 2);
                    labelGrid.Children.Add(textBlockRight);

                    textBlockLeft.Margin = new Thickness(0, 2, 0, 0);
                    textBlockMiddle.Margin = new Thickness(0, 2, 0, 0);
                    textBlockRight.Margin = new Thickness(0, 2, 0, 0);

                    textBlockLeft.Text = string.Format($"{i}. ");

                    nameTextBlocks.Add(textBlockMiddle);
                    scoreTextBlocks.Add(textBlockRight);
                }
            }
        }

        private void LoadHighscoreToScoreboard()
        {
            titleTextBlock.Text = CurrentHighscore.HighscoreTitle;

            int i = 0;
            foreach (var entry in CurrentHighscore.GetHighscoreList('.', 60))
            {
                nameTextBlocks[i].Text = entry.Name;
                scoreTextBlocks[i].Text = $"{entry.Score}";
                i++;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitScoreboard();
            LoadHighscoreToScoreboard();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Owner.Activate();
        }
    }
}
