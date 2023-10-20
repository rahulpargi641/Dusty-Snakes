using UnityEngine;
using System;

public class GameAssetService : MonoSingletonGeneric<GameAssetService> // didn't change the previously written code, snakeBody gameobject should be the part of SnakeBodyPart scriptable object
{
    public GameObject m_SnakeBody;
}
