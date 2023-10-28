using UnityEngine;

public class FoodView : MonoBehaviour
{
    public FoodController Controller { private get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SnakeView snakeView = collision.GetComponent<SnakeView>();
        if (snakeView)
            Controller.ProcessFoodEaten(snakeView);
    }
}

public enum FoodType
{
    MassGainer, MassBurner
}




