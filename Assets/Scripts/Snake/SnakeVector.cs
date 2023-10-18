using UnityEngine;

public class SnakeVector
{
    Vector2Int snakePosition;
    EDirection direction;

    public SnakeVector(Vector2Int snakePosition, EDirection direction)
    {
        this.snakePosition = snakePosition;
        this.direction = direction;
    }

    public Vector2Int GetSnakePosition()
    {
        return snakePosition;
    }

    public EDirection GetDirection()
    {
        return direction;
    }
}
