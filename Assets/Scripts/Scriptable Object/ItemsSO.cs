using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Item")]
public class ItemsSO : ScriptableObject
{
    FoodView[] foods;
    PowerUpView[] powerups;
    public ItemsView itemsView;
    public int foodSpawnIntervalDelay = 1;
    public int powerUpSpawnIntervalDelay = 12;
    public int levelWidth = 30;
    public int levelHeight = 30;
}
