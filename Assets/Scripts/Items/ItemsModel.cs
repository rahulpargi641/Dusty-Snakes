
public class ItemsModel
{ 
    public int FoodSpawnIntervalDelay { get; private set; }
    public int PowerUpSpawnIntervalDelay { get; private set; }
    public int SpawnAreaWidth { get; set; }
    public int SpawnAreaHeight { get; set; } 
    public bool GameRunning { get; set; } = true;

    private ItemsSO itemsSO;
    
    public ItemsModel(ItemsSO itemsSO)
    {
        this.itemsSO = itemsSO;

        FoodSpawnIntervalDelay = itemsSO.foodSpawnIntervalDelay;
        PowerUpSpawnIntervalDelay = itemsSO.powerUpSpawnIntervalDelay;
        SpawnAreaWidth = (int)itemsSO.itemSpawnArea.x;
        SpawnAreaHeight = (int)itemsSO.itemSpawnArea.y;
    }
}
