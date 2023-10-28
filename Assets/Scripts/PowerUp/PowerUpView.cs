using UnityEngine;

public class PowerUpView : MonoBehaviour
{
    public PowerUpType powerUpType;
    public PowerUpController Controller { private get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SnakeView snakeView = collision.GetComponent<SnakeView>();
        if (snakeView)
            Controller.ProcessPowerUpEaten(snakeView);
    }
}

public enum PowerUpType
{
    Shield, ScoreBoost, SpeedBoost
}
