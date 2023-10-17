using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModel
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int ScoreToWin { get; private set; }
    public int CurrentScore { get; set; } = 0;
    public GameState GameState { get; set; } = GameState.Running;

    public LevelController Controller { private get; set; }
    
    // Scriptable Object
    public LevelModel()
    {
        Width = 30;
        Height = 30;
        ScoreToWin = 100;
    }
}
