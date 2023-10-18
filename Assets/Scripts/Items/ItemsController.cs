using System;
using System.Threading.Tasks;
using UnityEngine;

public class ItemsController: MonoBehaviour
{
    [SerializeField] Food[] m_FoodArray;
    [SerializeField] PowerUp[] m_PowerUpsArray;

    private ItemsModel model;
    public ItemsController(ItemsModel model)
    {
        this.model = model;

        model.LevelWidth = LevelService.Instance.GetLevelWidth();
        model.LevelHeight = LevelService.Instance.GetLevelHeight();

        SpawnFoodItem();
        SpawnPowerUpItem();
    }

    private async void SpawnFoodItem()
    {
        while (true)
        {
            Vector2Int randomFoodPos;
            randomFoodPos = GenerateRandomFoodNotAtSnakeBody();

            Food foodToSpawn = RandomItemSelector(m_FoodArray);
            Food food = GameObject.Instantiate(foodToSpawn, new Vector3(randomFoodPos.x, randomFoodPos.y), Quaternion.identity);
            model.Foods.Add(randomFoodPos, food);

            //yield return new WaitForSeconds(foodToSpawn.destroyAfterTime);
            await Task.Delay(foodToSpawn.destroyAfterTime * 1000);

            if (model.Foods.ContainsValue(food))
            {
                //Destroy(food.gameObject);
                food.gameObject.SetActive(false);
                GetEatenFoodAndRemoveFromDictionary(randomFoodPos);
            }

            //yield return new WaitForSeconds(model.ItemSpawnInterwalDelay);
            await Task.Delay(model.ItemSpawnInterwalDelay * 1000);
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


    public async void SpawnPowerUpItem()
    {
        Vector2Int randomPowerUpPos;
        while (true)
        {
            //yield return new WaitForSeconds(5f);
            await Task.Delay(model.ItemSpawnInterwalDelay * 5 * 1000);

            randomPowerUpPos = GenerateRandomPowerUpNotAtSnakeBody();

            PowerUp powerUpToSpawn = RandomItemSelector(m_PowerUpsArray);
            PowerUp powerUp = GameObject.Instantiate(powerUpToSpawn, new Vector3(randomPowerUpPos.x, randomPowerUpPos.y), Quaternion.identity);
            model.PowerUps.Add(randomPowerUpPos, powerUp);

            //yield return new WaitForSeconds(powerUpToSpawn.destroyAfterTime);
            await Task.Delay(powerUpToSpawn.destroyAfterTime * 1000);

            if (model.PowerUps.ContainsValue(powerUp))
            {
                //Destroy(powerUp.gameObject);
                powerUp.gameObject.SetActive(false);
                GetEatenPowerupAndRemoveFromDictionary(randomPowerUpPos);
            }

            //yield return new WaitForSeconds(model.ItemSpawnInterwalDelay);
            await Task.Delay(model.ItemSpawnInterwalDelay * 1000);

        }
    }

    private Vector2Int GenerateRandomPowerUpNotAtSnakeBody()
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

    public bool IsFoodEaten(Vector2Int snakeCurrentPos)
    {
        if (model.Foods.ContainsKey(snakeCurrentPos))
        {
            Food eatenFoodItem = GetEatenFoodAndRemoveFromDictionary(snakeCurrentPos);
            model.EatenFoodItem = eatenFoodItem;
            //Destroy(eatenFoodItem.gameObject);
            eatenFoodItem.gameObject.SetActive(false);
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
            //Destroy(eatenPowerUpItem.gameObject);
            eatenPowerUpItem.gameObject.SetActive(false);
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
