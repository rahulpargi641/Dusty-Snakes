using System.Collections.Generic;
using UnityEngine;

public class ItemsModel
{ 
    public int FoodSpawnIntervalDelay { get; private set; }
    public int PowerUpSpawnIntervalDelay { get; private set; }
    public FoodView EatenFoodItem { get; set; }
    public PowerUpView EatenPowerUpItem { get; set; }
    public Dictionary<Vector2Int, FoodController> SpawnedFoods { get; set; } = new Dictionary<Vector2Int, FoodController>();
    public Dictionary<Vector2Int, PowerUpController> SpawnedPowerUps { get; set; } = new Dictionary<Vector2Int, PowerUpController>();
    public int LevelWidth { get; set; } // Spawn Area
    public int LevelHeight { get; set; } // Spawn Area

    private ItemsSO itemsSO;
    
    public ItemsModel(ItemsSO itemsSO)
    {
        this.itemsSO = itemsSO;

        FoodSpawnIntervalDelay = itemsSO.foodSpawnIntervalDelay;
        PowerUpSpawnIntervalDelay = itemsSO.powerUpSpawnIntervalDelay;
        LevelWidth = itemsSO.levelWidth;
        LevelHeight = itemsSO.levelHeight;
    }
}
