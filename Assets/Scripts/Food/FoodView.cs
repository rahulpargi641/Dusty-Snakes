using UnityEngine;

public class FoodView : Item
{
    public FoodType foodType;
    public FoodController Controller { private get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<SnakeView>())
        {
            gameObject.SetActive(false);
        }
    }
}

public enum FoodType
{
    MassGainer, MassBurner
}




