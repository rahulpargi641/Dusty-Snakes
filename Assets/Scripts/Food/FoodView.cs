using UnityEngine;

public class FoodView : Item
{
    public FoodType foodType;
    public FoodController Controller { private get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SnakeView snakeView = collision.GetComponent<SnakeView>();
        if (snakeView)
        {
            snakeView.FoodEaten(this);
            Controller.InvokeOnFoodEaten();

            gameObject.SetActive(false);
        }
    }
}

public enum FoodType
{
    MassGainer, MassBurner
}




