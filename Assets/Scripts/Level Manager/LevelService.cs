using UnityEngine;

public class LevelService : MonoSingletonGeneric<LevelService>
{
    [SerializeField] LevelSO levelSO;
    [SerializeField] LevelView levelView;
    private LevelController levelController;

    // Start is called before the first frame update
    void Start()
    {
        LevelModel levelModel = new LevelModel(levelSO);
        //LevelView levelView = Instantiate(levelSO.levelView, transform.position, transform.rotation);
        levelController = new LevelController(levelModel, levelView);

        SnakeController.onSnakeDeath += ProcessGameOver;
        PowerUpController.onPowerUpEaten += DisplayPowerUpActivatedText;
    }

    private void OnDestroy()
    {
        SnakeController.onSnakeDeath -= ProcessGameOver;
        PowerUpController.onPowerUpEaten -= DisplayPowerUpActivatedText;
    }

    private void ProcessGameOver()
    {
        levelController.ProcessGameOver();
    }

    public void DisplayPowerUpActivatedText(PowerUpType pickedPowerUpType)
    {
        levelController.DisplayPowerUpActivatedText(pickedPowerUpType);
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
