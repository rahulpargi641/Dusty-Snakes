
public class LevelController
{
    private LevelModel model;
    private LevelView view;

    public LevelController(LevelModel model, LevelView view)
    {
        this.model = model;
        this.view = view;

        view.Controller = this;
        model.Controller = this;
    }

    public int GetLevelWidth()
    {
        return model.Width;
    }

    public int GetLevelHeight()
    {
        return model.Height;
    }

    public GameState GetGameState()
    {
        return model.GameState;
    }

    public void SetGameState(GameState gameState)
    {
        model.GameState = gameState;
    }

    public void ProcessGameOver() // if snake dies
    {
        view.EnableGameOverGO();
    }

    public void ProcessIfLevelWin() // Achievement System, get the score from score service
    {
        if (model.CurrentScore >= model.ScoreToWin)
        {
            view.EnableLevelWinGO();
        }
    }
}
