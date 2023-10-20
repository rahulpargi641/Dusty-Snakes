using UnityEngine;
using UnityEngine.InputSystem;

public class SnakeView : MonoBehaviour
{
    Vector2 moveInput1;
    Vector2 moveInput2;
    public SnakeController Controller { private get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Controller.CreateTwoBodyParts();
    }

    // Update is called once per frame
    void Update()
    {
        if (Controller.GetSnakeType() == SnakeType.Snake1)
        {
            Controller.ProcessSnakeTranslation(moveInput1);
        }
        else if (Controller.GetSnakeType() == SnakeType.Snake2)
        {
            Controller.ProcessSnakeTranslation(moveInput2);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Controller.ProcessSnakeDeath();
    }

    // Input System Message
    void OnMove1(InputValue value)
    {
        moveInput1 = value.Get<Vector2>();
        Debug.Log(moveInput1);
    }
    
    // Input System Message
    void OnMove2(InputValue value)
    {
        moveInput2 = value.Get<Vector2>();
        Debug.Log(moveInput2);
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
