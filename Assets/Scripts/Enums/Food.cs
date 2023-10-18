using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Food : Item
{
    public FoodType foodType;
}

public enum FoodType

{
    MassGainer, MassBurner
}

public enum SoundType
{
    ButtonClick,
    AteFood,
    PowerupShiledPickup,
    PowerupScoreBoosterPickup,
    PowerupSpeedUpPickup,
    Death,
    SnakeCollide
}

public enum GameState
{
    Pause, Running
}

public enum EDirection
{
    Left, Right, Up, Down
}

public enum ESnakeState
{
    Alive, Dead
}

public enum EInput
{
    WASDKeys, ArrowKeys
}