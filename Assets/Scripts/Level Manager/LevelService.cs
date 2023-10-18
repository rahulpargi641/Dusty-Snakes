using UnityEngine;

public class LevelService : MonoSingletonGeneric<LevelService>
{
    [SerializeField] LevelView levelView;
    private LevelController levelController;

    // Start is called before the first frame update
    void Start()
    {
        LevelModel levelModel = new LevelModel();
        levelController = new LevelController(levelModel, levelView);

        SnakeController.onSnakeDeath += ProcessGameOver;
    }

    private void OnDestroy()
    {
        SnakeController.onSnakeDeath -= ProcessGameOver;
    }

    private void ProcessGameOver()
    {
        levelController.ProcessGameOver();
    }

    public int GetLevelWidth()
    {
        return levelController.GetLevelWidth();
    }

    public int GetLevelHeight()
    {
        return levelController.GetLevelHeight();
    }
}
