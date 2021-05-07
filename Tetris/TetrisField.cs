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
                        LetThemFall();
                    }
                }

            }
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
                        i--;
                    }
                    i++;
                };
            }
        }

        private void LetThemFall()
        {
            foreach(var entry in LandedTetri)
            {
                entry.Fall();
            }
        }

        private bool FallingDown(CoordListingTetri landedTetri)
        {
            bool borderCollision = CollisionWithBorder(landedTetri);
            bool squareCollision = CollisionWithSquare(landedTetri);

            if (!borderCollision && !squareCollision)
            {
                CurrentTetri.PositionY++;
                return true;
            }
            return false;
        }

        public void MoveDown()
        {
            bool borderCollision = CollisionWithBorder(CurrentTetri, CurrentTetri.PositionX, CurrentTetri.PositionY + 1);
            bool squareCollision = CollisionWithSquare(CurrentTetri, CurrentTetri.PositionX, CurrentTetri.PositionY + 1);

            if (!borderCollision && !squareCollision)
                CurrentTetri.PositionY++;
            else
            {
                LandedTetri.Add(new CoordListingTetri(CurrentTetri));

                if (TetriLanded != null)
                    TetriLanded(null, EventArgs.Empty);

                PrepareForNextTetri();
                FindFinishedLines();

                if (ShowNextTetri != null)
                    ShowNextTetri(null, EventArgs.Empty);
            }

            if (FieldChanged != null)
                FieldChanged(null, EventArgs.Empty);
        }

        public void MoveLeft()
        {
            bool borderCollision = CollisionWithBorder(CurrentTetri, CurrentTetri.PositionX - 1, CurrentTetri.PositionY);
            bool squareCollision = CollisionWithSquare(CurrentTetri, CurrentTetri.PositionX - 1, CurrentTetri.PositionY);

            if ((CurrentTetri.PositionX > 0 || !borderCollision) && !squareCollision)
            {
                CurrentTetri.PositionX--;

                if (FieldChanged != null)
                    FieldChanged(null, EventArgs.Empty);
            }
        }

        public void MoveRight()
        {
            bool borderCollision = CollisionWithBorder(CurrentTetri, CurrentTetri.PositionX + 1, CurrentTetri.PositionY);
            bool squareCollision = CollisionWithSquare(CurrentTetri, CurrentTetri.PositionX + 1, CurrentTetri.PositionY);

            if ((CurrentTetri.PositionX < FieldSizeX - 4 || !borderCollision) && !squareCollision)
            {
                CurrentTetri.PositionX++;

                if (FieldChanged != null)
                    FieldChanged(null, EventArgs.Empty);
            }
        }

        public void RotateRight()
        {
            var (collision, moveValue) = RightRotationCollision(CurrentTetri);

            if (!collision)
            {
                CurrentTetri.PositionX += moveValue;
                CurrentTetri.RotateRight();

                if (FieldChanged != null)
                    FieldChanged(null, EventArgs.Empty);
            }
        }

        public void RotateLeft()
        {
            var (collision, moveValue) = LeftRotationCollision(CurrentTetri);

            if (!collision)
            {
                CurrentTetri.PositionX += moveValue;
                CurrentTetri.RotateLeft();

                if (FieldChanged != null)
                    FieldChanged(null, EventArgs.Empty);
            }
        }
    }
}
