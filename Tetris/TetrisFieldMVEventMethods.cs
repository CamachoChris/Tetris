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
        private void TetrisEvent_FieldChanged(object sender, EventArgs e)
        {
            TidyUpLandedList();

            currentTetri.CoordTetri = new CoordListingTetri(tetrisField.CurrentTetri);
            currentTetri.Paint();
            foreach (var entry in LandedTetriMV)
                entry.Paint();
        }

        private void TetrisEvent_TetriLanded(object sender, EventArgs e)
        {
            currentTetri.CoordTetri = tetrisField.LandedTetri[tetrisField.LandedTetri.Count - 1];
            LandedTetriMV.Add(currentTetri);
            MakeNewCurrent();
        }

        private void Field_ShowNextTetri(object sender, EventArgs e)
        {
            MakeNewNext();
        }
    }
}
