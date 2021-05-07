using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TetrisModel
{
    public class TetrisField
    {
        public int FieldSizeX { get; private set; }
        public int FieldSizeY { get; private set; }

        public MatrixTetri CurrentTetri { get; private set; }
        public MatrixTetri NextTetri { get; private set; }

        public event EventHandler TetriLanded;
        public event EventHandler FieldChanged;
        public event EventHandler ShowNextTetri;

        public List<CoordListingTetri> landedTetri { get; private set; } = new List<CoordListingTetri>();

        public TetrisField(int fieldSizeX, int fieldSizeY)
        {
            FieldSizeX = fieldSizeX;
            FieldSizeY = fieldSizeY;

            CurrentTetri = new MatrixTetri(FieldSizeX / 2 - 2, -4);
            CurrentTetri.BeRandomTetri();
            //CurrentTetri.BeStandardTetri(Tetri.I); 

            NextTetri = new MatrixTetri(0, 0);
            NextTetri.BeRandomTetri();
        }

        public StandardTetriType GetCurrentTetriType()
        {
            return CurrentTetri.StandardType;
        }
        
        /// <summary>
        /// Tries to find a place by moving the Tetri horizontally from -2 to +2.
        /// </summary>
        /// <param name="matrixTetri"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <returns>collision: true, when collision everywhere. false, when no collision when moving by moveValue</returns>
        private (bool collision, int moveValue) HorizontalPlaceFinder(MatrixTetri matrixTetri, int positionX, int positionY)
        {
            int moveValue = 0;
            bool borderCollision;
            bool squareCollision;
            int[] moveOrder = new int[] { 0, 1, -1, 2, -2};
            for (int i = 0; i < moveOrder.Length; i++)
            {
                moveValue = moveOrder[i];
                CoordListingTetri current = new CoordListingTetri(matrixTetri, positionX, positionY);
                borderCollision = CollisionWithBorder(matrixTetri, positionX + moveValue, positionY);
                squareCollision = CollisionWithSquare(matrixTetri, positionX + moveValue, positionY);
                if (!borderCollision && !squareCollision)
                    return (false, moveValue);
            }
            return (true, moveValue);
        }

        private bool CollisionWithSquare(MatrixTetri matrixTetri, int positionX, int positionY)
        {
            if (landedTetri.Count == 0)
                return false;

            CoordListingTetri current = new CoordListingTetri(matrixTetri, positionX, positionY);
            foreach (var entry in landedTetri)
            {
                for (int i = 0; i < current.Listing.Length; i++)
                    for (int j = 0; j < entry.Listing.Length; j++)
                        if (current.Listing[i].X == entry.Listing[j].X && current.Listing[i].Y == entry.Listing[j].Y)
                            return true;
            }
            return false;
        }

        private bool CollisionWithBorder(MatrixTetri matrixTetri, int positionX, int positionY)
        {
            CoordListingTetri current = new CoordListingTetri(matrixTetri, positionX, positionY);
            bool collision = false;
            var(minX, maxX, _, maxY) = current.GetRange();
            if ((minX < 0) || (maxX >= FieldSizeX) || (maxY >= FieldSizeY))
                collision = true;
            return collision;
        }

        private (bool collision, int moveValue) RightRotationCollision(MatrixTetri matrixTetri, int positionX, int positionY)
        {
            MatrixTetri rotatedTetri = matrixTetri.GetCopy();
            rotatedTetri.RotateRight();
            return HorizontalPlaceFinder(rotatedTetri, CurrentTetri.PositionX, CurrentTetri.PositionY);
        }

        private (bool collision, int moveValue) LeftRotationCollision(MatrixTetri matrixTetri, int positionX, int positionY)
        {
            MatrixTetri rotatedTetri = matrixTetri.GetCopy();
            rotatedTetri.RotateLeft();
            return HorizontalPlaceFinder(rotatedTetri, CurrentTetri.PositionX, CurrentTetri.PositionY);
        }

        public void MoveDown()
        {
            bool borderCollision = CollisionWithBorder(CurrentTetri, CurrentTetri.PositionX, CurrentTetri.PositionY + 1);
            bool squareCollision = CollisionWithSquare(CurrentTetri, CurrentTetri.PositionX, CurrentTetri.PositionY + 1);

            if (!borderCollision && !squareCollision)
                CurrentTetri.PositionY++;
            else
            {
                if (TetriLanded != null)
                    TetriLanded(null, EventArgs.Empty);

                landedTetri.Add(new CoordListingTetri(CurrentTetri));
                NextTetri.PositionX = FieldSizeX / 2 - 2;
                NextTetri.PositionY = -4;
                CurrentTetri = NextTetri;
                MatrixTetri tmp = new MatrixTetri(0, 0);
                tmp.BeRandomTetri();
                NextTetri = tmp;

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
            var (collision, moveValue) = RightRotationCollision(CurrentTetri, CurrentTetri.PositionX, CurrentTetri.PositionY);

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
            var (collision, moveValue) = LeftRotationCollision(CurrentTetri, CurrentTetri.PositionX, CurrentTetri.PositionY);

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
