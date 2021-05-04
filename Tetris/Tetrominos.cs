using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TetrisModel
{
    public partial class TetrominoModel
    {
        public bool[,] Position { get; private set; } = new bool[4, 4];

        public TetrominoModel() {}
        public TetrominoModel(Tetri tetri)
        {
            CreateStandardTetri(tetri);
        }

        public void CreateRandomTetri()
        {
            Random rnd = new Random();
            int nextTetri = rnd.Next(6);
            CreateStandardTetri((Tetri)nextTetri);

            int randRotate = rnd.Next(4);
            for (int i = 0; i < randRotate; i++)            
                RotateRight();                       
        }

        public void RotateLeft()
        {
            bool[,] rotated = new bool[4, 4];
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    rotated[x, y] = Position[3 - y, x];
                }
            }
            Position = rotated;
        }

        public void RotateRight()
        {
            bool[,] rotated = new bool[4, 4];
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    rotated[x, y] = Position[y, 3 - x];
                }
            }
            Position = rotated;
        }

        private void CreateStandardTetri(Tetri tetri)
        {
            switch (tetri)
            {
                case Tetri.I:
                    CopyStandardTetri(I);
                    break;
                case Tetri.O:
                    CopyStandardTetri(O);
                    break;
                case Tetri.L:
                    CopyStandardTetri(L);
                    break;
                case Tetri.J:
                    CopyStandardTetri(J);
                    break;
                case Tetri.S:
                    CopyStandardTetri(S);
                    break;
                case Tetri.Z:
                    CopyStandardTetri(Z);
                    break;
                default:
                    break;
            }
        }

        private void CopyStandardTetri(Coord[] tetri)
        {
            for (int i = 0; i < 4; i++)
            {
                Position[tetri[i].X, tetri[i].Y] = true;
            }
        }

        private void CreateRandomTetro()
        {
            Random rnd = new Random();
            Position[2, 2] = true;
            for (int i = 0; i < 3; i++)
            {
                int column;
                int row;
                do
                {
                    column = rnd.Next(4);
                    row = rnd.Next(4);
                } 
                while (Position[column, row] == true || !HasNeighbor(column, row));
                Position[column, row] = true;
            }
        }
        private bool HasNeighbor(int x, int y)
        {
            if (x > 0 && Position[x - 1, y]) return true;
            if (x < 3 && Position[x + 1, y]) return true;
            if (y > 0 && Position[x, y - 1]) return true;
            if (y < 3 && Position[x, y + 1]) return true;

            return false;
        }
    }
}
