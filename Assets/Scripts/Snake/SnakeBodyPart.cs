using UnityEngine;

public class SnakeBodyPart
{
    private SnakeVector snakeBodyPartPosVector;
    private GameObject snakeBodyPart;
    //Transform transform;

    public SnakeBodyPart(int bodyPartNo) // body index - count of SnakeBodyPart list
    {
        snakeBodyPart = new GameObject("SnakeBody", typeof(SpriteRenderer));
        snakeBodyPart.GetComponent<SpriteRenderer>().sprite = GameAssetService.Instance.m_SnakeBody.GetComponent<SpriteRenderer>().sprite;
        //snakeBodyPart.GetComponent<SpriteRenderer>().sortingOrder = -bodyPartNo;
        //transform = snakeBodyPart.transform;
    }

    public void SetSnakeBodyPartTransform(SnakeVector snakeHeadPosVector)
    {
        snakeBodyPartPosVector = snakeHeadPosVector;
        snakeBodyPart.transform.position = new Vector3(snakeHeadPosVector.GetSnakePosition().x, snakeHeadPosVector.GetSnakePosition().y);

        float angle;
        switch (snakeHeadPosVector.GetDirection())
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

        snakeBodyPart.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public Vector2Int GetSnakeBodyPartPosition()
    {
        return snakeBodyPartPosVector.GetSnakePosition();
    }

    public GameObject GetSnakeBodyPart()
    {
        return snakeBodyPart;
    }
}
