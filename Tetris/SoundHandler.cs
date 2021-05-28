using System;
using System.Collections.Generic;
using System.Text;
using System.Media;
using System.IO;


namespace Tetris
{
    class SoundHandler
    {
        //checked
        private static string pathToSounds;
        private static List<SoundPlayer> soundList;
        private static List<String> nameList;
        private static int breakLoop;


        //unchecked
        string debugSound = Environment.CurrentDirectory + @"\sound\soundSpeedIncreased.wav";
        public static string debugLabel;
        private static int indexDebug;



        public static void initSoundHandler() // Loader
        {
            // Setter
            pathToSounds = Environment.CurrentDirectory + @"\Sound\"; // SoundDirectory
            soundList = new List<SoundPlayer>();
            nameList = new List<string>();
                         
            // Fill List with Filenames & Load to Ram
            foreach (string f in Directory.GetFiles(pathToSounds))
            {
                nameList.Add(f);
                soundList.Add(new SoundPlayer(f));
            }
            breakLoop = nameList.Count - 1;
        
        }

        static void callSound(int soundIndex, bool priority)
        {
            if (priority)
            {
                soundList[soundIndex].PlaySync();
            }
            else
            {
                soundList[soundIndex].Play();
            }
        }

        public static void playSound(string dataName)
        {
            int soundIndex = -1;
            bool fileNotFound = false;

            // Find Sound
            do
            {
                if (soundIndex == breakLoop) // Pseudo Escape Throw
                {
                    fileNotFound = true;
                    break;
                }
                soundIndex++;
            } while (nameList[soundIndex].Contains(dataName) == false);

            // Play Sound if found
            if ((fileNotFound == false))
            {
                callSound(soundIndex, false);
            }

        }

        public static void playSound(string dataName, bool priority)
        {
            int soundIndex = -1;
            bool fileNotFound = false;

            // Find Sound
            do
            {
                if (soundIndex == breakLoop) // Pseudo Escape Throw
                {
                    fileNotFound = true;
                    break;
                }
                soundIndex++;
            } while (nameList[soundIndex].Contains(dataName) == false);

            // Play SYNC Sound if found
            if ((fileNotFound == false))
            {
                callSound(soundIndex, priority);
            }

        }

    }




}


//  SoundHandler.initSoundHandler(); // Old but Gold


/*
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

*/
