using UnityEngine;

public class ItemsView : MonoBehaviour
{
    public Food[] foods;
    public PowerUp[] powerUps;

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
