using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodService : MonoSingletonGeneric<FoodService>
{
    [SerializeField] FoodView[] foods;
    private List<FoodController> foodControllers = new List<FoodController>();

    // Start is called before the first frame update
    void Start()
    {
        // subscribe to the events
    }

    public FoodController SpawnRandomFood(Vector2 spawnPoint)
    {
        FoodModel foodModel = new FoodModel();

        int randIdx = Random.Range(0, foods.Length);
        FoodView foodView = Instantiate(foods[randIdx], spawnPoint, Quaternion.identity);

        FoodController foodController = new FoodController(foodModel, foodView);

        foodControllers.Add(foodController);

        return foodController;
    }

}
