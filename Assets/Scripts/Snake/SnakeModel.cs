using System.Collections.Generic;
using UnityEngine;

public class SnakeModel
{
    public EDirection CurrentFacingDir { get; set; }
    public ESnakeState SnakeState { get; set; }
    public Vector2Int CurrentSnakeHeadPos { get; set; }
    public int SnakeBodySize { get; set; } = 0;
    public List<SnakeVector> SnakeHeadPosVectors { get; set; } = new List<SnakeVector>();
    public List<SnakeBodyPart> SnakeBodyParts { get; set; } = new List<SnakeBodyPart>();
    public float snakeMoveTimer { get; set; }
    public float snakeMoveTimerMax { get; private set; }
    public int MassGainerFoodEatenCounter { get; set; }
    public int MassBurnerFoodEatenCounter { get; set; }
    public int PowerUpCoolDownTime { get; private set; }
    public bool ShieldActive { get; set; } = false;
    public bool ScoreBoostActive { get; set; } = false;

    private SnakeSO snakeSO;
    public SnakeModel(SnakeSO snakeSO)
    {
        this.snakeSO = snakeSO;

        CurrentSnakeHeadPos = new Vector2Int(10, 10);
        CurrentFacingDir = EDirection.Right;
        SnakeState = ESnakeState.Alive;

        snakeMoveTimerMax = snakeSO.snakeMoveTimerMax;
        snakeMoveTimer = snakeMoveTimerMax;

        PowerUpCoolDownTime = snakeSO.PowerUpCoolDownTime;
    }
}
