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

        readonly private Canvas _canvas;
        readonly private int _squareSize;

        private CoordListingTetri _coordTetri;
        public CoordListingTetri CoordTetri
        {
            get => _coordTetri;
            set
            {
                _coordTetri = value;
                SyncTetriLength();
            }
        }

        public TetriMV(Canvas canvas, int squareSize)
        {
            _canvas = canvas;
            _squareSize = squareSize;
        }

        public void UpdateTetri()
        {
            SyncTetriLength();

            for (int i = 0; i < SquaresTetri.Count; i++)
            {
                SquaresTetri[i].PositionX = CoordTetri.Listing[i].X;
                SquaresTetri[i].PositionY = CoordTetri.Listing[i].Y;
                SquaresTetri[i].SetTetriColor(CoordTetri);
                SquaresTetri[i].UpdateSquare();
            }
        }

        private void SyncTetriLength()
        {
            if (_coordTetri.Listing.Count == SquaresTetri.Count)
                return;

            if (SquaresTetri.Count > 0)
            {
                foreach (var entry in SquaresTetri)
                {
                    entry.RemoveSquareFromCanvas();
                }
                SquaresTetri.Clear();
            }

            for (int i = 0; i < _coordTetri.Listing.Count; i++)
            {
                SquareMV nextSquare = new SquareMV(_canvas, _squareSize);
                SquaresTetri.Add(nextSquare);
            }
        }
    }
}