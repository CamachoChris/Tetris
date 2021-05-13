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
        public event EventHandler TetriGameScoreChange;

        private bool _gameRunning;
        private bool _gameOver;

        private int _gameSpeed;
        private int _finishedLinesCount;
        private int _level;
        private int _score;

        readonly private System.Timers.Timer tick = new System.Timers.Timer();

        public List<CoordListingTetri> LandedTetri { get; private set; } = new List<CoordListingTetri>();
        private List<int> _finishedLinesList = new List<int>();

        public TetrisField(int fieldSizeX, int fieldSizeY)
        {
            FieldSizeX = fieldSizeX;
            FieldSizeY = fieldSizeY;

            tick.Elapsed += Tick_Elapsed;
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

            _gameSpeed = 600;
            _finishedLinesCount = 0;
            _level = 1;
            _score = 0;

            if (TetriGameLevelUp != null)
                TetriGameLevelUp(_level, EventArgs.Empty);
            if (TetriGameScoreChange != null)
                TetriGameScoreChange(_score, EventArgs.Empty);

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

        public int GetLandedSquareCount()
        {
            int count = 0;
            foreach (var entry in LandedTetri)
            {
                count += entry.Listing.Count;
            }
            return count;
        }

        private void Tick_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            MovingField();
            MoveDown();
        }

        private void GameOver()
        {
            _gameRunning = false;
            tick.Stop();
        }

        private void SpeedUp()
        {
            if (_gameSpeed > 300)
                _gameSpeed -= 40;
            else if (_gameSpeed > 200)
                _gameSpeed -= 20;
            else if (_gameSpeed > 100)
                _gameSpeed -= 10;
            else if (_gameSpeed > 70)
                _gameSpeed -= 5;

            tick.Interval = _gameSpeed;
            Debug.WriteLine(tick.Interval);
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
    }
}
