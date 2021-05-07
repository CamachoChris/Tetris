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
        private readonly int _squareSize;

        private readonly List<Rectangle> _rectangleTetri = new List<Rectangle>();
        private Brush _tetriColor;
        private Canvas _canvas;

        private CoordListingTetri _coordTetri;
        public CoordListingTetri CoordTetri
        {
            get => _coordTetri;
            set
            {
                _coordTetri = value;
                _coordTetri.RemoveOne += _coordTetri_RemoveOne;
                SetTetriColor();
            }
        }

        private void _coordTetri_RemoveOne(object sender, EventArgs e)
        {
            //_canvas.Children.Clear();
            _rectangleTetri[_rectangleTetri.Count - 1].Visibility = Visibility.Hidden;
            _rectangleTetri.RemoveAt(_rectangleTetri.Count - 1);
        }

        public TetriMV() { }
        
        public TetriMV(Canvas canvas, int squareSize)
        {
            _canvas = canvas;

            for (int i = 0; i < 4; i++)
            {
                _rectangleTetri.Add(new Rectangle()
                {
                    Height = squareSize,
                    Width = squareSize,
                    RadiusX = 5,
                    RadiusY = 5,
                    Fill = Brushes.Transparent
                });


                _canvas.Children.Add(_rectangleTetri[i]);
            }
            _squareSize = squareSize;
        }

        public void Paint()
        {
            for (int i = 0; i < _rectangleTetri.Count; i++)
                PaintSquare(_rectangleTetri[i], CoordTetri.Listing[i].X, CoordTetri.Listing[i].Y);
        }

        private void PaintSquare(Rectangle square, int x, int y)
        {
            if (y >= 0)
                square.Fill = _tetriColor;
            else
                square.Fill = Brushes.Transparent;
            square.Margin = new Thickness(x * _squareSize, y * _squareSize, 0, 0);
        }

        private void SetTetriColor()
        {
            switch (CoordTetri.TetriType)
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
            };
        }
    }
}