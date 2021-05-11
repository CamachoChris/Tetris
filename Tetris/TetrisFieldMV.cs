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
    partial class TetrisFieldMV
    {
        private readonly int SquareSize;

        private readonly Canvas TetrisCanvas;
        private readonly Canvas TeaserCanvas;
        private readonly TetrisField tetrisField;
        
        private TetriMV currentTetri;
        private TetriMV nextTetri;

        private readonly List<TetriMV> LandedTetriMV = new List<TetriMV>();

        private readonly List<TetriMV> LandedSquaresMV = new List<TetriMV>();

        public TetrisFieldMV(Canvas canvas, Canvas teasercanvas, TetrisField field, int squaresize)
        {
            TetrisCanvas = canvas;
            TeaserCanvas = teasercanvas;
            tetrisField = field;
            SquareSize = squaresize;

            field.FieldChanged += TetrisEvent_FieldChanged;
            field.TetriLanded += TetrisEvent_TetriLanded;
            field.ShowNextTetri += Field_ShowNextTetri;
        }

        private void MakeNewCurrent()
        {
            currentTetri = new TetriMV(TetrisCanvas, SquareSize, 4)
            {
                CoordTetri = new CoordListingTetri(tetrisField.CurrentTetri)
            };
        }

        private void MakeNewNext()
        {
            TeaserCanvas.Children.Clear();

            nextTetri = new TetriMV(TeaserCanvas, SquareSize, 4)
            {
                CoordTetri = new CoordListingTetri(tetrisField.NextTetri)
            };

            nextTetri.UpdateTetri();
        }

        private void SyncLandedList()
        {
            int actualSquareCount = tetrisField.GetLandedSquareCount();
            int difference = actualSquareCount - LandedSquaresMV.Count;
            if (difference == 0)
                return;
            if (difference > 0)
            {
                for (int i = 0; i < difference; i++)
                {
                    TetriMV nextTetri = new TetriMV(TetrisCanvas, SquareSize, 1);
                    LandedSquaresMV.Add(nextTetri);
                }
            }
            else if (difference < 0)
            {
                for (int i = 0; i < difference; i++)
                {
                    LandedSquaresMV[LandedSquaresMV.Count - 1].RemoveSquares(1);
                    LandedSquaresMV.RemoveAt(LandedSquaresMV.Count - 1);
                }
            }
        }

        private void UpdateField()
        {
            SyncLandedList();

            int i = 0;
            foreach (var entry in tetrisField.LandedTetri)
            {
                for (int j = 0; j < entry.Listing.Length; j++)
                {
                    int x = entry.Listing[j].X;
                    int y = entry.Listing[j].Y;
                    TetriMV.UpdateSquare(LandedSquaresMV[i].RectangleTetri[0], x, y, LandedSquaresMV[i].SquareSize, LandedSquaresMV[i].TetriColor);
                }
            }
        }

        private void TidyUpLandedList()
        {
            
            List<TetriMV> emptyEntry = new List<TetriMV>();
            foreach (var entry in LandedTetriMV)
            {
                if (entry.CoordTetri.Listing.Length == 0)
                    emptyEntry.Add(entry);
            }
            foreach (var entry in emptyEntry)
            {
                LandedTetriMV.Remove(entry);
            }
        }

        public void Init()
        {
            MakeNewCurrent();
            MakeNewNext();
        }

        public void MoveLeft()
        {
            tetrisField.MoveLeft();
        }

        public void MoveRight()
        {
            tetrisField.MoveRight();
        }

        public void MoveDown()
        {
            tetrisField.MoveDown();
        }

        public void RotateLeft()
        {
            tetrisField.RotateLeft();
        }

        public void RotateRight()
        {
            tetrisField.RotateRight();
        }
    }
}
