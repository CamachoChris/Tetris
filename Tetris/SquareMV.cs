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
        private Brush _tetriColor;

        public SquareMV() { }

        public SquareMV(Canvas canvas, int squareSize) : this()
        {
            _canvas = canvas;
            SquareSize = squareSize;

            AddRectangleToCanvas();
        }

        private void AddRectangleToCanvas()
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

        public void RemoveSquareFromCanvas()
        {
            _canvas.Children.Remove(Square);
            Square = null;
        }

        public void ChangeVisibility(Visibility visibility)
        {
            Square.Visibility = visibility;
        }

        public void UpdateSquare()
        {
            if (PositionY >= 0)
                Square.Fill = _tetriColor;
            else
                Square.Fill = Brushes.Transparent;
            Square.Margin = new Thickness(PositionX * SquareSize, PositionY * SquareSize, 0, 0);
        }

        public void SetTetriColor(CoordListingTetri tetri)
        {
            Brush ColorI = Brushes.Gold;
            Brush ColorO = Brushes.DarkRed;
            Brush ColorL = Brushes.MediumSeaGreen;
            Brush ColorJ = Brushes.CornflowerBlue;
            Brush ColorS = Brushes.Crimson;
            Brush ColorZ = Brushes.DarkOrchid;
            Brush ColorT = Brushes.MediumTurquoise;

            switch (tetri.TetriType)
            {
                case StandardTetriType.I:
                    _tetriColor = ColorI;
                    break;
                case StandardTetriType.O:
                    _tetriColor = ColorO;
                    break;
                case StandardTetriType.L:
                    _tetriColor = ColorL;
                    break;
                case StandardTetriType.J:
                    _tetriColor = ColorJ;
                    break;
                case StandardTetriType.S:
                    _tetriColor = ColorS;
                    break;
                case StandardTetriType.Z:
                    _tetriColor = ColorZ;
                    break;
                case StandardTetriType.T:
                    _tetriColor = ColorT;
                    break;
            };
        }
    }
}