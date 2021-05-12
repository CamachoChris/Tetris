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
using TetrisModel;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string AppName = "Tetris";
        const string Version = "0.0.0";
        const string Developer = "Grimakar";
        const string TimeOfDevelopment = "May 2021";

        const int SquareSize = 30;
        const int FieldSizeX = 10; //horizontal
        const int FieldSizeY = 18; //vertical

        readonly TetrisField tetrisField;
        readonly TetrisFieldMV tetrisFieldMV;        

        public MainWindow()
        {
            InitializeComponent();
            tetrisField = new TetrisField(FieldSizeX, FieldSizeY);
            tetrisField.Init();

            tetrisFieldMV = new TetrisFieldMV(PlayingCanvas, TeaserCanvas, tetrisField, SquareSize)
            {
                LevelText = LevelText,
                ScoreText = ScoreText,
                PauseText = PauseText
            };
            PauseText.Visibility = Visibility.Hidden;

            tetrisFieldMV.Init();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            tetrisField.Start();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            tetrisField.Reset();
        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, $"{AppName}\n{Version}\n{TimeOfDevelopment} {Developer}.\nNo rights reserved...", $"About {AppName}");
        }

        private void MenuQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.NumPad5:
                case Key.S:                    
                    tetrisField.MoveDown();
                    break;
                case Key.NumPad4:
                case Key.A:
                    tetrisField.MoveLeft();
                    break;
                case Key.NumPad6:
                case Key.D:
                    tetrisField.MoveRight();
                    break;
                case Key.NumPad7:
                case Key.Q:
                    tetrisField.RotateLeft();
                    break;
                case Key.NumPad9:
                case Key.E:
                    tetrisField.RotateRight();
                    break;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (tetrisField.IsGameRunning())
                tetrisField.PauseGame();

            MessageBoxResult result = MessageBox.Show("Really quit?", "Warning", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
