using System;
using System.Collections.Generic;
using System.Text;

namespace TetrisModel
{
    public partial class TetrisField
    {
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

                SeekAndDestroyFinishedLines();

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
