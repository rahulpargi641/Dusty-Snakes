using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsService : MonoSingletonGeneric<ItemsService>
{
    [SerializeField] ItemsView itemsView;
    private ItemsController itemsController;

    // Start is called before the first frame update
    void Start()
    {
        CreateItemsController();
    }

    private void CreateItemsController()
    {
        ItemsModel itemsModel = new ItemsModel();
        ItemsView itemsView = Instantiate(this.itemsView);
        itemsController = new ItemsController(itemsModel, itemsView);
    }
}
