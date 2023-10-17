using UnityEngine;

public class ScoreModel
{
    public int CurrentScore { get; set; }
    public int HighScore { get; private set; } 

    public ScoreModel()
    {
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
    }
}
