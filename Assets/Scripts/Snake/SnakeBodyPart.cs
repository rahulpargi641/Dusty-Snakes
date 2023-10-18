using UnityEngine;

public class SnakeBodyPart
{
    SnakeVector snakeBodyPartPositionVector;
    Transform transform;
    GameObject snakeBody;

    public SnakeBodyPart(int bodyIndex) // body index - count of SnakeBodyPart list
    {
        snakeBody = new GameObject("SnakeBody", typeof(SpriteRenderer));
        snakeBody.GetComponent<SpriteRenderer>().sprite = GameAsset.Instance.m_SnakeBody.GetComponent<SpriteRenderer>().sprite;
        snakeBody.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
        transform = snakeBody.transform;
    }

    public void SetSnakeBodyPartPosition(SnakeVector snakeHeadPositionVector)
    {
        snakeBodyPartPositionVector = snakeHeadPositionVector;
        transform.position = new Vector3(snakeHeadPositionVector.GetGridPosition().x, snakeHeadPositionVector.GetGridPosition().y);
        float angle;
        switch (snakeHeadPositionVector.GetDirection())
        {
            default:
            case EDirection.Up:  // Currently going up
                angle = 0;
                break;

            case EDirection.Down: // Currently going down
                angle = 180;
                break;

            case EDirection.Left: // Currently going to the left 
                angle = -90;
                break;

            case EDirection.Right: // Currently going to the Right
                angle = 90;
                break;
        }

        transform.eulerAngles = new Vector3(0, 0, angle);
    }
    public Vector2Int GetSnakeBodyPartPosition()
    {
        return snakeBodyPartPositionVector.GetGridPosition();
    }

    public GameObject GetSnakeBodyPart()
    {
        return snakeBody;
    }
}
