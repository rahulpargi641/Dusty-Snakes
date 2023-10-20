
public class SnakeBodyPartPool: ObjectPoolGeneric<SnakeBodyPart>
{
    public SnakeBodyPart GetSnakeBodyPart()
    {
        return GetItemFromPool();
    }

    protected override SnakeBodyPart CreateItem()
    {
        return new SnakeBodyPart(0);
    }
}
