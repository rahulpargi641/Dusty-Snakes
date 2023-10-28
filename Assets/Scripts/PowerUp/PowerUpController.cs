using System.Threading.Tasks;
using UnityEngine;

public class PowerUpController
{
    private PowerUpModel model;
    private PowerUpView view;

    public PowerUpController(PowerUpModel model, PowerUpView view)
    {
        this.model = model;
        this.view = view;

        view.Controller = this;
        model.Controller = this;
    }

    public void ProcessPowerUpEaten(SnakeView snakeView)
    {
        snakeView.PowerUpEaten(model.powerUpType);
        PowerUpService.Instance.ReturnPowerUpToPool(this);
    }

    public void SetTransform(Vector2 spawnPoint)
    {
        view.transform.position = spawnPoint;
    }

    public void EnablePowerUp()
    {
        view.gameObject.SetActive(true);
        SendPowerUpToPoolAsync();
    }

    private async void SendPowerUpToPoolAsync()
    {
        await Task.Delay(model.activeDuration * 1000);

        PowerUpService.Instance.ReturnPowerUpToPool(this);
    }

    public void DisablePowerUp()
    {
        view.gameObject.SetActive(false);
    }
}
