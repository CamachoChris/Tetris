using System;
using System.Collections.Generic;
using System.Text;

namespace TetrisModel
{
    class PlayingFieldModel
    {
        public int FieldSizeX { get; private set; }
        public int FieldSizeY { get; private set; }

        private TetrominoModel CurrentTetri = new TetrominoModel(); 
        private TetrominoModel NextTetri = new TetrominoModel();

        public int TetriPositionX { get; private set; }
        public int TetriPositionY { get; private set; }

        public PlayingFieldModel(int fieldSizeX, int fieldSizeY)
        {
            FieldSizeX = fieldSizeX;
            FieldSizeY = fieldSizeY;
            CurrentTetri.BeRandomTetri();
            NextTetri.BeRandomTetri();

            TetriPositionX = FieldSizeX / 2 - 2;
            TetriPositionY = -4;
        }

        public Tetri GetCurrentTetriType()
        {
            return CurrentTetri.TetriType;
        }

        public Coord[] LocateCurrentTetri()
        {
            Coord[] fieldTetri = CurrentTetri.ConvertTetri();
            for (int i = 0; i < 4; i++)
            {
                fieldTetri[i].X = fieldTetri[i].X + TetriPositionX;
                fieldTetri[i].Y = fieldTetri[i].Y + TetriPositionY;
            }
            return fieldTetri;
        }
        private bool LeftCollision()
        {
            Coord[] current = LocateCurrentTetri();
            for (int i = 0; i < 4; i++)
                if (current[i].X == 0)
                    return true;
            return false;
        }
        private bool RightCollision()
        {
            Coord[] current = LocateCurrentTetri();
            for (int i = 0; i < 4; i++)
                if (current[i].X == FieldSizeX - 1)
                    return true;
            return false;
        }
        private bool BottomCollision()
        {
            Coord[] current = LocateCurrentTetri();
            for (int i = 0; i < 4; i++)
                if (current[i].Y == FieldSizeY - 1)
                    return true;
            return false;
        }
        public void MoveDown()
        {
            if (!BottomCollision())
                TetriPositionY++;
        }
        public void MoveLeft()
        {
            if (!LeftCollision())
                TetriPositionX--;
        }
        public void MoveRight()
        {
            if (!RightCollision())
                TetriPositionX++;
        }
        public void RotateRight()
        {
            CurrentTetri.RotateRight();
        }
        public void RotateLeft()
        {
            CurrentTetri.RotateLeft();
        }
    }
}
