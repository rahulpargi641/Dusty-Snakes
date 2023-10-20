using UnityEngine;

public class PowerUpPool : ObjectPoolGeneric<PowerUpController>
{
    private PowerUpModel model;
    private PowerUpView view;

    public void Initialize(PowerUpModel model, PowerUpView view)
    {
        this.model = model;
        this.view = view;
    }

    public PowerUpController GetFood()
    {
        return GetItemFromPool();
    }

    protected override PowerUpController CreateItem()
    {
        PowerUpView powerUpView = Object.Instantiate(view);
        return new PowerUpController(model, powerUpView);
    }
}