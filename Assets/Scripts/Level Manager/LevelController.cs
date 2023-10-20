using System.Threading.Tasks;

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
        return model.LevelWidth;
    }

    public int GetLevelHeight()
    {
        return model.LevelHeight;
    }

    public EGameState GetGameState()
    {
        return model.GameState;
    }

    public void SetGameState(EGameState gameState)
    {
        model.GameState = gameState;
    }

    public async void ProcessGameOver() // if snake dies
    {
        await Task.Delay(2 * 1000);

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
