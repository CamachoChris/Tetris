using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TetrisModel
{
    public partial class TetrisField
    {
        /// <summary>
        /// Does the whole work around finished lines
        /// </summary>
        /// <returns>Returns the amount of found finished lines</returns>
        private int LineFinishCheck()
        {
            int currentFinishedLines = SeekAndDestroyFinishedLines();
            if (currentFinishedLines > 0)
            {
                switch (currentFinishedLines)
                {
                    case 1:
                        _score += 1;
                        break;
                    case 2:
                        _score += 4;
                        break;
                    case 3:
                        _score += 9;
                        break;
                    case 4:
                        _score += 32;
                        break;
                }
                TetriGameScoreChange(_score, EventArgs.Empty);

                _finishedLinesCount += currentFinishedLines;
                if (_finishedLinesCount / 5 + 1 > _level)
                {
                    _level = _finishedLinesCount / 5 + 1;
                    SpeedUp();
                    TetriGameLevelUp(_level, EventArgs.Empty);
                }
            }
            return currentFinishedLines;
        }

        public void MovingField()
        {
            if (!_gameRunning) return;

            if (LetThemFall() || LineFinishCheck() > 0)
            {
                if (TetriFieldChanged != null)
                    TetriFieldChanged(null, EventArgs.Empty);
            }
        }

        public void MoveDown()
        {
            if (!_gameRunning) return;

            if (IsAnyFalling())
                return;

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
                LineFinishCheck();

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
