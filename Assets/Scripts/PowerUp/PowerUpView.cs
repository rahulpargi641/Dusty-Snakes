using UnityEngine;

public class PowerUpView : Item
{
    public PowerUpType powerUpType;
    public PowerUpController Controller { private get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<SnakeView>())
        {
            gameObject.SetActive(false);
        }
    }
}

public enum PowerUpType
{
    Shield, ScoreBoost, SpeedUp
}
