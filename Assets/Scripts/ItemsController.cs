using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsController : MonoBehaviour
{
    [SerializeField] Food[] m_FoodArray;
    [SerializeField] PowerUps[] m_PowerUpsArray;
    [SerializeField] LevelController m_LevelController;
    [SerializeField] SnakeController m_Snake;
    Food m_EatenFoodItem;
    PowerUps.EPowerType EP_EatenPowerUpItem;
    int m_Width;
    int m_Height;
    int m_ItemSpawnInterwalDelay;
  
    Dictionary<Vector2Int, Food> m_FoodDictionary;
    Dictionary<Vector2Int, PowerUps> m_PowerUpsDictionary;

    private void Awake()
    {
        m_FoodDictionary = new Dictionary<Vector2Int, Food>();
        m_PowerUpsDictionary = new Dictionary<Vector2Int, PowerUps>();
        m_ItemSpawnInterwalDelay = 1;
        m_Width = m_LevelController.GetLevelGridWidth();
        m_Height = m_LevelController.GetLevelGridHeight();
    }

    private void Start()
    {
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
            m_FoodDictionary.Add(randomFoodPos, food);

            yield return new WaitForSeconds(foodToSpawn.m_DestroyAfterTime);

            if (m_FoodDictionary.ContainsValue(food))
            {
                Destroy(food.gameObject);
                GetEatenFoodAndRemoveFromDictionary(randomFoodPos);
            }

            yield return new WaitForSeconds(m_ItemSpawnInterwalDelay);
        }
    }

    private Vector2Int GenerateRandomFoodNotAtSnakeBody()
    {
        Vector2Int randomFoodPos;
        do
        {
            randomFoodPos = new Vector2Int(UnityEngine.Random.Range(1, m_Width-1), UnityEngine.Random.Range(1, m_Height-1));
        } while (m_Snake.GetWholeSnakeBodyPositions().IndexOf(randomFoodPos) != -1);

        return randomFoodPos;
    }


    private IEnumerator SpawnPowerUpItem()
    {
        Vector2Int randomPowerUpPos;
        while (true)
        {
            yield return new WaitForSeconds(5f);
            randomPowerUpPos = GenerateRandomPowerUpNotAtSnakeBody();

            PowerUps powerUpToSpawn = RandomItemSelector(m_PowerUpsArray);
            PowerUps powerUp = Instantiate(powerUpToSpawn, new Vector3(randomPowerUpPos.x, randomPowerUpPos.y), transform.rotation);
            m_PowerUpsDictionary.Add(randomPowerUpPos, powerUp);

            yield return new WaitForSeconds(powerUpToSpawn.m_DestroyAfterTime);

            if (m_PowerUpsDictionary.ContainsValue(powerUp))
            {
                Destroy(powerUp.gameObject);
                GetEatenPowerupAndRemoveFromDictionary(randomPowerUpPos);
            }

            yield return new WaitForSeconds(m_ItemSpawnInterwalDelay);

        }
    }

    private Vector2Int GenerateRandomPowerUpNotAtSnakeBody()
    {
        Vector2Int RandomPowerUpPos;
        do
        {
            RandomPowerUpPos = new Vector2Int(UnityEngine.Random.Range(1, m_Width-1), UnityEngine.Random.Range(1, m_Height-1));
        } while (m_Snake.GetWholeSnakeBodyPositions().IndexOf(RandomPowerUpPos) != -1);
        return RandomPowerUpPos;
    }

    T RandomItemSelector<T>(T[] items)
    {
        int randomIndex = UnityEngine.Random.Range(0, items.Length);
        return items[randomIndex];
    }

    public bool IsFoodEaten(Vector2Int snakeCurrentPos)
    {
        if (m_FoodDictionary.ContainsKey(snakeCurrentPos))
        {
            Food eatenFoodItem = GetEatenFoodAndRemoveFromDictionary(snakeCurrentPos);
            m_EatenFoodItem = eatenFoodItem;
            Destroy(eatenFoodItem.gameObject);
            SpawnFoodItem();
            return true;
        }
        else
            return false;
    }

    public bool IsPowerUpEaten(Vector2Int snakeCurrentPos)
    {
        if (m_PowerUpsDictionary.ContainsKey(snakeCurrentPos))
        {
            PowerUps eatenPowerUpItem = GetEatenPowerupAndRemoveFromDictionary(snakeCurrentPos);
            EP_EatenPowerUpItem = eatenPowerUpItem.EP_PowerType;
            Destroy(eatenPowerUpItem.gameObject);
            return true;
        }
        else
            return false;
    }

    private PowerUps GetEatenPowerupAndRemoveFromDictionary(Vector2Int snakeCurrentPos)
    {
        if (m_PowerUpsDictionary.TryGetValue(snakeCurrentPos, out PowerUps powerUpToDestroy))
        {
            m_PowerUpsDictionary.Remove(snakeCurrentPos);
        }
        return powerUpToDestroy;
    }

 
    private Food GetEatenFoodAndRemoveFromDictionary(Vector2Int removeAtPosition)
    {
        if (m_FoodDictionary.TryGetValue(removeAtPosition, out Food eatenFood))
        {
            m_FoodDictionary.Remove(removeAtPosition);
        }
        return eatenFood;
    }

    public Food GetEatenFood()
    {
        return m_EatenFoodItem;
    }

    public PowerUps.EPowerType GetEatenPowerUpItemType()
    {
        return EP_EatenPowerUpItem;
    }
}
