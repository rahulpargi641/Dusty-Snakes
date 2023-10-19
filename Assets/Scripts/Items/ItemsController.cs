using System;
using System.Collections;
using UnityEngine;

public class ItemsController
{
    private ItemsModel model;
    private ItemsView view;

    public ItemsController(ItemsModel model, ItemsView view)
    {
        this.model = model;
        this.view = view;

        view.Controller = this;
    }

    public IEnumerator SpawnFoodItems()
    {
        while (model.GameRunning) 
        {
            Vector2Int randomFoodPos;
            FoodController spawnedFood;
            SpawnFoodAtRandomPosition(out randomFoodPos, out spawnedFood);

            yield return new WaitForSeconds(model.FoodSpawnIntervalDelay);

            if(!model.FoodEaten)
                UnspawnTheFood();
        }
    }

    private void SpawnFoodAtRandomPosition(out Vector2Int randomFoodPos, out FoodController spawnedFood)
    {
        randomFoodPos = GenerateRandomPosNotAtSnakeBody();
        spawnedFood = FoodService.Instance.SpawnRandomFood(randomFoodPos);

        model.FoodEaten = false;
    }

    private void UnspawnTheFood() // remove spawned from via events
    {
        // Send back to Food Pool and deactivate
        Debug.Log("Food got unspawned successfully");
    }

    public IEnumerator SpawnPowerUpItems()
    {
        yield return new WaitForSeconds(model.PowerUpSpawnIntervalDelay);

        while (model.GameRunning)
        {
            Vector2Int randomPowerUpPos;
            PowerUpController spawnedPowerUp;
            SpawnPowerUpAtRandomPosition(out randomPowerUpPos, out spawnedPowerUp);

            yield return new WaitForSeconds(model.PowerUpSpawnIntervalDelay);
            if(!model.PowerUpEaten)
                UnspawnedThePowerUp();
        }
    }

    private void SpawnPowerUpAtRandomPosition(out Vector2Int randomPowerUpPos, out PowerUpController spawnedPowerUp)
    {
        randomPowerUpPos = GenerateRandomPosNotAtSnakeBody();

        spawnedPowerUp = PowerUpService.Instance.SpawnRandomPowerUp(randomPowerUpPos);
    }

    private void UnspawnedThePowerUp()
    {
        // send back to pool
        Debug.Log("PowerUp Unspawned successfully");
    }

    private Vector2Int GenerateRandomPosNotAtSnakeBody()
    {
        Vector2Int RandomPowerUpPos;
        do
        {
            RandomPowerUpPos = new Vector2Int(UnityEngine.Random.Range(1, model.LevelWidth - 1), UnityEngine.Random.Range(1, model.LevelHeight - 1));
        } while (SnakeService.Instance.GetWholeSnakeBodyPositions().IndexOf(RandomPowerUpPos) != -1);

        return RandomPowerUpPos;
    }

    public void FoodEaten()
    {
        model.FoodEaten = true;
        Debug.Log("Food Up Eaten Successfully");
    }

    public void PowerUpEaten()
    {
        model.PowerUpEaten = true;
        Debug.Log("Power Up Eaten Successfully");
    }
}
