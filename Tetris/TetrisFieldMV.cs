using System;
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
using System.Diagnostics;

namespace Tetris
{
    partial class TetrisFieldMV
    {
        private readonly int SquareSize;

        private readonly Canvas TetrisCanvas;
        private readonly Canvas TeaserCanvas;
        private readonly TetrisField tetrisField;
        
        private TetriMV currentTetri;
        private TetriMV nextTetri;

        public TextBlock PauseText;
        public TextBlock LevelText;
        public TextBlock ScoreText;

        private readonly List<SquareMV> LandedSquaresMV = new List<SquareMV>();

        public TetrisFieldMV(Canvas canvas, Canvas teasercanvas, TetrisField field, int squaresize)
        {
            TetrisCanvas = canvas;
            TeaserCanvas = teasercanvas;
            tetrisField = field;
            SquareSize = squaresize;

            field.TetriMoved += Field_TetriMoved;
            field.TetriLanded += TetrisEvent_TetriLanded;
            field.TetriFieldChanged += Field_TetriFieldChanged;
            field.TetriGameOver += Field_TetriGameOver;
            field.TetriGameReset += Field_TetriGameReset;
            field.TetriGamePaused += Field_TetriGamePaused;
            field.TetriGameUnpaused += Field_TetriGameUnpaused;
            field.TetriGameLevelUp += Field_TetriGameLevelUp;
            field.TetriGameScoreChange += Field_TetriGameScoreChange;
        }

        private void MakeNewCurrent()
        {
            currentTetri = new TetriMV(TetrisCanvas, SquareSize)
            {
                CoordTetri = new CoordListingTetri(tetrisField.CurrentTetri)
            };

            currentTetri.UpdateTetri();
        }

        private void MakeNewNext()
        {
            TeaserCanvas.Children.Clear();

            nextTetri = new TetriMV(TeaserCanvas, SquareSize)
            {
                CoordTetri = new CoordListingTetri(tetrisField.NextTetri)
            };

            nextTetri.UpdateTetri();
        }

        private void UpdateCurrentTetri()
        {
            currentTetri.CoordTetri = new CoordListingTetri(tetrisField.CurrentTetri);
            currentTetri.UpdateTetri();
        }

        private void SyncLandedList()
        {
            int actualSquareCount = tetrisField.GetLandedSquareCount();
            int difference = actualSquareCount - LandedSquaresMV.Count;
            if (difference == 0)
                return;
            if (difference > 0)
            {
                for (int i = 0; i < difference; i++)
                {
                    SquareMV nextSquare = new SquareMV(TetrisCanvas, SquareSize);
                    LandedSquaresMV.Add(nextSquare);
                }
            }
            else if (difference < 0)
            {
                for (int i = 0; i < Math.Abs(difference); i++)
                {
                    LandedSquaresMV[^1].RemoveSquareFromCanvas();
                    LandedSquaresMV.RemoveAt(LandedSquaresMV.Count - 1);
                }
            }
        }

        private void UpdateField()
        {
            SyncLandedList();

            int i = 0;
            foreach (var entry in tetrisField.LandedTetri)
            {
                for (int j = 0; j < entry.Listing.Count; j++)
                {
                    LandedSquaresMV[i].PositionX = entry.Listing[j].X;
                    LandedSquaresMV[i].PositionY = entry.Listing[j].Y;
                    LandedSquaresMV[i].SetTetriColor(entry);
                    LandedSquaresMV[i].UpdateSquare();
                    i++;
                }
            }
        }

        public void Init()
        {
            MakeNewCurrent();
            MakeNewNext();
        }

        private void AllElementsToNormalMode()
        {
            PauseText.Visibility = Visibility.Hidden;

            foreach (var entry in LandedSquaresMV)
                entry.ChangeVisibility(Visibility.Visible);

            foreach (var entry in currentTetri.SquaresTetri)
                entry.ChangeVisibility(Visibility.Visible);

            foreach (var entry in nextTetri.SquaresTetri)
                entry.ChangeVisibility(Visibility.Visible);
        }

        private void AllElementsInPauseMode()
        {
            PauseText.Visibility = Visibility.Visible;

            foreach (var entry in LandedSquaresMV)
                entry.ChangeVisibility(Visibility.Hidden);

            foreach (var entry in currentTetri.SquaresTetri)
                entry.ChangeVisibility(Visibility.Hidden);

            foreach (var entry in nextTetri.SquaresTetri)
                entry.ChangeVisibility(Visibility.Hidden);
        }
    }
}
