using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsModel
{
    public int ItemSpawnInterwalDelay { get; private set; }
    public Food EatenFoodItem { get; set; }
    public PowerUp EatenPowerUpItem { get; set; }
    public Dictionary<Vector2Int, Food> Foods { get; set; } = new Dictionary<Vector2Int, Food>();
    public Dictionary<Vector2Int, PowerUp> PowerUps { get; set; } = new Dictionary<Vector2Int, PowerUp>();
    public int LevelWidth { get; set; }
    public int LevelHeight { get; set; }
    

    public ItemsModel()
    {
        ItemSpawnInterwalDelay = 1;
    }
}
