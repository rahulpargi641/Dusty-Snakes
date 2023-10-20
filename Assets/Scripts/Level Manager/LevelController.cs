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
        AudioService.Instance.StopSound(SoundType.BgMusic);
        AudioService.Instance.PlaySound(SoundType.GameOver);
    }

    public void ProcessIfLevelWin() // Achievement System, get the score from score service
    {
        if (model.CurrentScore >= model.ScoreToWin)
        {
            view.EnableLevelWinGO();
        }
    }

    public void DisplayPowerUpActivatedText(PowerUpType pickedPowerUpType)
    {
        switch(pickedPowerUpType)
        {
            case PowerUpType.SpeedBoost:
                DisplaySpeedBoostTextAsync();
                break;
            case PowerUpType.ScoreBoost:
                DisplayScoreBoostTextAsync();
                break;
            case PowerUpType.Shield:
                DisplayShieldTextAsync();
                break;
        }
    }

    public async void DisplaySpeedBoostTextAsync()
    {
        view.EnableSpeedBoostText();

        await Task.Delay(model.TextDisplayDuration * 1000);

        view.DisableSpeedBoostText();
    }

    public async void DisplayScoreBoostTextAsync()
    {
        view.EnableScoreBoostText();

        await Task.Delay(model.TextDisplayDuration * 1000);

        view.DisableScoreBoostText();
    }

    public async void DisplayShieldTextAsync()
    {
        view.EnableShieldText();

        await Task.Delay(model.TextDisplayDuration * 1000);

        view.DisableShieldText();
    }
}
