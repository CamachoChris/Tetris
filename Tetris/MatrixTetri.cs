﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TetrisModel
{
    public partial class MatrixTetri
    {
        public bool[,] Matrix { get; private set; } = new bool[4, 4];
        public StandardTetriType StandardType;

        private int _possibleRotations;
        private int _currentState;

        public int PositionX;
        public int PositionY;

        public MatrixTetri() {}

        public MatrixTetri(int startPositionX, int startPositionY)
        {
            PositionX = startPositionX;
            PositionY = startPositionY;
        }

        public MatrixTetri GetCopy()
        {
            MatrixTetri copiedMatrixTetri = new MatrixTetri
            {
                StandardType = this.StandardType,
                PositionX = this.PositionX,
                PositionY = this.PositionY,
                _currentState = this._currentState,
                _possibleRotations = this._possibleRotations                
            };

            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                {
                    copiedMatrixTetri.Matrix[x, y] = this.Matrix[x, y];
                }

            return copiedMatrixTetri;
        }

        public void BeRandomStandardTetri()
        {
            Random rnd = new Random();
            int standardTetriPosition = rnd.Next(7);
            CreateStandardTetri((StandardTetriType)standardTetriPosition);

            int rndRotate = rnd.Next(4);
            for (int i = 0; i < rndRotate; i++)            
                RotateRight();                       
        }

        public void BeThatStandardTetri(StandardTetriType tetriPosition)
        {
            CreateStandardTetri(tetriPosition);
        }

        public void RotateLeft()
        {
            if (_possibleRotations == 0)
                return;

            if (_possibleRotations == 1)
            {
                if (_currentState == 0)
                {
                    MatrixRotationLeft();
                    _currentState = 1;
                }
                else if (_currentState == 1)
                {
                    MatrixRotationRight();
                    _currentState = 0;
                }
            }

            if (_possibleRotations == 4)
                MatrixRotationLeft();
        }

        public void RotateRight()
        {
            if (_possibleRotations == 0)
                return;

            if (_possibleRotations == 1)
            {
                if (_currentState == 0)
                {
                    MatrixRotationRight();
                    _currentState = 1;
                }
                else if (_currentState == 1)
                {
                    MatrixRotationLeft();
                    _currentState = 0;
                }
            }

            if (_possibleRotations == 4)
                MatrixRotationRight();
        }

        private void MatrixRotationLeft()
        {
            bool[,] rotatedMatrix = new bool[4, 4];
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    rotatedMatrix[x, y] = Matrix[3 - y, x];
                }
            }
            Matrix = rotatedMatrix;
        }

        private void MatrixRotationRight()
        {
            bool[,] rotatedMatrix = new bool[4, 4];
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    rotatedMatrix[x, y] = Matrix[y, 3 - x];
                }
            }
            Matrix = rotatedMatrix;
        }

        private void CreateStandardTetri(StandardTetriType tetriPosition)
        {
            switch (tetriPosition)
            {
                case StandardTetriType.I:
                    SetFromCoordArray(I);
                    StandardType = StandardTetriType.I;
                    _possibleRotations = 1;
                    break;
                case StandardTetriType.O:
                    SetFromCoordArray(O);
                    StandardType = StandardTetriType.O;
                    _possibleRotations = 0;
                    break;
                case StandardTetriType.L:
                    SetFromCoordArray(L);
                    StandardType = StandardTetriType.L;
                    _possibleRotations = 4;
                    break;
                case StandardTetriType.J:
                    SetFromCoordArray(J);
                    StandardType = StandardTetriType.J;
                    _possibleRotations = 4;
                    break;
                case StandardTetriType.S:
                    SetFromCoordArray(S);
                    StandardType = StandardTetriType.S;
                    _possibleRotations = 1;
                    break;
                case StandardTetriType.Z:
                    SetFromCoordArray(Z);
                    StandardType = StandardTetriType.Z;
                    _possibleRotations = 1;
                    break;
                case StandardTetriType.T:
                    SetFromCoordArray(T);
                    StandardType = StandardTetriType.T;
                    _possibleRotations = 4;
                    break;
            }
        }

        private void SetFromCoordArray(Coord[] tetri)
        {
            for (int i = 0; i < tetri.Length; i++)
            {
                Matrix[tetri[i].X, tetri[i].Y] = true;
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
                    if (Matrix[x, y])
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

        public void BeRandomTetro()
        {
            Random rnd = new Random();
            Matrix[2, 2] = true;
            for (int i = 0; i < 3; i++)
            {
                int column;
                int row;
                do
                {
                    column = rnd.Next(4);
                    row = rnd.Next(4);
                } 
                while (Matrix[column, row] == true || !HasNeighbor(column, row));
                Matrix[column, row] = true;
            }
        }

        private bool HasNeighbor(int x, int y)
        {
            if (x > 0 && Matrix[x - 1, y]) return true;
            if (x < 3 && Matrix[x + 1, y]) return true;
            if (y > 0 && Matrix[x, y - 1]) return true;
            if (y < 3 && Matrix[x, y + 1]) return true;

            return false;
        }
    }
}
