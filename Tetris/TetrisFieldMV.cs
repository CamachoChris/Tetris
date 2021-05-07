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

    class TetrisFieldMV
    {
        private readonly int SquareSize;

        private readonly Canvas TetrisCanvas;
        private readonly Canvas TeaserCanvas;
        private readonly TetrisField tetrisField;
        
        private TetriMV currentTetri;
        private TetriMV nextTetri;

        private readonly List<TetriMV> FieldTetri = new List<TetriMV>();

        public TetrisFieldMV(Canvas canvas, Canvas teasercanvas, TetrisField field, int squaresize)
        {
            TetrisCanvas = canvas;
            TeaserCanvas = teasercanvas;
            tetrisField = field;
            SquareSize = squaresize;

            field.FieldChanged += TetrisEvent_FieldChanged;
            field.TetriLanded += TetrisEvent_TetriLanded;
            field.ShowNextTetri += Field_ShowNextTetri;
        }

        private void Field_ShowNextTetri(object sender, EventArgs e)
        {
            MakeNewNext();
        }

        private void MakeNewCurrent()
        {
            currentTetri = new TetriMV(TetrisCanvas, SquareSize)
            {
                CoordTetri = new CoordListingTetri(tetrisField.CurrentTetri)
            };
        }

        private void MakeNewNext()
        {
            TeaserCanvas.Children.Clear();

            nextTetri = new TetriMV(TeaserCanvas, SquareSize)
            {
                CoordTetri = new CoordListingTetri(tetrisField.NextTetri)
            };

            nextTetri.Paint();
        }

        private void TetrisEvent_TetriLanded(object sender, EventArgs e)
        {
            currentTetri.CoordTetri = tetrisField.LandedTetri[tetrisField.LandedTetri.Count - 1];
            FieldTetri.Add(currentTetri);
            MakeNewCurrent();
        }

        private void TetrisEvent_FieldChanged(object sender, EventArgs e)
        {
            currentTetri.CoordTetri = new CoordListingTetri(tetrisField.CurrentTetri);
            currentTetri.Paint();
        }

        public void Start()
        {
            MakeNewCurrent();
            MakeNewNext();
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
    }
}
