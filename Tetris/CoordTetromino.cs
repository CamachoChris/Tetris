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
            SetTetri(matrixTetri, matrixTetri.PositionX, matrixTetri.PositionY);
        }

        public CoordTetromino(Tetromino matrixTetri, int positionX, int positionY)
        {
            SetTetri(matrixTetri, positionX, positionY);
        }

        public void SetTetri(Tetromino matrixTetri, int positionX, int positionY)
        {
            Tetri = Tetromino.ConvertToFieldCoord(matrixTetri, positionX, positionY);
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

        public (int minX, int maxX, int minY, int maxY) GetRange()
        {
            int minX = Tetri[0].X;
            int maxX = Tetri[0].X;
            int minY = Tetri[0].Y;
            int maxY = Tetri[0].Y;
            for (int j = 1; j < Tetri.Length; j++)
            {
                if (Tetri[j].X < minX) minX = Tetri[j].X;
                if (Tetri[j].X > maxX) maxX = Tetri[j].X;
                if (Tetri[j].Y < minY) minX = Tetri[j].Y;
                if (Tetri[j].Y > minY) maxX = Tetri[j].Y;
            }
            return (minX, maxX, minY, maxY);
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
