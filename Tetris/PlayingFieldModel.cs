using System;
using System.Collections.Generic;
using System.Text;

namespace TetrisModel
{
    class PlayingFieldModel
    {
        public int FieldSizeX { get; private set; }
        public int FieldSizeY { get; private set; }

        public TetrominoModel CurrentTetri = new TetrominoModel();
        public TetrominoModel NextTetri = new TetrominoModel();

        public int TetriPositionX { get; private set; }
        public int TetriPositionY { get; private set; }

        public PlayingFieldModel(int fieldSizeX, int fieldSizeY)
        {
            FieldSizeX = fieldSizeX;
            FieldSizeY = fieldSizeY;
            CurrentTetri.BeRandomTetri();
            NextTetri.BeRandomTetri();

            TetriPositionX = FieldSizeX / 2 - 2;
            TetriPositionY = -2;
        }
        public Coord[] GetTetri()
        {
            Coord[] fieldTetri = CurrentTetri.ConvertTetri();
            for (int i = 0; i < 4; i++)
            {
                int tmpX = fieldTetri[i].X;
                int tmpY = fieldTetri[i].Y;
                fieldTetri[i].X = fieldTetri[i].X + TetriPositionX;
                fieldTetri[i].Y = fieldTetri[i].Y + TetriPositionY;
                int tmpafterX = fieldTetri[i].X;
                int tmpafterY = fieldTetri[i].Y;
            }
            return fieldTetri;
        }
        public void MoveDown()
        {
            TetriPositionY++;
        }
    }
}
