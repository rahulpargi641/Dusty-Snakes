using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Food : Item
{
    public enum FoodType
    {
        MassGainer, MassBurner
    }

    public FoodType foodType;
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

