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

        PlayingFieldModel FieldModel = new PlayingFieldModel(FieldSizeX, FieldSizeY);
        PlayingFieldView FieldView;

        public MainWindow()
        {
            InitializeComponent();
            FieldView = new PlayingFieldView(PlayingCanvas, FieldModel, SquareSize);
            
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
                case Key.S:
                    FieldModel.MoveDown();
                    FieldView.PaintCurrentTetri();
                    break;
                case Key.A:
                    FieldModel.MoveLeft();
                    FieldView.PaintCurrentTetri();
                    break;
                case Key.D:
                    FieldModel.MoveRight();
                    FieldView.PaintCurrentTetri();
                    break;
                case Key.Q:
                    FieldModel.CurrentTetri.RotateLeft();
                    FieldView.PaintCurrentTetri();
                    break;
                case Key.E:
                    FieldModel.CurrentTetri.RotateRight();
                    FieldView.PaintCurrentTetri();
                    break;
            }
        }
    }
}
