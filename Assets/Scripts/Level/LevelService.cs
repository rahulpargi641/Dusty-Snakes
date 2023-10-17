using System.Collections;
using System.Collections.Generic;
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
    }

    public int GetLevelWidth()
    {
        return levelController.GetLevelWidth();
    }

    public int GetLevelHeight()
    {
        return levelController.GetLevelHeight();
    }

    public void TimerExpired()
    {
        levelController.TimerExpired();
    }

    public void SnakeCollided()
    {
        levelController.SnakeCollided();
    }
}
