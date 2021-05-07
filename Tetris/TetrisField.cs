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

        public Tetromino CurrentTetri;
        public Tetromino NextTetri;

        public event EventHandler TetriLanded;
        public event EventHandler FieldChanged;
        public event EventHandler ShowNextTetri;

        public List<CoordTetromino> landedTetri = new List<CoordTetromino>();

        public TetrisField(int fieldSizeX, int fieldSizeY)
        {
            FieldSizeX = fieldSizeX;
            FieldSizeY = fieldSizeY;
            CurrentTetri = new Tetromino(FieldSizeX / 2 - 2, -4);
            //NextTetri = new Tetromino(FieldSizeX / 2 - 2, -4);
            NextTetri = new Tetromino(0, 0);
            //CurrentTetri.BeStandardTetri(Tetri.I); 
            CurrentTetri.BeRandomTetri();
            NextTetri.BeRandomTetri();
        }

        public Tetri GetCurrentTetriType()
        {
            return CurrentTetri.TetriType;
        }

        public Coord[] LocateCurrentTetri()
        {
            return LocateTetri(CurrentTetri, CurrentTetri.PositionX, CurrentTetri.PositionY);
        }

        private static Coord[] LocateTetri(Tetromino matrixTetri, int positionX, int positionY)
        {
            Coord[] someTetri = matrixTetri.ConvertToCoord();
            for (int i = 0; i < 4; i++)
            {
                someTetri[i].X = someTetri[i].X + positionX;
                someTetri[i].Y = someTetri[i].Y + positionY;
            }
            return someTetri;
        }
        
        /// <summary>
        /// Tries to find a place by moving the Tetri horizontally from -2 to +2.
        /// </summary>
        /// <param name="matrixTetri"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <returns>collision: true, when collision everywhere. false, when no collision when moving by moveValue</returns>
        private (bool collision, int moveValue) HorizontalPlaceFinder(Tetromino matrixTetri, int positionX, int positionY)
        {
            int moveValue = 0;
            bool borderCollision;
            bool squareCollision;
            int[] moveOrder = new int[] { 0, 1, -1, 2, -2, 3, -3 };
            for (int i = 0; i < moveOrder.Length; i++)
            {
                moveValue = moveOrder[i];
                CoordTetromino current = new CoordTetromino(matrixTetri, positionX, positionY);
                borderCollision = CollisionWithBorder(matrixTetri, positionX + moveValue, positionY);
                squareCollision = CollisionWithSquare(matrixTetri, positionX + moveValue, positionY);
                if (!borderCollision && !squareCollision)
                    return (false, moveValue);
            }
            return (true, moveValue);
        }

        private bool CollisionWithSquare(Tetromino matrixTetri, int positionX, int positionY)
        {
            if (landedTetri.Count == 0)
                return false;

            CoordTetromino current = new CoordTetromino(matrixTetri, positionX, positionY);
            foreach (var entry in landedTetri)
            {
                for (int i = 0; i < current.Tetri.Length; i++)
                    for (int j = 0; j < entry.Tetri.Length; j++)
                    {
                        if (current.Tetri[i].X == entry.Tetri[j].X && current.Tetri[i].Y == entry.Tetri[j].Y)
                            return true;
                    }
            }
            return false;
        }

        private bool CollisionWithBorder(Tetromino matrixTetri, int positionX, int positionY)
        {
            bool collision = false;
            CoordTetromino current = new CoordTetromino(matrixTetri, positionX, positionY);
            var(minX, maxX, _, maxY) = current.GetRange();
            if ((minX < 0) || (maxX >= FieldSizeX) || (maxY >= FieldSizeY))
                collision = true;
            return collision;
        }

        private (bool collision, int moveValue) RightRotationCollision(Tetromino matrixTetri, int positionX, int positionY)
        {
            Tetromino rotatedTetri = matrixTetri.GetCopy();
            rotatedTetri.RotateRight();
            return HorizontalPlaceFinder(rotatedTetri, CurrentTetri.PositionX, CurrentTetri.PositionY);
        }

        private (bool collision, int moveValue) LeftRotationCollision(Tetromino matrixTetri, int positionX, int positionY)

        {
            Tetromino rotatedTetri = matrixTetri.GetCopy();
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

                landedTetri.Add(new CoordTetromino(CurrentTetri));
                NextTetri.PositionX = FieldSizeX / 2 - 2;
                NextTetri.PositionY = -4;
                CurrentTetri = NextTetri;
                Tetromino tmp = new Tetromino(0, 0);
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
