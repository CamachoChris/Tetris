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

        private static Coord[] LocateTetri(Tetromino tetri, int positionX, int positionY)
        {
            Coord[] someTetri = tetri.ConvertToCoord();
            for (int i = 0; i < 4; i++)
            {
                someTetri[i].X = someTetri[i].X + positionX;
                someTetri[i].Y = someTetri[i].Y + positionY;
            }
            return someTetri;
        }
        
        private bool IsSquareFree(Tetromino tetri, int positionX, int positionY)
        {
            if (landedTetri.Count == 0)
                return true;
            Coord[] current = LocateTetri(tetri, positionX, positionY);
            foreach (var entry in landedTetri)
            {
                for (int i = 0; i < current.Length; i++)
                    for (int j = 0; j < entry.Tetri.Length; j++)
                    {
                        if (current[i].X == entry.Tetri[j].X && current[i].Y == entry.Tetri[j].Y)
                            return false;
                    }
            }
            return true;
        }

        /// <summary>
        /// Returns the distance of the left and right border. For bottom collision only bool relevant.
        /// </summary>
        /// <param name="tetri"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <returns>negative value = how deep through the border. 0 = directly at the border, still inside. 1 = rest of field.</returns>
        private (bool freeway, int distance) CollisionDetection(Tetromino tetri, int positionX, int positionY)
        {
            bool freeway = true;
            int distance = 1;
            var (minX, maxX, minY, maxY) = tetri.GetRange();
            if (minX + positionX <= 0)
                distance = minX + positionX;
            else if (maxX + positionX >= FieldSizeX - 1)
                distance = FieldSizeX - 1 - (maxX + positionX);

            freeway = IsSquareFree(tetri, positionX, positionY);

            if (maxY + positionY >= FieldSizeY)
                freeway = false;

            return (freeway, distance);
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

        private (bool freeway, int distance) LeftRotationCollision()
        {
            Tetromino tmp = CurrentTetri.GetCopy();
            tmp.RotateLeft();
            return CollisionDetection(tmp, CurrentTetri.PositionX, CurrentTetri.PositionY);
        }

        private (bool freeway, int distance) RightRotationCollision()
        {
            Tetromino tmp = CurrentTetri.GetCopy();
            tmp.RotateRight();
            return CollisionDetection(tmp, CurrentTetri.PositionX, CurrentTetri.PositionY);
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
            var (freeway, distance) = RightRotationCollision();

            if (!freeway) return;

            if (distance < 0)
            {
                distance = Math.Abs(distance);
                if (CurrentTetri.PositionX > FieldSizeX / 2) 
                {
                    Tetromino tmp = CurrentTetri.GetCopy();
                    tmp.RotateRight();
                    if (IsSquareFree(tmp, CurrentTetri.PositionX - distance, CurrentTetri.PositionY))
                        CurrentTetri.PositionX -= distance;
                }
                if (CurrentTetri.PositionX < FieldSizeX / 2)
                {
                    Tetromino tmp = CurrentTetri.GetCopy();
                    tmp.RotateRight();
                    if (IsSquareFree(tmp, CurrentTetri.PositionX + distance, CurrentTetri.PositionY))
                        CurrentTetri.PositionX += distance;
                }
            }
            CurrentTetri.RotateRight();
            if (FieldChanged != null)
                FieldChanged(null, EventArgs.Empty);
        }

        public void RotateLeft()
        {
            var (freeway, distance) = LeftRotationCollision();

            if (!freeway) return;

            if (distance < 0)
            {
                distance = Math.Abs(distance);
                if (CurrentTetri.PositionX > FieldSizeX / 2)
                {
                    Tetromino tmp = CurrentTetri.GetCopy();
                    tmp.RotateLeft();
                    if (IsSquareFree(tmp, CurrentTetri.PositionX - distance, CurrentTetri.PositionY))
                        CurrentTetri.PositionX -= distance;
                }
                if (CurrentTetri.PositionX < FieldSizeX / 2)
                {
                    Tetromino tmp = CurrentTetri.GetCopy();
                    tmp.RotateLeft();
                    if (IsSquareFree(tmp, CurrentTetri.PositionX + distance, CurrentTetri.PositionY))
                        CurrentTetri.PositionX += distance;
                }
            }
            CurrentTetri.RotateLeft();
            if (FieldChanged != null)
                FieldChanged(null, EventArgs.Empty);
        }
    }
}
