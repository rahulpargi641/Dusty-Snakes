using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodService : MonoSingletonGeneric<FoodService>
{
    [SerializeField] FoodView[] foods;
    private List<FoodController> foodControllers;

    // Start is called before the first frame update
    void Start()
    {
        // subscribe to the events
        SpawnRandomFood(new Vector2(10, 5));
    }

    public void SpawnRandomFood(Vector2 spawnPoint)
    {
        FoodModel foodModel = new FoodModel();

        int randIdx = UnityEngine.Random.Range(0, foods.Length);
        FoodView foodView = Instantiate(foods[randIdx], spawnPoint, Quaternion.identity);

        FoodController foodController = new FoodController(foodModel, foodView);

        foodControllers.Add(foodController);
    }

}
