using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TetrisModel
{
    public partial class TetrisField
    {
        public int FieldSizeX { get; private set; }
        public int FieldSizeY { get; private set; }

        public MatrixTetri CurrentTetri { get; private set; }
        public MatrixTetri NextTetri { get; private set; }

        public event EventHandler TetriLanded;
        public event EventHandler FieldChanged;
        public event EventHandler ShowNextTetri;

        public List<CoordListingTetri> LandedTetri { get; private set; } = new List<CoordListingTetri>();

        public TetrisField(int fieldSizeX, int fieldSizeY)
        {
            FieldSizeX = fieldSizeX;
            FieldSizeY = fieldSizeY;

            CurrentTetri = new MatrixTetri();
            CurrentTetri.BeRandomStandardTetri();
            SetStartPosition(CurrentTetri);

            NextTetri = new MatrixTetri(0, 0);
            NextTetri.BeRandomStandardTetri();
        }

        public StandardTetriType GetCurrentTetriType()
        {
            return CurrentTetri.StandardType;
        }

        private void PrepareForNextTetri()
        {
            SetStartPosition(NextTetri);
            CurrentTetri = NextTetri;
            MatrixTetri tmp = new MatrixTetri(0, 0);
            tmp.BeRandomStandardTetri();
            NextTetri = tmp;
        }

        private void SetStartPosition(MatrixTetri matrixTetri)
        {
            var (_, _, _, maxY) = matrixTetri.GetRange();
            matrixTetri.PositionX = FieldSizeX / 2 - 2;
            matrixTetri.PositionY = -1 - maxY;
        }

        private bool[,] ReturnFilledField()
        {
            bool[,] filledField = new bool[FieldSizeX, FieldSizeY];
            foreach(var entry in LandedTetri)
            {
                for (int i = 0; i < entry.Listing.Length; i++)
                {
                    filledField[entry.Listing[i].X, entry.Listing[i].Y] = true;
                }
            }
            return filledField;
        }

        private void FindFinishedLines()
        {
            int finishedLineCount = 0;
            bool[,] filledField = ReturnFilledField();

            for (int y = 0; y < FieldSizeY; y++)
            {
                for (int x = 0; x < FieldSizeX; x++)
                {
                    if (filledField[x, y] == false)
                    {
                        break;
                    }
                    if (x == FieldSizeX - 1)
                    {
                        RemovedFinishedLine(y);
                        finishedLineCount++;
                    }
                }
            }
            if (finishedLineCount > 0)
                LetThemFall(finishedLineCount);
        }

        private void RemovedFinishedLine(int lineNumber)
        {
            foreach(var entry in LandedTetri)
            {
                int i = 0;
                while (i < entry.Listing.Length)
                {
                    if (entry.Listing[i].Y == lineNumber)
                    {
                        entry.RemoveAt(i);
                        if (entry.Listing.Length == 0)
                        {
                            LandedTetri.Remove(entry);
                        }
                        i--;
                    }
                    i++;
                };
            }
        }

        private void LetThemFall(int lineCount)
        {
            bool couldFall = false;
            //do
            {
                foreach (var entry in LandedTetri)
                {
                    if (FallingDown(entry))
                        couldFall = true;
                }
            } while (couldFall);
        }

        private bool FallingDown(CoordListingTetri landedTetri)
        {
            bool borderCollision = CollisionWithBorder(landedTetri);
            bool squareCollision = CollisionWithSquare(landedTetri);

            if (!borderCollision && !squareCollision)
            {
                landedTetri.Fall(1);
                return true;
            }
            return false;
        }
    }
}
