using System.Collections;
using System.Collections.Generic;
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


}
