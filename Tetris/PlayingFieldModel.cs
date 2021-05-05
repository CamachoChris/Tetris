using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TetrisModel
{
    public class TetriView : TetrominoModel
    {
        public int PositionX;
        public int PositionY;

        public TetriView() {}
        public TetriView(int beginX, int beginY)
        {
            PositionX = beginX;
            PositionY = beginY;
        }
    }

    public class PlayingFieldModel
    {
        public int FieldSizeX { get; private set; }
        public int FieldSizeY { get; private set; }

        private TetriView CurrentTetri; 
        private TetriView NextTetri;

        public PlayingFieldModel(int fieldSizeX, int fieldSizeY)
        {
            FieldSizeX = fieldSizeX;
            FieldSizeY = fieldSizeY;
            CurrentTetri = new TetriView(FieldSizeX / 2 - 2, -4);
            NextTetri = new TetriView(FieldSizeX / 2 - 2, -4);
            CurrentTetri.BeStandardTetri(Tetri.I); //BeRandomTetri();
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

        private bool LeftCollision(TetrominoModel tetri, int positionX, int positionY)
        {
            if (positionX >= 0)
                return false;
            Coord[] current = LocateTetri(tetri, positionX, positionY);
            for (int i = 0; i < 4; i++)
                if (current[i].X < 0)
                    return true;
            return false;
        }
        private bool RightCollision(TetrominoModel tetri, int positionX, int positionY)
        {
            if (positionX <= FieldSizeX - 4)
                return false;
            Coord[] current = LocateTetri(tetri, positionX, positionY);
            for (int i = 0; i < 4; i++)
                if (current[i].X > FieldSizeX - 1)
                    return true;
            return false;
        }
        private bool BottomCollision(TetrominoModel tetri, int fieldX, int fieldY)
        {
            Coord[] current = LocateTetri(tetri, fieldX, fieldY);
            for (int i = 0; i < 4; i++)
                if (current[i].Y > FieldSizeY - 1)
                    return true;
            return false;
        }
        private bool LeftRotationCollision()
        {
            TetrominoModel tmp = CurrentTetri.GetCopy();
            tmp.RotateLeft();
            return SideCollision(tmp);
        }
        private bool RightRotationCollision()
        {
            TetrominoModel tmp = CurrentTetri.GetCopy();
            tmp.RotateRight();            
            return SideCollision(tmp);
        }

        private bool SideCollision(TetrominoModel tetri)
        {
            bool didCollideRight = false;
            bool didCollideLeft = false;
            didCollideRight = RightCollision(tetri, CurrentTetri.PositionX, CurrentTetri.PositionY);
            didCollideLeft = LeftCollision(tetri, CurrentTetri.PositionX, CurrentTetri.PositionY);
            if (didCollideLeft || didCollideRight)
                return true;
            else
                return false;
        }

        public void MoveDown()
        {
            if (!BottomCollision(CurrentTetri, CurrentTetri.PositionX, CurrentTetri.PositionY + 1))
                CurrentTetri.PositionY++;
            else
            {
                CurrentTetri = NextTetri;
                NextTetri.BeRandomTetri();
            }
        }
        public void MoveLeft()
        {
            if (!LeftCollision(CurrentTetri, CurrentTetri.PositionX - 1, CurrentTetri.PositionY))
                CurrentTetri.PositionX--;
        }
        public void MoveRight()
        {
            if (!RightCollision(CurrentTetri, CurrentTetri.PositionX + 1, CurrentTetri.PositionY))
                CurrentTetri.PositionX++;
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
