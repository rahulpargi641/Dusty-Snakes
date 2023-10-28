using System.Collections.Generic;
using UnityEngine;

public class FoodService : MonoSingletonGeneric<FoodService>
{
    [SerializeField] FoodSO[] foodSOs;

    private List<FoodController> foodControllers = new List<FoodController>();
    private Dictionary<FoodSO, FoodPool> foodPools = new Dictionary<FoodSO, FoodPool>();

    public FoodController SpawnRandomFood(Vector2 spawnPoint)
    {
        FoodPool selectedPool = SelectFoodPool();

        FoodController foodController = selectedPool.GetFood();
        foodController.SetTransform(spawnPoint);
        foodController.EnableFood();

        foodControllers.Add(foodController);

        return foodController;
    }

    private FoodPool SelectFoodPool()
    {
        int randIdx = Random.Range(0, foodSOs.Length);
        FoodSO selectedFoodSO = foodSOs[randIdx];

        if (!foodPools.ContainsKey(selectedFoodSO))
            CreateFoodPool(selectedFoodSO);

        FoodPool selectedPool = foodPools[selectedFoodSO];
        return selectedPool;
    }

    private void CreateFoodPool(FoodSO selectedFoodSO)
    {
        FoodModel foodModel = new FoodModel(selectedFoodSO);
        FoodView foodView = selectedFoodSO.foodView;

        FoodPool foodPool = new FoodPool();
        foodPool.Initialize(foodModel, foodView);

        foodPools[selectedFoodSO] = foodPool;
    }

    public void ReturnFoodToPool(FoodController foodController)
    {
        foodController.DisableFood();

        foreach (var pool in foodPools.Values)
            pool.ReturnItem(foodController);
    }
}
