using UnityEngine;

[CreateAssetMenu(fileName = "NewFood", menuName = "ScriptableObjects/Food")]
public class FoodSO : ScriptableObject
{
    public FoodView foodView;
    public FoodType foodType;
    public int pointGain;
    public int activeDuration;
}
