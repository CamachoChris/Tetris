using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;


namespace Highscores
{
    [Serializable]
    public class Highscore
    {
        public string HighscoreTitle = "";
        public List<HighscoreField> HighscoreList = new List<HighscoreField>();
        public int BestScore;
        public int WorstScore;
        private const int MAXENTRIES = 10;

        private Highscore() { }

        public Highscore(int bestScore, int worstScore, string title)
        {
            BestScore = bestScore;
            WorstScore = worstScore;
            HighscoreTitle = title;
        }
        
        /// <summary>
        /// Adds and sorts new potential player for highscore, if good enough.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="score"></param>
        public void Add(string name, int score)
        {
            int earnedPosition = GetHighscorePosition(score);

            if (earnedPosition < MAXENTRIES)
            {
                HighscoreField highscoreField = new HighscoreField(name, score);
                HighscoreList.Insert(earnedPosition, highscoreField);
            }
            if (HighscoreList.Count - 1 == MAXENTRIES)
            {
                HighscoreList.RemoveAt(MAXENTRIES);
            }        
        }

        /// <summary>
        /// Gets the highscore list. Fills empty slots with placeholder.
        /// </summary>
        /// <param name="placeholder">char for placeholder name</param>
        /// <param name="amount">amount of chars for placeholder name</param>
        /// <returns>Highscore list</returns>
        public List<HighscoreField> GetHighscoreList(char placeholder, int amount)
        {
            List<HighscoreField> newList = new List<HighscoreField>();

            foreach (var entry in HighscoreList)
            {
                newList.Add(entry);
            }

            for (int i = 0; i < MAXENTRIES - HighscoreList.Count; i++)
            {
                HighscoreField placeholderField = new HighscoreField();
                string placeholderString = "";

                for (int j = 0; j < amount; j++)
                {
                    placeholderString += placeholder;
                }

                placeholderField.Name = placeholderString;
                placeholderField.Score = WorstScore;

                newList.Add(placeholderField);
            }

            return newList;
        }

        /// <summary>
        /// Loads highscore from file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>true if loaded, false if file does not exist</returns>
        public bool LoadFromFile(string filename)
        {
            if (!File.Exists(filename))
                return false;

            XmlSerializer ser = new XmlSerializer(typeof(Highscore));
            using Stream s = File.OpenRead(filename);
            Highscore loadedHighscore = (Highscore)ser.Deserialize(s);            

            GetFromHighscore(loadedHighscore);

            return true;
        }

        /// <summary>
        /// Saves highscore to file.
        /// </summary>
        /// <param name="filename"></param>
        public void SaveToFile(string filename)
        {
            XmlSerializer ser = new XmlSerializer(this.GetType());
            using StreamWriter s = new StreamWriter(filename);
            ser.Serialize(s, this);
        }

        public bool IsNewGoodEnough(int newScore)
        {
            if (GetHighscorePosition(newScore) < MAXENTRIES)
                return true;
            else return false;
        }

        private void GetFromHighscore(Highscore highscore)
        {
            HighscoreTitle = highscore.HighscoreTitle;
            BestScore = highscore.BestScore;
            WorstScore = highscore.WorstScore;

            HighscoreList.Clear();
            foreach (var entry in highscore.HighscoreList)
            {
                HighscoreList.Add(entry);
            }
        }

        public int GetHighscorePosition(int score)
        {
            int earnedPosition = HighscoreList.Count;
            int index = 0;

            foreach (var entry in HighscoreList)
            {
                int difference = BestScore - WorstScore;
                if ((difference > 0 && score > entry.Score) || (difference < 0 && score < entry.Score))
                {
                    earnedPosition = index;
                    break;
                }
                index++;
            }

            return earnedPosition;
        }
    }
}
