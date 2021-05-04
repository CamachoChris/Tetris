using System;
using System.Collections.Generic;
using System.Text;

namespace TetrisModel
{
    public partial class TetrominoModel
    {
        public enum Tetri
        {
            I, O, L, J, S, Z
        }

        struct Coord
        {
            public int X;
            public int Y;
        }
        private readonly Coord[] I = {
            new Coord() { X = 2, Y = 0 },
            new Coord() { X = 2, Y = 1 },
            new Coord() { X = 2, Y = 2 },
            new Coord() { X = 2, Y = 3 },
        };
        private readonly Coord[] O = {
            new Coord() { X = 1, Y = 1 },
            new Coord() { X = 2, Y = 1 },
            new Coord() { X = 1, Y = 2 },
            new Coord() { X = 2, Y = 2 },
        };
        private readonly Coord[] L = {
            new Coord() { X = 1, Y = 0 },
            new Coord() { X = 1, Y = 1 },
            new Coord() { X = 1, Y = 2 },
            new Coord() { X = 2, Y = 2 },
        };
        private readonly Coord[] J = {
            new Coord() { X = 2, Y = 0 },
            new Coord() { X = 2, Y = 1 },
            new Coord() { X = 2, Y = 2 },
            new Coord() { X = 1, Y = 2 },
        };
        private readonly Coord[] S = {
            new Coord() { X = 0, Y = 2 },
            new Coord() { X = 1, Y = 2 },
            new Coord() { X = 1, Y = 1 },
            new Coord() { X = 2, Y = 1 },
        };
        private readonly Coord[] Z = {
            new Coord() { X = 0, Y = 1 },
            new Coord() { X = 1, Y = 1 },
            new Coord() { X = 1, Y = 2 },
            new Coord() { X = 2, Y = 2 },
        };
    }
}
