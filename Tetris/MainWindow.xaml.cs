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

        readonly Brush ColorI = Brushes.Gold;
        readonly Brush ColorO = Brushes.DarkRed;
        readonly Brush ColorL = Brushes.MediumSeaGreen;
        readonly Brush ColorJ = Brushes.CornflowerBlue;
        readonly Brush ColorS = Brushes.Crimson;
        readonly Brush ColorZ = Brushes.DarkOrchid;

        const int SquareSize = 30;
        const int FieldSizeX = 10; //horizontal
        const int FieldSizeY = 18; //vertical

        public MainWindow()
        {
            InitializeComponent();
            TetrominoModel tetrominoI = new TetrominoModel(TetrominoModel.Tetri.I);
            PaintTetromino(tetrominoI, 0, 1, ColorI);
            TetrominoModel tetrominoO = new TetrominoModel(TetrominoModel.Tetri.O);
            PaintTetromino(tetrominoO, 5, 1, ColorO);
            TetrominoModel tetrominoL = new TetrominoModel(TetrominoModel.Tetri.L);
            PaintTetromino(tetrominoL, 0, 6, ColorL);
            TetrominoModel tetrominoJ = new TetrominoModel(TetrominoModel.Tetri.J);
            PaintTetromino(tetrominoJ, 5, 6, ColorJ);
            TetrominoModel tetrominoS = new TetrominoModel(TetrominoModel.Tetri.S);
            PaintTetromino(tetrominoS, 0, 11, ColorS);
            TetrominoModel tetrominoZ = new TetrominoModel(TetrominoModel.Tetri.Z);
            PaintTetromino(tetrominoZ, 5, 11, ColorZ);

        }

        public void PaintTetromino(TetrominoModel tetromino, int x, int y, Brush brush)
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
