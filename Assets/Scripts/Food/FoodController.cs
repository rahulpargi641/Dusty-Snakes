using System.Threading.Tasks;
using UnityEngine;

public class FoodController
{
    private FoodModel model;
    private FoodView view;

    public FoodController(FoodModel model, FoodView view)
    {
        this.model = model;
        this.view = view;

        view.Controller = this;
        model.Controller = this;
    }

    public void ProcessFoodEaten(SnakeView snakeView)
    {
        snakeView.FoodEaten(model.FoodType);
        FoodService.Instance.ReturnFoodToPool(this);
    }

    public void SetTransform(Vector2 spawnPoint)
    {
        view.transform.position = spawnPoint;
    }

    public void EnableFood()
    {
        view.gameObject.SetActive(true);
        SendFoodToPoolAsync();
    }

    private async void SendFoodToPoolAsync()
    {
        await Task.Delay(model.activeDuration * 1000);

        FoodService.Instance.ReturnFoodToPool(this);
    }

    public void DisableFood()
    {
        view.gameObject.SetActive(false);
    }
}
