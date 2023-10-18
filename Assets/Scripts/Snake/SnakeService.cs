using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeService : MonoBehaviour
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
        snakeController = new SnakeController(snakeModel, snakeView);
    }
}
