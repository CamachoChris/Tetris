using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TetrisModel
{

    public class PlayingFieldModel
    {
        public int FieldSizeX { get; private set; }
        public int FieldSizeY { get; private set; }

        private Tetromino CurrentTetri;
        private Tetromino NextTetri;

        private List<LandedTetromino> landedTetri = new List<LandedTetromino>();

        public PlayingFieldModel(int fieldSizeX, int fieldSizeY)
        {
            FieldSizeX = fieldSizeX;
            FieldSizeY = fieldSizeY;
            CurrentTetri = new Tetromino(FieldSizeX / 2 - 2, -4);
            NextTetri = new Tetromino(FieldSizeX / 2 - 2, -4);
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

        private static Coord[] LocateTetri(Tetromino tetri, int fieldX, int fieldY)
        {
            Coord[] someTetri = tetri.ConvertTetri();
            for (int i = 0; i < 4; i++)
            {
                someTetri[i].X = someTetri[i].X + fieldX;
                someTetri[i].Y = someTetri[i].Y + fieldY;
            }
            return someTetri;
        }

        /// <summary>
        /// Returns the distance of the left and right border. For bottom collision only bool relevant.
        /// </summary>
        /// <param name="tetri"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <returns>negative value = how deep through the border. 0 = directly at the border, still inside. 1 = rest of field.</returns>
        private (bool rotateable, int distance) CollisionDetection(Tetromino tetri, int positionX, int positionY)
        {
            bool rotateable = true;
            int distance = 1;
            var (minX, maxX, minY, maxY) = tetri.GetRange();
            if (minX + positionX <= 0)
                distance = minX + positionX;
            else if (maxX + positionX >= FieldSizeX - 1)
                distance = FieldSizeX - 1 - (maxX + positionX);

            if (maxY + positionY >= FieldSizeY)
                rotateable = false;

            return (rotateable, distance);
        }

        private bool BottomCollision(Tetromino tetri, int fieldX, int fieldY)
        {
            Coord[] current = LocateTetri(tetri, fieldX, fieldY);
            for (int i = 0; i < 4; i++)
                if (current[i].Y > FieldSizeY - 1)
                    return true;
            return false;
        }
        private (bool rotateable, int distance) LeftRotationCollision()
        {
            Tetromino tmp = CurrentTetri.GetCopy();
            tmp.RotateLeft();
            return CollisionDetection(tmp, CurrentTetri.PositionX, CurrentTetri.PositionY);
        }
        private (bool rotateable, int distance) RightRotationCollision()
        {
            Tetromino tmp = CurrentTetri.GetCopy();
            tmp.RotateRight();
            return CollisionDetection(tmp, CurrentTetri.PositionX, CurrentTetri.PositionY);
        }

        public void MoveDown()
        {
            if (!BottomCollision(CurrentTetri, CurrentTetri.PositionX, CurrentTetri.PositionY + 1))
                CurrentTetri.PositionY++;
            else
            {
                landedTetri.Add(new LandedTetromino(CurrentTetri));
                landedTetri[landedTetri.Count-1].Show();
                CurrentTetri = NextTetri;
                Tetromino tmp = new Tetromino(FieldSizeX / 2 - 2, -4);
                tmp.BeRandomTetri();
                NextTetri = tmp;
            }
        }
        public void MoveLeft()
        {
            var (rotateable, distance) = CollisionDetection(CurrentTetri, CurrentTetri.PositionX, CurrentTetri.PositionY);
            if (CurrentTetri.PositionX > 0 || distance == 1)
                CurrentTetri.PositionX--;
        }
        public void MoveRight()
        {
            var (rotateable, distance) = CollisionDetection(CurrentTetri, CurrentTetri.PositionX, CurrentTetri.PositionY);
            if (CurrentTetri.PositionX < FieldSizeX - 4 || distance == 1)
                CurrentTetri.PositionX++;
        }
        public void RotateRight()
        {
            var (rotateable, distance) = RightRotationCollision();

            if (!rotateable) return;

            if (distance < 0)
            {
                distance = Math.Abs(distance);
                if (CurrentTetri.PositionX > FieldSizeX / 2) CurrentTetri.PositionX -= distance;
                if (CurrentTetri.PositionX < FieldSizeX / 2) CurrentTetri.PositionX += distance;
            }
            CurrentTetri.RotateRight();
        }
        public void RotateLeft()
        {
            var (rotateable, distance) = LeftRotationCollision();

            if (!rotateable) return;

            if (distance < 0)
            {
                distance = Math.Abs(distance);
                if (CurrentTetri.PositionX > FieldSizeX / 2) CurrentTetri.PositionX -= distance;
                if (CurrentTetri.PositionX < FieldSizeX / 2) CurrentTetri.PositionX += distance;
            }
            CurrentTetri.RotateLeft();
        }
    }
}
