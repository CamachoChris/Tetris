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
            SoundHandler.playSound("soundChangeRotation.wav");
        }

        private void TetrisEvent_TetriLanded(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                UpdateTetri(currentTetri, tetrisField.CurrentTetri);
                UpdateTetri(nextTetri, tetrisField.NextTetri);
                UpdateField();
                SoundHandler.playSound("Connect.wav");
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

                SoundHandler.playSound("Gameover.wav",true);
                PauseText.Visibility = Visibility.Visible;

                HighscoreWindow highscoreWindow = new HighscoreWindow(999999, 0, "Tetris Highscore", HighscoreFilename);
                highscoreWindow.Owner = Application.Current.MainWindow;
                highscoreWindow.Show();
                SoundHandler.playSound("Highscore.wav",true);
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
            SoundHandler.playSound("Pause.wav",true);
        }

        private void Field_TetriGameReset(object sender, EventArgs e)
        {
            Init();

        }

        private void Field_TetriGameLevelUp(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                LevelText.Text = string.Format($"{(int)sender}");
            }));

            SoundHandler.playSound("SpeedIncreased.wav");

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
