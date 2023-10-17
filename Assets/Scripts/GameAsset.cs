using UnityEngine;
using System;

public class GameAsset : MonoBehaviour
{
    public static GameAsset Instance;
    public GameObject m_SnakeBody;
   
    private void Awake()
    {
        Instance = this;
    }

}
