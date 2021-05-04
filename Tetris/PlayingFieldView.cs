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
    class PlayingFieldView
    {
        readonly Brush ColorI = Brushes.Gold;
        readonly Brush ColorO = Brushes.DarkRed;
        readonly Brush ColorL = Brushes.MediumSeaGreen;
        readonly Brush ColorJ = Brushes.CornflowerBlue;
        readonly Brush ColorS = Brushes.Crimson;
        readonly Brush ColorZ = Brushes.DarkOrchid;

        readonly int SquareSize;

        readonly Canvas TetrisCanvas;
        public PlayingFieldModel TetrisFieldModel;
        private Rectangle[] currentTetri;

        public PlayingFieldView(Canvas canvas, PlayingFieldModel playingField, int squaresize)
        {
            TetrisCanvas = canvas;
            TetrisFieldModel = playingField;
            currentTetri = new Rectangle[4];
            SquareSize = squaresize;
            for (int i = 0; i < 4; i++)
            {
                currentTetri[i] = new Rectangle() { Height = SquareSize, Width = SquareSize, RadiusX = 5, RadiusY = 5 };
                currentTetri[i].Fill = Brushes.Transparent;
                TetrisCanvas.Children.Add(currentTetri[i]);
            }
        }

        public void Start()
        {
            PaintCurrentTetri();
        }

        public void PaintCurrentTetri()
        {
            PaintTetromino(TetrisFieldModel.LocateCurrentTetri());
        }

        public void PaintTetromino(Coord[] tetri)
        {
            for (int i = 0; i < 4; i++)
                PaintSquare(currentTetri[i], tetri[i].X, tetri[i].Y);
        }

        public void PaintSquare(Rectangle rectangle, int x, int y)
        {
            if (y >= 0)
                rectangle.Fill = GetTetriColor(TetrisFieldModel.GetCurrentTetriType());
            else
                rectangle.Fill = Brushes.Transparent;
            rectangle.Margin = new Thickness(x * SquareSize, y * SquareSize, 0, 0);
        }

        private Brush GetTetriColor(Tetri tetri)
        {
            return tetri switch
            {
                Tetri.I => ColorI,
                Tetri.O => ColorO,
                Tetri.L => ColorL,
                Tetri.J => ColorJ,
                Tetri.S => ColorS,
                Tetri.Z => ColorZ,
                _ => Brushes.Transparent,
            };
        }
    }
}
