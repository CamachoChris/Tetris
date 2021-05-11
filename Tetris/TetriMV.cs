using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public readonly int SquareSize;

        public readonly List<Rectangle> RectangleTetri = new List<Rectangle>();
        public Brush TetriColor;
        private Canvas _canvas;

        private CoordListingTetri _coordTetri;
        public CoordListingTetri CoordTetri
        {
            get => _coordTetri;
            set
            {
                _coordTetri = value;
                TetriColor = GetTetriColor(value);
            }
        }

        public TetriMV() { }

        public TetriMV(Canvas canvas, int squareSize, int squareCount) : this()
        {
            _canvas = canvas;
            SquareSize = squareSize;

            AddSquares(squareCount);
        }

        public void AddSquares(int count)
        {
            for (int i = 0; i < count; i++)
            {
                RectangleTetri.Add(new Rectangle()
                {
                    Height = SquareSize,
                    Width = SquareSize,
                    RadiusX = 5,
                    RadiusY = 5,
                    Fill = Brushes.Transparent
                });

                _canvas.Children.Add(RectangleTetri[i]);
            }
        }

        public void RemoveSquares(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _canvas.Children.Remove(RectangleTetri[RectangleTetri.Count - 1]);
                RectangleTetri.RemoveAt(RectangleTetri.Count - 1);
            }
        }

        public void UpdateTetri()
        {
            for (int i = 0; i < RectangleTetri.Count; i++)
                UpdateSquare(RectangleTetri[i], CoordTetri.Listing[i].X, CoordTetri.Listing[i].Y, SquareSize, TetriColor);
        }

        public static void UpdateSquare(Rectangle square, int x, int y, int squareSize, Brush color)
        {
            if (y >= 0)
                square.Fill = color;
            else
                square.Fill = Brushes.Transparent;
            square.Margin = new Thickness(x * squareSize, y * squareSize, 0, 0);
        }

        public static Brush GetTetriColor(CoordListingTetri tetri)
        {
            Brush ColorI = Brushes.Gold;
            Brush ColorO = Brushes.DarkRed;
            Brush ColorL = Brushes.MediumSeaGreen;
            Brush ColorJ = Brushes.CornflowerBlue;
            Brush ColorS = Brushes.Crimson;
            Brush ColorZ = Brushes.DarkOrchid;

            Brush color = Brushes.Transparent;
            switch (tetri.TetriType)
            {
                case StandardTetriType.I:
                    color = ColorI;
                    break;
                case StandardTetriType.O:
                    color = ColorO;
                    break;
                case StandardTetriType.L:
                    color = ColorL;
                    break;
                case StandardTetriType.J:
                    color = ColorJ;
                    break;
                case StandardTetriType.S:
                    color = ColorS;
                    break;
                case StandardTetriType.Z:
                    color = ColorZ;
                    break;
            };
            return color;
        }
    }
}