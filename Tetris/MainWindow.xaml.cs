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

        public MainWindow()
        {
            InitializeComponent();
            Tetromino tetromino = new Tetromino();
            tetromino.CreateRandomTetro();
            PaintTetromino(tetromino, 0, 0, Brushes.Blue);
            tetromino.RotateLeft();
            PaintTetromino(tetromino, 5, 0, Brushes.Green);
            tetromino.RotateLeft();
            PaintTetromino(tetromino, 0, 5, Brushes.Green);
            tetromino.RotateLeft();
            PaintTetromino(tetromino, 5, 5, Brushes.Green);
            tetromino.RotateRight();
            PaintTetromino(tetromino, 0, 10, Brushes.Red);
            tetromino.RotateRight();
            PaintTetromino(tetromino, 5, 10, Brushes.Red);
            tetromino.RotateRight();
            PaintTetromino(tetromino, 0, 15, Brushes.Red);
            tetromino.RotateRight();
            PaintTetromino(tetromino, 5, 15, Brushes.Red);

        }

        public void PaintTetromino(Tetromino tetromino, int x, int y, Brush brush)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (tetromino.Position[j, i] == true && IsInField(x + j, y + i))
                        PaintSquare(x + j, y + i, brush);
                }
            }
        }

        private bool IsInField(int x, int y)
        {
            if ((x >= 0) && (y >= 0) && (x < FieldSizeX) && (y < FieldSizeY))
                return true;
            return false;
        }

        public void PaintSquare(int x, int y, Brush brush)
        {
            Rectangle rectangle = new Rectangle() { Height = SquareSize, Width = SquareSize, RadiusX = 4, RadiusY = 4 };
            rectangle.Fill = brush;
            rectangle.Margin = new Thickness(x * SquareSize, y * SquareSize, 0, 0);
            PlayingField.Children.Add(rectangle);
        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, $"{AppName}\n{Version}\n{TimeOfDevelopment} {Developer}.\nNo rights reserved...", $"About {AppName}");
        }

        private void MenuQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
