
using UnityEngine;

public class ScoreController
{
    private ScoreModel model;
    private ScoreView view;

    public ScoreController(ScoreModel model, ScoreView view)
    {
        this.model = model;
        this.view = view;

        view.Controller = this;
    }

    public int GetCurrentScore()
    {
        return model.CurrentScore;
    }

    public void UpdateCurrentScore(int pointsGain)
    {
        model.CurrentScore += pointsGain;
        view.UpdateScoreUI(model.CurrentScore);

        if(model.CurrentScore > model.HighScore)
        {
            // Invoke event to notify achivement system
            PlayerPrefs.SetInt("HighScore", model.CurrentScore);
            PlayerPrefs.Save(); 
        }
    }
}
