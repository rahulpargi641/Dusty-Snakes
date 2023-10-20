using System.Collections.Generic;
using UnityEngine;

public class SnakeService : MonoSingletonGeneric<SnakeService>
{
    [SerializeField] SnakeSO[] snakeSOs;
    private SnakeController snakeController;

    // Start is called before the first frame update
    void Start()
    {
        foreach(SnakeSO snakeSO in snakeSOs) // spawns two snakes
            SpawnSnake(snakeSO);
    }

    public void SpawnSnake(SnakeSO snakeSO)
    {
        SnakeModel snakeModel = new SnakeModel(snakeSO);
        SnakeView snakeView = Instantiate(snakeSO.snakeView, (Vector2)snakeSO.spawnPos, snakeSO.snakeView.transform.rotation);
        snakeController = new SnakeController(snakeModel, snakeView);
    }

    public List<Vector2Int> GetWholeSnakeBodyPositions()
    {
        return snakeController.GetWholeSnakeBodyPositions();
    }
}
