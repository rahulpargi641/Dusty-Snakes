using UnityEngine;

public class SnakeView : MonoBehaviour
{
    public SnakeController Controller { private get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Controller.CreateTwoBodyParts();
    }

    // Update is called once per frame
    void Update()
    {
        Controller.ProcessSnakeTranslation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Food eatenFood = collision.GetComponent<Food>();
        if (eatenFood)
        {
            Controller.ProcessSnakeEatingFood(eatenFood);
            eatenFood.gameObject.SetActive(false);
            // Spawn food
            return;
        }

        PowerUp eatenPowerUp = collision.GetComponent<PowerUp>();
        if (eatenPowerUp)
        {
            Controller.ProcessSnakeEatingPowerUp(eatenPowerUp);
            eatenPowerUp.gameObject.SetActive(false);
            // Spawn Powerup
        }
    }
}
