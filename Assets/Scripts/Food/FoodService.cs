using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodService : MonoSingletonGeneric<FoodService>
{
    [SerializeField] FoodSO[] foodSOs;
    private List<FoodController> foodControllers = new List<FoodController>();

    // Start is called before the first frame update
    void Start()
    {
        // subscribe to the events
    }

    public FoodController SpawnRandomFood(Vector2 spawnPoint)
    {
        int randIdx = Random.Range(0, foodSOs.Length);

        FoodModel foodModel = new FoodModel(foodSOs[randIdx]);
        FoodView foodView = Instantiate(foodSOs[randIdx].foodView, spawnPoint, Quaternion.identity);
        FoodController foodController = new FoodController(foodModel, foodView);

        foodControllers.Add(foodController);

        return foodController;
    }
}
