using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsService : MonoSingletonGeneric<ItemsService>
{
    [SerializeField] ItemsSO itemSO;
    private ItemsController itemsController;

    // Start is called before the first frame update
    void Start()
    {
        CreateItemsController();

        FoodController.onFoodEaten += FoodEaten;
        PowerUpController.onPowerUpEaten += PowerUpEaten;
    }

    private void OnDestroy()
    {
        FoodController.onFoodEaten -= FoodEaten;
        PowerUpController.onPowerUpEaten -= PowerUpEaten;
    }

    private void CreateItemsController()
    {
        ItemsModel itemsModel = new ItemsModel(itemSO);
        ItemsView itemsView = Instantiate(itemSO.itemsView);
        itemsController = new ItemsController(itemsModel, itemsView);
    }

    public void FoodEaten()
    {
        itemsController.FoodEaten();
    }

    public void PowerUpEaten()
    {
        itemsController.PowerUpEaten();
    }
}
