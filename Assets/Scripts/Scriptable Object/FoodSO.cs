using UnityEngine;

[CreateAssetMenu(fileName = "NewFood", menuName = "ScriptableObjects/Food")]
public class FoodSO : ScriptableObject
{
    public FoodView foodView;
    public int pointGain;
    public int destroyAfterTime;
}
