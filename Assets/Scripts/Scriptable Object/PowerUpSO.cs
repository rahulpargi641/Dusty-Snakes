using UnityEngine;

[CreateAssetMenu(fileName = "NewPowerUp", menuName = "ScriptableObjects/PowerUp")]
public class PowerUpSO : ScriptableObject
{
    public PowerUpView powerUpView;
    public PowerUpType powerUpType;
    public int activeDuration;
    public int pointGain;
}
