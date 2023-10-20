using UnityEngine;

public class FoodPool : ObjectPoolGeneric<FoodController>
{
    private FoodModel model;
    private FoodView view;

    public void Initialize(FoodModel model, FoodView view)
    {
        this.model = model;
        this.view = view;
    }

    public FoodController GetFood()
    {
        return GetItemFromPool();
    }

    protected override FoodController CreateItem()
    {
        FoodView foodView = Object.Instantiate(view);
        return new FoodController(model, foodView);
    }
}
