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
using Highscores;
using System.Diagnostics;
using System.Media;

namespace Tetris
{
    partial class TetrisFieldMV
    {
        private void Field_TetriMoved(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                UpdateTetri(currentTetri, tetrisField.CurrentTetri);
            }));
               SoundPlayer Snd = new SoundPlayer(@"G:\0 Proggen 2021\Tetris2\Tetris\soundChangeRotation.wav");
               Snd.Play();
        }

        private void TetrisEvent_TetriLanded(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                UpdateTetri(currentTetri, tetrisField.CurrentTetri);
                UpdateTetri(nextTetri, tetrisField.NextTetri);
                UpdateField();
                   SoundPlayer Snd = new SoundPlayer(@"G:\0 Proggen 2021\Tetris2\Tetris\soundGroundConnect.wav");
                   Snd.PlaySync();
            }));
        }

        private void Field_TetriFieldChanged(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                UpdateField();
            }));
        }

        private void Field_TetriGameOver(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                PauseText.Text = "Game Over";
                   SoundPlayer Snd = new SoundPlayer(@"G:\0 Proggen 2021\Tetris2\Tetris\soundGameover.wav");
                   Snd.PlaySync();
                PauseText.Visibility = Visibility.Visible;

                HighscoreWindow highscoreWindow = new HighscoreWindow(999999, 0, "Tetris Highscore", HighscoreFilename);
                highscoreWindow.Owner = Application.Current.MainWindow;
                highscoreWindow.Show();
                SoundPlayer Snd2 = new SoundPlayer(@"G:\0 Proggen 2021\Tetris2\Tetris\soundHighscore.wav");
                Snd2.PlaySync();
                highscoreWindow.TryAdd(int.Parse(ScoreText.Text));
            }));
        }

        private void Field_TetriGameUnpaused(object sender, EventArgs e)
        {
            AllElementsToNormalMode();
        }


        private void Field_TetriGamePaused(object sender, EventArgs e)
        {
            AllElementsInPauseMode();
            // TEST
         //   SoundPlayer Snd = new SoundPlayer(@"G:\0 Proggen 2021\Tetris2\Tetris\soundChangeRotation.wav");
         //   Snd.Play();
          
                 }

        private void Field_TetriGameReset(object sender, EventArgs e)
        {
            Init();
            SoundPlayer Snd = new SoundPlayer(@"G:\0 Proggen 2021\Tetris2\Tetris\soundStartNewgame.wav");
            Snd.Play();
        }

        private void Field_TetriGameLevelUp(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                LevelText.Text = string.Format($"{(int)sender}");
            }));
               SoundPlayer Snd = new SoundPlayer(@"G:\0 Proggen 2021\Tetris2\Tetris\soundSpeedIncreased.wav");
               Snd.PlaySync();
        }

        private void Field_TetriGameScoreChange(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                ScoreText.Text = string.Format($"{(int)sender}");
            }));
        }
    }
}
