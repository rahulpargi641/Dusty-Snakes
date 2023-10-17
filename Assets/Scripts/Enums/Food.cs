using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Food : Item
{
    public enum EFoodType
    {
        MassGainer, MassBurner
    }

    public EFoodType EF_FoodType;
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


