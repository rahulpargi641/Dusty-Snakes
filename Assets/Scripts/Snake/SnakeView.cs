using System;
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

    // called by FoodView
    public void FoodEaten(FoodType eatenFoodType)
    {
        Controller.ProcessSnakeEatingFood(eatenFoodType);
    }

    // called by PowerUpView
    public void PowerUpEaten(PowerUpType eatenPowerUpType)
    {
        Controller.ProcessSnakeEatingPowerUp(eatenPowerUpType);
    }
}
