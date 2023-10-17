using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModel
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public LevelController Controller { private get; set; }
    public LevelModel()
    {
        Width = 30;
        Height = 30;
    }
}
