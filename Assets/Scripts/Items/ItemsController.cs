using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsController : MonoBehaviour
{
    [SerializeField] Food[] m_FoodArray;
    [SerializeField] PowerUp[] m_PowerUpsArray;

    private ItemsModel model;
    //public ItemsController(ItemsModel model)
    //{
    //    this.model = model;
    //}

    private void Start()
    {
        model = new ItemsModel();

        model.LevelWidth = LevelService.Instance.GetLevelWidth();
        model.LevelHeight = LevelService.Instance.GetLevelHeight();

        StartCoroutine(SpawnFoodItem());
        StartCoroutine(SpawnPowerUpItem());
    }

    private IEnumerator SpawnFoodItem()
    {
        while (true)
        {
            Vector2Int randomFoodPos;
            randomFoodPos = GenerateRandomFoodNotAtSnakeBody();

            Food foodToSpawn = RandomItemSelector(m_FoodArray);
            Food food = Instantiate(foodToSpawn, new Vector3(randomFoodPos.x, randomFoodPos.y), transform.rotation);
            model.Foods.Add(randomFoodPos, food);

            yield return new WaitForSeconds(foodToSpawn.destroyAfterTime);

            if (model.Foods.ContainsValue(food))
            {
                Destroy(food.gameObject);
                GetEatenFoodAndRemoveFromDictionary(randomFoodPos);
            }

            yield return new WaitForSeconds(model.ItemSpawnInterwalDelay);
        }
    }

    private Vector2Int GenerateRandomFoodNotAtSnakeBody()
    {
        Vector2Int randomFoodPos;
        do
        {
            randomFoodPos = new Vector2Int(UnityEngine.Random.Range(1, model.LevelWidth - 1), UnityEngine.Random.Range(1, model.LevelHeight - 1));
        } while (SnakeService.Instance.GetWholeSnakeBodyPositions().IndexOf(randomFoodPos) != -1);

        return randomFoodPos;
    }


    private IEnumerator SpawnPowerUpItem()
    {
        Vector2Int randomPowerUpPos;
        while (true)
        {
            yield return new WaitForSeconds(5f);
            randomPowerUpPos = GenerateRandomPowerUpNotAtSnakeBody();

            PowerUp powerUpToSpawn = RandomItemSelector(m_PowerUpsArray);
            PowerUp powerUp = Instantiate(powerUpToSpawn, new Vector3(randomPowerUpPos.x, randomPowerUpPos.y), transform.rotation);
            model.PowerUps.Add(randomPowerUpPos, powerUp);

            yield return new WaitForSeconds(powerUpToSpawn.destroyAfterTime);

            if (model.PowerUps.ContainsValue(powerUp))
            {
                Destroy(powerUp.gameObject);
                GetEatenPowerupAndRemoveFromDictionary(randomPowerUpPos);
            }

            yield return new WaitForSeconds(model.ItemSpawnInterwalDelay);

        }
    }

    private Vector2Int GenerateRandomPowerUpNotAtSnakeBody()
    {
        int width = LevelService.Instance.GetLevelWidth();
        int height = LevelService.Instance.GetLevelHeight();

        Vector2Int RandomPowerUpPos;
        do
        {
            RandomPowerUpPos = new Vector2Int(UnityEngine.Random.Range(1, width-1), UnityEngine.Random.Range(1, height-1));
        } while (SnakeService.Instance.GetWholeSnakeBodyPositions().IndexOf(RandomPowerUpPos) != -1);

        return RandomPowerUpPos;
    }

    T RandomItemSelector<T>(T[] items)
    {
        int randomIndex = UnityEngine.Random.Range(0, items.Length);
        return items[randomIndex];
    }

    public bool IsFoodEaten(Vector2Int snakeCurrentPos)
    {
        if (model.Foods.ContainsKey(snakeCurrentPos))
        {
            Food eatenFoodItem = GetEatenFoodAndRemoveFromDictionary(snakeCurrentPos);
            model.EatenFoodItem = eatenFoodItem;
            Destroy(eatenFoodItem.gameObject);
            SpawnFoodItem();
            return true;
        }
        else
            return false;
    }

    public bool IsPowerUpEaten(Vector2Int snakeCurrentPos)
    {
        if (model.PowerUps.ContainsKey(snakeCurrentPos))
        {
            PowerUp eatenPowerUpItem = GetEatenPowerupAndRemoveFromDictionary(snakeCurrentPos);
            model.EatenPowerUpItem = eatenPowerUpItem;
            Destroy(eatenPowerUpItem.gameObject);
            return true;
        }
        else
            return false;
    }

    private PowerUp GetEatenPowerupAndRemoveFromDictionary(Vector2Int snakeCurrentPos)
    {
        if (model.PowerUps.TryGetValue(snakeCurrentPos, out PowerUp powerUpToDestroy))
        {
            model.PowerUps.Remove(snakeCurrentPos);
        }
        return powerUpToDestroy;
    }

 
    private Food GetEatenFoodAndRemoveFromDictionary(Vector2Int removeAtPosition)
    {
        if (model.Foods.TryGetValue(removeAtPosition, out Food eatenFood))
        {
            model.Foods.Remove(removeAtPosition);
        }
        return eatenFood;
    }

    public Food GetEatenFood()
    {
        return model.EatenFoodItem;
    }

    public PowerUp GetEatenPowerUpItemType()
    {
        return model.EatenPowerUpItem;
    }
}
