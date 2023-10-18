using System.Collections.Generic;
using UnityEngine;

public class SnakeModel
{
    public EDirection CurrentFacingDir { get; set; }
    public ESnakeState SnakeState { get; set; }
    public Vector2Int CurrentSnakeHeadPos { get; set; }
    public int SnakeBodySize { get; set; } = 0;
    public List<SnakeVector> SnakeHeadPositions { get; set; } = new List<SnakeVector>();
    public List<SnakeBodyPart> SnakeBodyParts { get; set; } = new List<SnakeBodyPart>();
    public float MoveTimer { get; set; }
    public float MoveTimerMax { get; private set; }
    public int MassGainerFoodEatenCounter { get; set; }
    public int MassBurnerFoodEatenCounter { get; set; }
    public bool ShieldActive { get; set; } = false;
    public bool ScoreBoostActive { get; set; } = false;
    public Vector2Int InititalSnakePos { get; private set; }

    public SnakeModel()
    {
        CurrentSnakeHeadPos = new Vector2Int(10, 10);
        CurrentFacingDir = EDirection.Right;
        SnakeState = ESnakeState.Alive;

        MoveTimerMax = 0.2f;
        MoveTimer = MoveTimerMax;
    }
}