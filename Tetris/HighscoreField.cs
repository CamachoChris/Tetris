namespace Highscores
{
    public class HighscoreField
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public HighscoreField(){}
        public HighscoreField (string name, int score)
        {
            Name = name;
            Score = score;
        }
    }
}
