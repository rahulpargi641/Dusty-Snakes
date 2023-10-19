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
        yield return new WaitForSeconds(model.FoodSpawnIntervalDelay);

        while (true)
        {
            Vector2Int randomFoodPos;
            FoodView foodToSpawn, spawnedFood;

            SpawnRandomFood(out randomFoodPos, out foodToSpawn, out spawnedFood);

            yield return new WaitForSeconds(foodToSpawn.destroyAfterTime);

            ProcessIfSpawnedFoodNotEaten(randomFoodPos, spawnedFood);

            yield return new WaitForSeconds(model.FoodSpawnIntervalDelay);
        }
    }

    private void SpawnRandomFood(out Vector2Int randomFoodPos, out FoodView foodToSpawn, out FoodView spawnedFood)
    {
        randomFoodPos = GenerateRandomPosNotAtSnakeBody();

        foodToSpawn = RandomItemSelector(view.foods);
        spawnedFood = GameObject.Instantiate(foodToSpawn, new Vector3(randomFoodPos.x, randomFoodPos.y), Quaternion.identity);
        model.SpawnedFoods.Add(randomFoodPos, spawnedFood);
    }

    private void ProcessIfSpawnedFoodNotEaten(Vector2Int randomFoodPos, FoodView spawnedFood)
    {
        if (model.SpawnedFoods.ContainsValue(spawnedFood))
        {
            //Destroy(food.gameObject);
            spawnedFood.gameObject.SetActive(false);
            GetEatenFoodAndRemoveFromDictionary(randomFoodPos);
        }
    }

    private PowerUpView GetEatenPowerupAndRemoveFromDictionary(Vector2Int snakeCurrentPos)
    {
        if (model.SpawnedPowerUps.TryGetValue(snakeCurrentPos, out PowerUpView powerUpToDestroy))
        {
            model.SpawnedPowerUps.Remove(snakeCurrentPos);
        }
        return powerUpToDestroy;
    }

    public IEnumerator SpawnPowerUpItems()
    {
        while (true)
        {
            yield return new WaitForSeconds(model.PowerUpSpawnIntervalDelay);

            Vector2Int randomPowerUpPos;
            PowerUpView powerUpToSpawn, spawnedPowerUp;

            SpawnRandomPowerUp(out randomPowerUpPos, out powerUpToSpawn, out spawnedPowerUp);

            yield return new WaitForSeconds(powerUpToSpawn.destroyAfterTime);

            ProcessIfSpawnedPowerUpNotEaten(randomPowerUpPos, spawnedPowerUp);

            yield return new WaitForSeconds(model.PowerUpSpawnIntervalDelay);
        }
    }

    private void SpawnRandomPowerUp(out Vector2Int randomPowerUpPos, out PowerUpView powerUpToSpawn, out PowerUpView powerUp)
    {
        randomPowerUpPos = GenerateRandomPosNotAtSnakeBody();

        powerUpToSpawn = RandomItemSelector(view.powerUps);
        powerUp = GameObject.Instantiate(powerUpToSpawn, new Vector3(randomPowerUpPos.x, randomPowerUpPos.y), Quaternion.identity);
        model.SpawnedPowerUps.Add(randomPowerUpPos, powerUp);
    }

    private void ProcessIfSpawnedPowerUpNotEaten(Vector2Int randomPowerUpPos, PowerUpView powerUp)
    {
        if (model.SpawnedPowerUps.ContainsValue(powerUp))
        {
            //Destroy(powerUp.gameObject);
            powerUp.gameObject.SetActive(false);
            GetEatenPowerupAndRemoveFromDictionary(randomPowerUpPos);
        }
    }

    private FoodView GetEatenFoodAndRemoveFromDictionary(Vector2Int removeAtPosition)
    {
        if (model.SpawnedFoods.TryGetValue(removeAtPosition, out FoodView eatenFood))
        {
            model.SpawnedFoods.Remove(removeAtPosition);
        }
        return eatenFood;
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

    T RandomItemSelector<T>(T[] items)
    {
        int randomIndex = UnityEngine.Random.Range(0, items.Length);
        return items[randomIndex];
    }
}
