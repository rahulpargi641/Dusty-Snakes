using UnityEngine;

public class ScoreService : MonoSingletonGeneric<ScoreService>
{
    [SerializeField] ScoreView scoreView;
    private ScoreController scoreController;

    // Start is called before the first frame update
    void Start()
    {
        ScoreModel scoreModel = new ScoreModel();
        scoreController = new ScoreController(scoreModel, scoreView);

        SnakeController.onFoodEaten += UpdateScore;
    }

    private void OnDestroy()
    {
        SnakeController.onFoodEaten -= UpdateScore;
    }

    private void UpdateScore(int pointsGain)
    {
        scoreController.UpdateCurrentScore(pointsGain);
    }
}
