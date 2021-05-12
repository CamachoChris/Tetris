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
        public readonly List<SquareMV> SquaresTetri = new List<SquareMV>();

        private CoordListingTetri _coordTetri;
        public CoordListingTetri CoordTetri
        {
            get => _coordTetri;
            set
            {
                _coordTetri = value;
                foreach (var entry in SquaresTetri)
                {
                    entry.SetTetriColor(value);
                }
            }
        }

        public TetriMV() { }

        public TetriMV(Canvas canvas, int squareSize, int squareCount) : this()
        {
            for (int i = 0; i < squareCount; i++)
            {
                SquareMV nextSquare = new SquareMV(canvas, squareSize);
                SquaresTetri.Add(nextSquare);
            }
        }

        public void UpdateTetri()
        {
            for (int i = 0; i < SquaresTetri.Count; i++)
            {
                SquaresTetri[i].PositionX = CoordTetri.Listing[i].X;
                SquaresTetri[i].PositionY = CoordTetri.Listing[i].Y;
                SquaresTetri[i].UpdateSquare();
            }
        }
    }
}