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
using System.IO;

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
            Troll_SoundHandler(2);
        }

        private void TetrisEvent_TetriLanded(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                UpdateTetri(currentTetri, tetrisField.CurrentTetri);
                UpdateTetri(nextTetri, tetrisField.NextTetri);
                UpdateField();
                Troll_SoundHandler(3);
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

                Troll_SoundHandler(5);
                PauseText.Visibility = Visibility.Visible;

                HighscoreWindow highscoreWindow = new HighscoreWindow(999999, 0, "Tetris Highscore", HighscoreFilename);
                highscoreWindow.Owner = Application.Current.MainWindow;
                highscoreWindow.Show();
                Troll_SoundHandler(6);
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
            Troll_SoundHandler(4);
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

            Troll_SoundHandler(1);

        }

        private void Field_TetriGameScoreChange(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                ScoreText.Text = string.Format($"{(int)sender}");
            }));
        }

        public void Troll_SoundHandler(int whichSound) // Public weil andere Klasse drauf zugreift. Ruhe.
        {
            string soundFile;
            string basePath = Environment.CurrentDirectory + @"\Sound";
            string soundPath;

            SoundPlayer Snd;

            switch(whichSound)
                {
                case 1:
                    {
                        soundFile = @"\soundSpeedIncreased.wav";
                        soundPath = basePath + soundFile;
                        Snd = new SoundPlayer(soundPath);
                        Snd.PlaySync();// Play mit Threadfreeze
                        break;
                    }
                case 2:
                    {
                        soundFile = @"\soundChangeRotation.wav";
                        soundPath = basePath + soundFile;
                        Snd = new SoundPlayer(soundPath);
                        Snd.Play(); // Play ohne Threadfreeze
                        break;
                    }
                case 3:
                    {
                        soundFile = @"\soundGroundConnect.wav";
                        soundPath = basePath + soundFile;
                        Snd = new SoundPlayer(soundPath);
                        Snd.PlaySync();
                        break;
                    }
                case 4:
                    {
                        soundFile = @"\soundPause.wav";
                        soundPath = basePath + soundFile;
                        Snd = new SoundPlayer(soundPath);
                        Snd.Play();
                        break;
                    }
                case 5:
                    {
                        soundFile = @"\soundGameover.wav";
                        soundPath = basePath + soundFile;
                        Snd = new SoundPlayer(soundPath);
                        Snd.PlaySync();
                        break;
                    }
                case 6:
                    {
                        soundFile = @"\soundHighscore.wav";
                        soundPath = basePath + soundFile;
                        Snd = new SoundPlayer(soundPath);
                        Snd.PlaySync();
                        break;
                    }
                case 7:
                    {
                        soundFile = @"\soundLineCleared.wav";
                        soundPath = basePath + soundFile;
                        Snd = new SoundPlayer(soundPath);
                        Snd.PlaySync();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

    }
}
