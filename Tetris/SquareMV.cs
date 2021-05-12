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
    class SquareMV
    {
        private readonly int SquareSize;
        private readonly Canvas _canvas;

        private Rectangle Square;

        public int PositionX;
        public int PositionY;
        public Brush TetriColor;

        public SquareMV() { }

        public SquareMV(Canvas canvas, int squareSize) : this()
        {
            _canvas = canvas;
            SquareSize = squareSize;

            AddRectangle();
        }

        private void AddRectangle()
        {
            Square = new Rectangle()
            {
                Height = SquareSize,
                Width = SquareSize,
                RadiusX = 5,
                RadiusY = 5,
                Fill = Brushes.Transparent
            };

            _canvas.Children.Add(Square);
        }

        public void ChangeVisibility(Visibility visibility)
        {
            Square.Visibility = visibility;
        }

        public void RemoveSquare()
        {
            _canvas.Children.Remove(Square);
        }

        public void UpdateSquare()
        {
            if (PositionY >= 0)
                Square.Fill = TetriColor;
            else
                Square.Fill = Brushes.Transparent;
            Square.Margin = new Thickness(PositionX * SquareSize, PositionY * SquareSize, 0, 0);
        }

        public static Brush GetTetriColor(CoordListingTetri tetri)
        {
            Brush ColorI = Brushes.Gold;
            Brush ColorO = Brushes.DarkRed;
            Brush ColorL = Brushes.MediumSeaGreen;
            Brush ColorJ = Brushes.CornflowerBlue;
            Brush ColorS = Brushes.Crimson;
            Brush ColorZ = Brushes.DarkOrchid;
            Brush ColorT = Brushes.MediumTurquoise;

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
                case StandardTetriType.T:
                    color = ColorT;
                    break;
            };
            return color;
        }
    }
}