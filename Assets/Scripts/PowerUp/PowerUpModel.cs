
public class PowerUpModel : Item
{
    public PowerUpController Controller { private get; set; }
    public PowerUpType powerUpType { get; private set; }

    private PowerUpSO powerUpSO;

    public PowerUpModel(PowerUpSO powerUpSO)
    {
        this.powerUpSO = powerUpSO;
        powerUpType = powerUpSO.powerUpType;
        activeDuration = powerUpSO.activeDuration;
    }
}
