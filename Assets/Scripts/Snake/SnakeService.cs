using System.Collections.Generic;
using UnityEngine;

public class SnakeService : MonoSingletonGeneric<SnakeService>
{
    [SerializeField] SnakeSO snakeSO;
    [SerializeField] Transform[] spawnPoints;

    private SnakeController snakeController;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform spawnPoint in spawnPoints) // spawns two snakes
            SpawnSnake(spawnPoint);
    }

    public void SpawnSnake(Transform spawnPoint)
    {
        SnakeModel snakeModel = new SnakeModel(snakeSO);
        SnakeView snakeView = Instantiate(snakeSO.snakeView, spawnPoint.position, snakeSO.snakeView.transform.rotation);
        snakeController = new SnakeController(snakeModel, snakeView);
    }

    public List<Vector2Int> GetWholeSnakeBodyPositions()
    {
        return snakeController.GetWholeSnakeBodyPositions();
    }
}
