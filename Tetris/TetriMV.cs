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
using System.Diagnostics;

namespace Tetris
{
    class TetriMV
    {
        readonly Brush ColorI = Brushes.Gold;
        readonly Brush ColorO = Brushes.DarkRed;
        readonly Brush ColorL = Brushes.MediumSeaGreen;
        readonly Brush ColorJ = Brushes.CornflowerBlue;
        readonly Brush ColorS = Brushes.Crimson;
        readonly Brush ColorZ = Brushes.DarkOrchid;

        public Rectangle[] RectangleTetri = new Rectangle[4];
        public Brush TetriColor;
        public int SquareSize;

        private CoordTetromino _coordTetri;
        public CoordTetromino CoordTetri
        {
            get => _coordTetri;
            set
            {
                _coordTetri = value;
                SetTetriColor();
            }
        }

        public TetriMV() { }
        
        public TetriMV(Canvas canvas, int squareSize)
        {
            for (int i = 0; i < 4; i++)
            {
                RectangleTetri[i] = new Rectangle() { Height = squareSize, Width = squareSize, RadiusX = 5, RadiusY = 5 };
                RectangleTetri[i].Fill = Brushes.Transparent;
                canvas.Children.Add(RectangleTetri[i]);
            }
            SquareSize = squareSize;
        }

        public void Paint()
        {
            for (int i = 0; i < RectangleTetri.Length; i++)
                PaintSquare(RectangleTetri[i], CoordTetri.Tetri[i].X, CoordTetri.Tetri[i].Y);
        }

        public void PaintSquare(Rectangle rectangle, int x, int y)
        {
            if (y >= 0)
                rectangle.Fill = TetriColor;
            else
                rectangle.Fill = Brushes.Transparent;
            rectangle.Margin = new Thickness(x * SquareSize, y * SquareSize, 0, 0);
        }

        private void SetTetriColor()
        {
            switch (CoordTetri.TetriType)
            {
                case Tetri.I:
                    TetriColor = ColorI;
                    break;
                case Tetri.O:
                    TetriColor = ColorO;
                    break;
                case Tetri.L:
                    TetriColor = ColorL;
                    break;
                case Tetri.J:
                    TetriColor = ColorJ;
                    break;
                case Tetri.S:
                    TetriColor = ColorS;
                    break;
                case Tetri.Z:
                    TetriColor = ColorZ;
                    break;
            };
        }
    }
}