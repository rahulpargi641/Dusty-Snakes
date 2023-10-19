using System.Collections.Generic;
using UnityEngine;

public class ItemsModel
{ 
    public int FoodSpawnIntervalDelay { get; private set; }
    public int PowerUpSpawnIntervalDelay { get; private set; }
    public float SpawnAreaWidth { get; set; }
    public float SpawnAreaHeight { get; set; } 
    public bool GameRunning { get; set; } = true;
    public bool FoodEaten { get; set; } = false;
    public bool PowerUpEaten { get; set; } = false;

    private ItemsSO itemsSO;
    
    public ItemsModel(ItemsSO itemsSO)
    {
        this.itemsSO = itemsSO;

        FoodSpawnIntervalDelay = itemsSO.foodSpawnIntervalDelay;
        PowerUpSpawnIntervalDelay = itemsSO.powerUpSpawnIntervalDelay;
        SpawnAreaWidth = itemsSO.itemSpawnArea.x;
        SpawnAreaHeight = itemsSO.itemSpawnArea.y;
    }
}
