using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TetrisModel
{
    public partial class TetrisField
    {
        public int FieldSizeX { get; private set; }
        public int FieldSizeY { get; private set; }

        public MatrixTetri CurrentTetri { get; private set; }
        public MatrixTetri NextTetri { get; private set; }

        public event EventHandler TetriMoved;
        public event EventHandler TetriLanded;
        public event EventHandler TetriGameOver;

        private bool _gameRunning;

        readonly private System.Timers.Timer tick = new System.Timers.Timer(750);

        public List<CoordListingTetri> LandedTetri { get; private set; } = new List<CoordListingTetri>();

        public TetrisField(int fieldSizeX, int fieldSizeY)
        {
            FieldSizeX = fieldSizeX;
            FieldSizeY = fieldSizeY;

            tick.Elapsed += Tick_Elapsed;
        }

        private void Tick_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            MoveDown();
        }

        public void Init()
        {
            CurrentTetri = new MatrixTetri();
            CurrentTetri.BeRandomStandardTetri();
            SetStartPosition(CurrentTetri);

            NextTetri = new MatrixTetri(0, 0);
            NextTetri.BeRandomStandardTetri();
        }

        public void Start()
        {
            tick.Enabled = true;
            _gameRunning = true;
        }

        public void Stop()
        {
            _gameRunning = false;
            tick.Stop();
        }

        public int GetLandedSquareCount()
        {
            int count = 0;
            foreach(var entry in LandedTetri)
            {
                count += entry.Listing.Length;
            }
            return count;
        }

        private void PrepareForNextTetri()
        {
            CurrentTetri = NextTetri;
            SetStartPosition(CurrentTetri);

            NextTetri = new MatrixTetri(0, 0);
            NextTetri.BeRandomStandardTetri();
        }

        private void SetStartPosition(MatrixTetri matrixTetri)
        {
            var (_, _, _, maxY) = matrixTetri.GetRange();
            matrixTetri.PositionX = FieldSizeX / 2 - 2;
            matrixTetri.PositionY = -1 - maxY;
        }

        /// <summary>
        /// Fills a 2D-Array analogous to the field with the tetris. True = there is one.
        /// </summary>
        /// <returns></returns>
        private bool[,] ReturnFilledField()
        {
            bool[,] filledField = new bool[FieldSizeX, FieldSizeY];
            foreach(var entry in LandedTetri)
            {
                for (int i = 0; i < entry.Listing.Length; i++)
                {
                    filledField[entry.Listing[i].X, entry.Listing[i].Y] = true;
                }
            }
            return filledField;
        }

        private int SeekAndDestroyFinishedLines()
        {
            int finishedLineCount = 0;
            bool[,] filledField = ReturnFilledField();

            for (int y = 0; y < FieldSizeY; y++)
            {
                for (int x = 0; x < FieldSizeX; x++)
                {
                    if (filledField[x, y] == false)
                    {
                        break;
                    }
                    if (x == FieldSizeX - 1)
                    {
                        RemovedFinishedLine(y);
                        finishedLineCount++;
                    }
                }
            }
            if (finishedLineCount > 0)
            {
                TidyUpLandedList();
                SeekAndDestroyFinishedLines();
            }

            return finishedLineCount;
        }

        private void TidyUpLandedList()
        {
            List<CoordListingTetri> emptyEntry = new List<CoordListingTetri>();
            foreach(var entry in LandedTetri)
            {
                if (entry.Listing.Length == 0)
                    emptyEntry.Add(entry);
            }
            foreach(var entry in emptyEntry)
            {
                LandedTetri.Remove(entry);
            }

            // Moving down squares hanging in the air.
            foreach(var entry in LandedTetri)
            {
                entry.CompressUpDown();
            }
        }

        private void RemovedFinishedLine(int lineNumber)
        {
            foreach(var entry in LandedTetri)
            {
                int i = 0;
                while (i < entry.Listing.Length)
                {
                    if (entry.Listing[i].Y == lineNumber)
                    {
                        entry.RemoveAt(i);
                        i--;
                    }
                    i++;
                };
            }
        }

        private void LetThemFall()
        {
            bool couldFall;
            do
            {
                couldFall = false;
                foreach (var entry in LandedTetri)
                {
                    if (FallingDown(entry) == true)
                        couldFall = true;
                }
            } while (couldFall);
        }

        /// <summary>
        /// Checks if falling is possible and lets the Tetris fall.
        /// </summary>
        /// <param name="landedTetri"></param>
        /// <returns>true if could fall, false if not</returns>
        private bool FallingDown(CoordListingTetri landedTetri)
        {
            CoordListingTetri copiedTetri = landedTetri.GetCopy();
            copiedTetri.FallOne();

            bool borderCollision = CollisionWithBorder(copiedTetri);
            bool squareCollision = CollisionWithSquare(copiedTetri, landedTetri);

            if (!borderCollision)
                if (!squareCollision)
                {
                    landedTetri.FallOne();
                    return true;
                }
            return false;
        }
    }
}
