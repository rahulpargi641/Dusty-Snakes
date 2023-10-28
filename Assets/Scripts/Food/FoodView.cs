using UnityEngine;

public class FoodView : MonoBehaviour
{
    public FoodController Controller { private get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SnakeView snakeView = collision.GetComponent<SnakeView>();
        if (snakeView)
        {
            snakeView.FoodEaten(Controller.GetFoodType());
            Controller.InvokeOnFoodEaten();

            gameObject.SetActive(false);
        }
    }
}

public enum FoodType
{
    MassGainer, MassBurner
}




