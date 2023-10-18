using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsService : MonoSingletonGeneric<ItemsService>
{
    private ItemsController itemsController;

    // Start is called before the first frame update
    void Start()
    {
        CreateItemController();
    }

    private void CreateItemController()
    {
        ItemsModel itemsModel = new ItemsModel();
        //itemsController = new ItemsController(itemsModel);
    }
}
