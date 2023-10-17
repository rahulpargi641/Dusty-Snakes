using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerUp : Item
{
    public PowerUpType powerUpType;
}

public enum PowerUpType
{
    Shield, ScoreBoost, SpeedUp
}
