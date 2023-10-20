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
        }
    }

    private void SpawnFoodAtRandomPosition(out Vector2Int randomFoodPos, out FoodController spawnedFood)
    {
        randomFoodPos = GenerateRandomPosNotAtSnakeBody();
        spawnedFood = FoodService.Instance.SpawnRandomFood(randomFoodPos);
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
        }
    }

    private void SpawnPowerUpAtRandomPosition(out Vector2Int randomPowerUpPos, out PowerUpController spawnedPowerUp)
    {
        randomPowerUpPos = GenerateRandomPosNotAtSnakeBody();

        spawnedPowerUp = PowerUpService.Instance.SpawnRandomPowerUp(randomPowerUpPos);
    }

    private Vector2Int GenerateRandomPosNotAtSnakeBody()
    {
        Vector2Int RandomPowerUpPos;
        do
        {
            RandomPowerUpPos = new Vector2Int(UnityEngine.Random.Range(1, model.SpawnAreaWidth - 1), UnityEngine.Random.Range(1, model.SpawnAreaHeight - 1));
        } while (SnakeService.Instance.GetWholeSnakeBodyPositions().IndexOf(RandomPowerUpPos) != -1);

        return RandomPowerUpPos;
    }
}
