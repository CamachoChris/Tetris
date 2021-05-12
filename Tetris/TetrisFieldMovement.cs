using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TetrisModel
{
    public partial class TetrisField
    {
        public void MoveDown()
        {
            if (!_gameRunning) return;

            _finishedLines += SeekAndDestroyFinishedLines();
            if (LetThemFall() && TetriFieldChanged != null)
                TetriFieldChanged(null, EventArgs.Empty);

            bool gameOver = GameOverCheck(CurrentTetri, CurrentTetri.PositionX, CurrentTetri.PositionY + 1);
            if (gameOver)
            {
                GameOver();

                if (TetriGameOver != null)
                    TetriGameOver(null, EventArgs.Empty);

                return;
            }

            bool borderCollision = CollisionWithBorder(CurrentTetri, CurrentTetri.PositionX, CurrentTetri.PositionY + 1);
            bool squareCollision = CollisionWithSquare(CurrentTetri, CurrentTetri.PositionX, CurrentTetri.PositionY + 1);
            if (!borderCollision && !squareCollision)
            {
                CurrentTetri.PositionY++;

                if (TetriMoved != null)
                    TetriMoved(null, EventArgs.Empty);
            }
            else
            {
                LandedTetri.Add(new CoordListingTetri(CurrentTetri));

                PrepareForNextTetri();

                //if (finishedLines > 0)
                //    LetThemFall();

                _finishedLines += SeekAndDestroyFinishedLines();
                if (_finishedLines/5+1 > _level)
                {
                    _level = _finishedLines / 5 + 1;
                    SpeedUp();
                    TetriGameLevelUp(_level, EventArgs.Empty);
                }

                if (TetriLanded != null)
                    TetriLanded(null, EventArgs.Empty);
            }
        }

        public void MoveLeft()
        {
            if (!_gameRunning) return;

            bool borderCollision = CollisionWithBorder(CurrentTetri, CurrentTetri.PositionX - 1, CurrentTetri.PositionY);
            bool squareCollision = CollisionWithSquare(CurrentTetri, CurrentTetri.PositionX - 1, CurrentTetri.PositionY);

            if ((CurrentTetri.PositionX > 0 || !borderCollision) && !squareCollision)
            {
                CurrentTetri.PositionX--;

                if (TetriMoved != null)
                    TetriMoved(null, EventArgs.Empty);
            }
        }

        public void MoveRight()
        {
            if (!_gameRunning) return;

            bool borderCollision = CollisionWithBorder(CurrentTetri, CurrentTetri.PositionX + 1, CurrentTetri.PositionY);
            bool squareCollision = CollisionWithSquare(CurrentTetri, CurrentTetri.PositionX + 1, CurrentTetri.PositionY);

            if ((CurrentTetri.PositionX < FieldSizeX - 4 || !borderCollision) && !squareCollision)
            {
                CurrentTetri.PositionX++;

                if (TetriMoved != null)
                    TetriMoved(null, EventArgs.Empty);
            }
        }

        public void RotateRight()
        {
            if (!_gameRunning) return;

            var (collision, moveValue) = RightRotationCollision(CurrentTetri);

            if (!collision)
            {
                CurrentTetri.PositionX += moveValue;
                CurrentTetri.RotateRight();

                if (TetriMoved != null)
                    TetriMoved(null, EventArgs.Empty);
            }
        }

        public void RotateLeft()
        {
            if (!_gameRunning) return;

            var (collision, moveValue) = LeftRotationCollision(CurrentTetri);

            if (!collision)
            {
                CurrentTetri.PositionX += moveValue;
                CurrentTetri.RotateLeft();

                if (TetriMoved != null)
                    TetriMoved(null, EventArgs.Empty);
            }
        }
    }
}
