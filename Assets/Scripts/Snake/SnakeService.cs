using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeService : MonoSingletonGeneric<SnakeService>
{
    [SerializeField] SnakeView snakeView;
    [SerializeField] Transform spawnPoint;

    private SnakeController snakeController;

    // Start is called before the first frame update
    void Start()
    {
        SpawnSnake(spawnPoint);
    }

    public void SpawnSnake(Transform spawnPoint)
    {
        SnakeModel snakeModel = new SnakeModel();
        SnakeView snakeView = Instantiate(this.snakeView, spawnPoint.position, this.snakeView.transform.rotation);
        snakeController = new SnakeController(snakeModel, snakeView);
    }

    public List<Vector2Int> GetWholeSnakeBodyPositions()
    {
        return snakeController.GetWholeSnakeBodyPositions();
    }
}
