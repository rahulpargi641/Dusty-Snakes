
public class FoodModel : Item
{
    public FoodController Controller { private get; set; }
    private FoodSO foodSO;
    public FoodModel(FoodSO foodSO)
    {
        this.foodSO = foodSO;
        pointGain = foodSO.pointGain;
        destroyAfterTime = foodSO.destroyAfterTime;
    }
}
