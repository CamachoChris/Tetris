using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

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
            CurrentTetri.BeStandardTetri(Tetri.I); //BeRandomTetri();
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
            return LocateTetri(CurrentTetri, TetriPositionX, TetriPositionY);
        }

        private static Coord[] LocateTetri(TetrominoModel tetri, int fieldX, int fieldY)
        {
            Coord[] someTetri = tetri.ConvertTetri();
            for (int i = 0; i < 4; i++)
            {
                someTetri[i].X = someTetri[i].X + fieldX;
                someTetri[i].Y = someTetri[i].Y + fieldY;
            }
            return someTetri;
        }

        private bool LeftCollision(TetrominoModel tetri, int fieldX, int fieldY)
        {
            Coord[] current = LocateTetri(tetri, fieldX, fieldY);
            for (int i = 0; i < 4; i++)
                if (current[i].X < 0)
                    return true;
            return false;
        }
        private bool RightCollision(TetrominoModel tetri, int fieldX, int fieldY)
        {
            Coord[] current = LocateTetri(tetri, fieldX, fieldY);
            for (int i = 0; i < 4; i++)
                if (current[i].X > FieldSizeX - 1)
                    return true;
            return false;
        }
        private bool BottomCollision(TetrominoModel tetri, int fieldX, int fieldY)
        {
            Coord[] current = LocateTetri(tetri, fieldX, fieldY);
            for (int i = 0; i < 4; i++)
                if (current[i].Y == FieldSizeY - 1)
                    return true;
            return false;
        }
        private bool LeftRotationCollision()
        {
            TetrominoModel tmp = CurrentTetri.GetCopy();
            tmp.RotateLeft();
            bool didCollide = LeftCollision(tmp, TetriPositionX, TetriPositionY);
            if (didCollide)
            {
                TetriPositionX++;
            }
            return false;
        }
        private bool RightRotationCollision()
        {
            TetrominoModel tmp = CurrentTetri.GetCopy();
            tmp.RotateRight();
            bool didCollide = RightCollision(tmp, TetriPositionX, TetriPositionY);
            if (didCollide)
            {
                TetriPositionX--;
            }
            return false;
        }
        public void MoveDown()
        {
            if (!BottomCollision(CurrentTetri, TetriPositionX, TetriPositionY))
                TetriPositionY++;
        }
        public void MoveLeft()
        {
            if (!LeftCollision(CurrentTetri, TetriPositionX-1, TetriPositionY))
                TetriPositionX--;
        }
        public void MoveRight()
        {
            if (!RightCollision(CurrentTetri, TetriPositionX+1, TetriPositionY))
                TetriPositionX++;
        }
        public void RotateRight()
        {
            if (!RightRotationCollision())
                CurrentTetri.RotateRight();
        }
        public void RotateLeft()
        {
            if (!LeftRotationCollision())
                CurrentTetri.RotateLeft();
        }
    }
}
