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
        public event EventHandler TetriFieldChanged;
        public event EventHandler TetriGameOver;
        public event EventHandler TetriGamePaused;
        public event EventHandler TetriGameUnpaused;
        public event EventHandler TetriGameReset;
        public event EventHandler TetriGameLevelUp;

        private bool _gameRunning;
        private bool _gameOver;

        private int _gameSpeed;
        private int _finishedLines;
        private int _level;

        readonly private System.Timers.Timer tick;

        public List<CoordListingTetri> LandedTetri { get; private set; } = new List<CoordListingTetri>();

        public TetrisField(int fieldSizeX, int fieldSizeY)
        {
            FieldSizeX = fieldSizeX;
            FieldSizeY = fieldSizeY;

            tick = new System.Timers.Timer();
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

            LandedTetri.Clear();

            _gameRunning = false;
            _gameOver = false;

            _gameSpeed = 600; // 70 schnellstes Ende. 600 langsamster Anfang.
            _finishedLines = 0;
            _level = 1;

            tick.Interval = _gameSpeed;
            tick.Enabled = false;
        }

        public void Reset()
        {
            Init();

            if (TetriGameReset != null)
                TetriGameReset(null, EventArgs.Empty);
        }

        public void Start()
        {
            if (_gameOver)
                return;

            if (!_gameRunning)
            {
                tick.Enabled = true;
                _gameRunning = true;
            }
            else
            {
                if (tick.Enabled == true)
                {
                    PauseGame();
                }
                else if (tick.Enabled == false)
                {
                    UnpauseGame();
                }
            }
        }

        public bool IsGameRunning()
        {
            return _gameRunning;
        }

        public void PauseGame()
        {
            tick.Enabled = false;

            if (TetriGamePaused != null)
                TetriGamePaused(null, EventArgs.Empty);
        }

        public void UnpauseGame()
        {
            tick.Enabled = true;

            if (TetriGameUnpaused != null)
                TetriGameUnpaused(null, EventArgs.Empty);
        }

        public void GameOver()
        {
            _gameRunning = false;
            tick.Stop();
        }

        public void SpeedUp()
        {
            if (_gameSpeed > 300)
                _gameSpeed -= 20;
            else if (_gameSpeed > 200)
                _gameSpeed -= 10;
            else if (_gameSpeed > 70)
                _gameSpeed -= 5;
            Debug.WriteLine(_gameSpeed);
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
                //SeekAndDestroyFinishedLines();
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

        private bool LetThemFall()
        {
            bool couldFall;
            //do
            //{
                couldFall = false;
                foreach (var entry in LandedTetri)
                {
                    if (FallingDown(entry) == true)
                        couldFall = true;
                }
            //} while (couldFall);
            return couldFall;
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
