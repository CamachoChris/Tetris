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

        PlayingFieldModel FieldModel;
        PlayingFieldView FieldView;

        public MainWindow()
        {
            InitializeComponent();
            FieldModel = new PlayingFieldModel(FieldSizeX, FieldSizeY);
            FieldView = new PlayingFieldView(PlayingCanvas, FieldModel, SquareSize);
            FieldView.Start();
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
                    FieldModel.MoveDown();
                    FieldView.PaintCurrentTetri();
                    break;
                case Key.NumPad4:
                case Key.A:
                    FieldModel.MoveLeft();
                    FieldView.PaintCurrentTetri();
                    break;
                case Key.NumPad6:
                case Key.D:
                    FieldModel.MoveRight();
                    FieldView.PaintCurrentTetri();
                    break;
                case Key.NumPad7:
                case Key.Q:
                    FieldModel.RotateLeft();
                    FieldView.PaintCurrentTetri();
                    break;
                case Key.NumPad9:
                case Key.E:
                    FieldModel.RotateRight();
                    FieldView.PaintCurrentTetri();
                    break;
            }
        }
    }
}
