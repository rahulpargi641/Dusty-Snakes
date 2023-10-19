

public class FoodView : Item
{
    public FoodType foodType;

    public FoodController Controller { private get; set; }
}

public enum FoodType
{
    MassGainer, MassBurner
}




