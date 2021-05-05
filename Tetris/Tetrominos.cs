using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TetrisModel
{
    public partial class TetrominoModel
    {
        public bool[,] Position { get; private set; } = new bool[4, 4];
        public Tetri TetriType;
        public TetrominoModel() {}
        public TetrominoModel(Tetri tetri)
        {
            CreateStandardTetri(tetri);
        }

        public TetrominoModel GetCopy()
        {
            TetrominoModel newTetri = new TetrominoModel();
            newTetri.TetriType = this.TetriType;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    newTetri.Position[i, j] = this.Position[i, j];
                }
            return newTetri;
        }

        public void BeRandomTetri()
        {
            Random rnd = new Random();
            int nextTetri = rnd.Next(6);
            CreateStandardTetri((Tetri)nextTetri);

            int randRotate = rnd.Next(4);
            for (int i = 0; i < randRotate; i++)            
                RotateRight();                       
        }
        public void BeStandardTetri(Tetri tetri)
        {
            CreateStandardTetri(tetri);
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

        public Coord[] ConvertTetri()
        {
            Coord[] tetri = new Coord[4];
            int count = 0;
            for (int y = 0; y < 4; y++)
                for (int x = 0; x < 4; x++)
                {
                    if (Position[x, y] == true)
                    {
                        tetri[count].X = x;
                        tetri[count].Y = y;
                        count++;
                    }
                    if (count == 4) return tetri;
                }
            return tetri;
        }
        private void CreateStandardTetri(Tetri tetri)
        {
            switch (tetri)
            {
                case Tetri.I:
                    CopyStandardTetri(I);
                    TetriType = Tetri.I;
                    break;
                case Tetri.O:
                    CopyStandardTetri(O);
                    TetriType = Tetri.O;
                    break;
                case Tetri.L:
                    CopyStandardTetri(L);
                    TetriType = Tetri.L;
                    break;
                case Tetri.J:
                    CopyStandardTetri(J);
                    TetriType = Tetri.J;
                    break;
                case Tetri.S:
                    CopyStandardTetri(S);
                    TetriType = Tetri.S;
                    break;
                case Tetri.Z:
                    CopyStandardTetri(Z);
                    TetriType = Tetri.Z;
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
        public (int minX, int maxX, int minY, int maxY) GetRange()
        {
            int minX = 2;
            int maxX = 1;
            int minY = 2;
            int maxY = 1;
            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                {
                    if (Position[x, y])
                    {
                        if (x < minX)
                            minX = x;
                        if (x > maxX)
                            maxX = x;
                        if (y < minY)
                            minY = y;
                        if (y > maxY)
                            maxY = y;
                    }
                }
            return (minX, maxX, minY, maxY);
        }
    }
}
