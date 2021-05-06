using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TetrisModel
{
    public class CoordTetromino
    {
        public Tetri TetriType;
        public Coord[] Tetri;

        public CoordTetromino()
        {
            Tetri = new Coord[4];
        }

        public CoordTetromino(Tetromino matrixTetri)
        {
            SetTetri(matrixTetri);
        }

        public void SetTetri(Tetromino matrixTetri)
        {
            Tetri = matrixTetri.ConvertToFieldCoord();
            TetriType = matrixTetri.TetriType;
        }

        public void RemoveAt(int index)
        {
            Coord[] newCoord = new Coord[Tetri.Length - 1];
            int newArrayCount = 0;
            for (int i = 0; i < Tetri.Length; i++)
            {
                if (i != index)
                    newCoord[newArrayCount++] = Tetri[i];
            }
            Tetri = newCoord;
        }

        public void Show()
        {
            for (int i = 0; i < 4; i++)
            {
                Debug.Write($"x:{Tetri[i].X} y:{Tetri[i].Y}, ");
            }
            Debug.WriteLine(TetriType);
        }
    }
}
