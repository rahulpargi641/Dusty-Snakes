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
    public void FoodEaten(FoodView eatenFood)
    {
        Controller.ProcessSnakeEatingFood(eatenFood);
    }

    // called by PowerUpView
    public void PowerUpEaten(PowerUpView eatenPowerUp)
    {
        Controller.ProcessSnakeEatingPowerUp(eatenPowerUp);
    }
}
