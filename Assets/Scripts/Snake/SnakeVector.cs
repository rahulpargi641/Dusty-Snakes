using UnityEngine;

public class SnakeVector
{
    Vector2Int gridPosition;
    EDirection direction;

    public SnakeVector(Vector2Int gridPosition, EDirection direction)
    {
        this.gridPosition = gridPosition;
        this.direction = direction;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public EDirection GetDirection()
    {
        return direction;
    }
}
