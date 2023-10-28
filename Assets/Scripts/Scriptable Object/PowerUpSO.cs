using UnityEngine;

[CreateAssetMenu(fileName = "NewPowerUp", menuName = "ScriptableObjects/PowerUp")]
public class PowerUpSO : ScriptableObject
{
    public PowerUpView powerUpView;
    public PowerUpType powerUpType;
    public int PowerUpDuration;
    public int pointGain;
}
