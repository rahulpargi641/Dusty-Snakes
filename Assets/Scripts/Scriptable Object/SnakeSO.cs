using UnityEngine;

[CreateAssetMenu(fileName = "NewSnake", menuName = "ScriptableObjects/Snake")]
public class SnakeSO : ScriptableObject
{
    public SnakeView snakeView;
    public SnakeType snakeType;
    public Vector2Int spawnPos;

    public float snakeMoveTimerMax = 0.2f;
    public int PowerUpCoolDownTime = 10;
}

public enum SnakeType
{
    Snake1, Snake2
}