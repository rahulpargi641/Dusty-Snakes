using UnityEngine;

public class ItemsView : MonoBehaviour
{
    public FoodView[] foods;
    public PowerUpView[] powerUps;

    public ItemsController Controller { private get; set; }

    private void Start()
    {
        if (Controller != null)
        {
            StartCoroutine(Controller.SpawnFoodItems());
            StartCoroutine(Controller.SpawnPowerUpItems());
        }
    }
}
