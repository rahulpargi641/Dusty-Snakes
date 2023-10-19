
public class LevelModel
{
    public int LevelWidth { get; private set; }
    public int LevelHeight { get; private set; }
    public int ScoreToWin { get; private set; }
    public int CurrentScore { get; set; } = 0;
    public EGameState GameState { get; set; } = EGameState.Running;

    public LevelController Controller { private get; set; }

    private LevelSO levelSO;
    public LevelModel(LevelSO levelSO)
    {
        this.levelSO = levelSO;

        LevelWidth = levelSO.levelWidth;
        LevelHeight = levelSO.levelHeight;
        ScoreToWin = levelSO.scoreToWin; // Use PlayerPrefs
    }
}
