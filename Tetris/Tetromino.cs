using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TetrisModel
{
    public partial class Tetromino
    {
        public bool[,] TetroMatrix { get; private set; } = new bool[4, 4];
        public Tetri TetriType;

        public int PositionX;
        public int PositionY;

        public Tetromino() {}
        public Tetromino(int beginX, int beginY)
        {
            PositionX = beginX;
            PositionY = beginY;
        }

        public Tetromino GetCopy()
        {
            Tetromino newTetri = new Tetromino();
            newTetri.TetriType = this.TetriType;
            newTetri.PositionX = this.PositionX;
            newTetri.PositionY = this.PositionY;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    newTetri.TetroMatrix[i, j] = this.TetroMatrix[i, j];
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
                    rotated[x, y] = TetroMatrix[3 - y, x];
                }
            }
            TetroMatrix = rotated;
        }

        public void RotateRight()
        {
            bool[,] rotated = new bool[4, 4];
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    rotated[x, y] = TetroMatrix[y, 3 - x];
                }
            }
            TetroMatrix = rotated;
        }

        public Coord[] ConvertToFieldCoord()
        {
            return ConvertToFieldCoord(this, this.PositionX, this.PositionY);
        }

        public static Coord[] ConvertToFieldCoord(Tetromino matrixTetri, int positionX, int positionY)
        {
            Coord[] convertedTetri = ConvertToCoord(matrixTetri, positionX, positionY);
            for (int i = 0; i < 4; i++)
            {
                convertedTetri[i].X += positionX;
                convertedTetri[i].Y += positionY;
            }
            return convertedTetri;
        }

        public static Coord[] ConvertToCoord(Tetromino matrixTetri, int positionX, int positionY)
        {
            Coord[] tetri = new Coord[4];
            int count = 0;
            for (int y = 0; y < 4; y++)
                for (int x = 0; x < 4; x++)
                {
                    if (matrixTetri.TetroMatrix[x, y] == true)
                    {
                        tetri[count].X = x;
                        tetri[count].Y = y;
                        count++;
                    }
                    if (count == 4) return tetri;
                }
            return tetri;
        }

        public Coord[] ConvertToCoord()
        {
            return ConvertToCoord(this, this.PositionX, this.PositionY);
        }

        private void CreateStandardTetri(Tetri tetri)
        {
            switch (tetri)
            {
                case Tetri.I:
                    SetFromCoordArray(I);
                    TetriType = Tetri.I;
                    break;
                case Tetri.O:
                    SetFromCoordArray(O);
                    TetriType = Tetri.O;
                    break;
                case Tetri.L:
                    SetFromCoordArray(L);
                    TetriType = Tetri.L;
                    break;
                case Tetri.J:
                    SetFromCoordArray(J);
                    TetriType = Tetri.J;
                    break;
                case Tetri.S:
                    SetFromCoordArray(S);
                    TetriType = Tetri.S;
                    break;
                case Tetri.Z:
                    SetFromCoordArray(Z);
                    TetriType = Tetri.Z;
                    break;
            }
        }

        private void SetFromCoordArray(Coord[] tetri)
        {
            for (int i = 0; i < tetri.Length; i++)
            {
                TetroMatrix[tetri[i].X, tetri[i].Y] = true;
            }
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
                    if (TetroMatrix[x, y])
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

        private void CreateRandomTetro()
        {
            Random rnd = new Random();
            TetroMatrix[2, 2] = true;
            for (int i = 0; i < 3; i++)
            {
                int column;
                int row;
                do
                {
                    column = rnd.Next(4);
                    row = rnd.Next(4);
                } 
                while (TetroMatrix[column, row] == true || !HasNeighbor(column, row));
                TetroMatrix[column, row] = true;
            }
        }

        private bool HasNeighbor(int x, int y)
        {
            if (x > 0 && TetroMatrix[x - 1, y]) return true;
            if (x < 3 && TetroMatrix[x + 1, y]) return true;
            if (y > 0 && TetroMatrix[x, y - 1]) return true;
            if (y < 3 && TetroMatrix[x, y + 1]) return true;

            return false;
        }
    }
}
