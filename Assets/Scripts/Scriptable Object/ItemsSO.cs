using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Item")]
public class ItemsSO : ScriptableObject
{
    public ItemsView itemsView;
    public int foodSpawnIntervalDelay = 8;
    public int powerUpSpawnIntervalDelay = 12;
    public Vector2 itemSpawnArea = new Vector2(29, 29);
}
