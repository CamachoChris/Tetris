﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TetrisModel;

namespace Tetris
{

    class TetrisFieldMV
    {
        private readonly int SquareSize;

        private readonly Canvas TetrisCanvas;
        private TetrisField tetrisField;
        
        private TetriMV currentTetri;

        private List<TetriMV> SquaresInField = new List<TetriMV>();

        public TetrisFieldMV(Canvas canvas, TetrisField playingField, int squaresize)
        {
            TetrisCanvas = canvas;
            tetrisField = playingField;
            playingField.FieldChanged += PlayingField_FieldChanged;
            SquareSize = squaresize;

            currentTetri = new TetriMV(TetrisCanvas, SquareSize);
            currentTetri.CoordTetri = new CoordTetromino(tetrisField.CurrentTetri);
        }

        private void PlayingField_FieldChanged(object sender, EventArgs e)
        {
            currentTetri.CoordTetri = new CoordTetromino(tetrisField.CurrentTetri);
            currentTetri.Paint();
        }

        public void Start()
        {
        }

        public void MoveLeft()
        {
            tetrisField.MoveLeft();
        }

        public void MoveRight()
        {
            tetrisField.MoveRight();
        }

        public void MoveDown()
        {
            tetrisField.MoveDown();
        }

        public void RotateLeft()
        {
            tetrisField.RotateLeft();
        }

        public void RotateRight()
        {
            tetrisField.RotateRight();
        }

        private void PaintSquaresInField()
        {
        }

        private int CountLandedSquaresModel(List<CoordTetromino> landedTetrominos)
        {
            int count = 0;
            foreach (var entry in landedTetrominos)
            {
                count += entry.Tetri.Length;
            }
            return count;
        }
    }
}
