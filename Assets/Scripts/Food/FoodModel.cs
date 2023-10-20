
public class FoodModel : Item
{
    public FoodController Controller { private get; set; }
    public FoodType FoodType { get; private set; }

    private FoodSO foodSO;
    public FoodModel(FoodSO foodSO)
    {
        this.foodSO = foodSO;
        FoodType = foodSO.foodType;
        pointGain = foodSO.pointGain;
        activeDurationTime = foodSO.destroyAfterTime;
    }
}
