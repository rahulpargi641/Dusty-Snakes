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

enum EDirection
{
    Left, Right, Up, Down
}

enum ESnakeState
{
    Alive, Dead
}

enum EInput
{
    WASDKeys, ArrowKeys
}