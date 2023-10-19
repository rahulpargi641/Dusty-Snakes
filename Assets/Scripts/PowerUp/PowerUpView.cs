using UnityEngine;

public class PowerUpView : Item
{
    public PowerUpType powerUpType;
    public PowerUpController Controller { private get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SnakeView snakeView = collision.GetComponent<SnakeView>();
        if (snakeView)
        {
            snakeView.PowerUpEaten(this);
            Controller.InvokeOnFoodEaten();

            gameObject.SetActive(false);
        }
    }
}

public enum PowerUpType
{
    Shield, ScoreBoost, SpeedUp
}
