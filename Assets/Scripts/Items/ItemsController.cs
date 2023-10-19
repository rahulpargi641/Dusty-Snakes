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

        while (true) // gameloop
        {
            SpawnFoodAtRandomPos();
            
            yield return new WaitForSeconds(model.FoodSpawnIntervalDelay);
        }
    }

    private void SpawnFoodAtRandomPos()
    {
        Vector2Int randomFoodPos;
        FoodController spawnedFood;

        randomFoodPos = GenerateRandomPosNotAtSnakeBody();
        spawnedFood = FoodService.Instance.SpawnRandomFood(randomFoodPos);

        model.SpawnedFoods.Add(randomFoodPos, spawnedFood);
    }

    private void ProcessIfSpawnedFoodNotEaten(Vector2Int foodPos, FoodController spawnedFood) // remove spawned from via events
    {
        if (model.SpawnedFoods.ContainsValue(spawnedFood))
        {
            //spawnedFood.gameObject.SetActive(false);
            GetEatenFoodAndRemoveFromDictionary(foodPos);
        }
    }

    private PowerUpView GetEatenPowerupAndRemoveFromDictionary(Vector2Int powerUpPos)
    {
        if (model.SpawnedPowerUps.TryGetValue(powerUpPos, out PowerUpView powerUpToDestroy))
        {
            model.SpawnedPowerUps.Remove(powerUpPos);
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

    private FoodController GetEatenFoodAndRemoveFromDictionary(Vector2Int foodPosition)
    {
        if (model.SpawnedFoods.TryGetValue(foodPosition, out FoodController eatenFood))
        {
            model.SpawnedFoods.Remove(foodPosition);
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
