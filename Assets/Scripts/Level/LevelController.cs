using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController
{
    private LevelModel model;
    private LevelView view;

    public LevelController(LevelModel model, LevelView view)
    {
        this.model = model;
        this.view = view;

        view.Controller = this;
        model.Controller = this;
    }

    public int GetLevelWidth()
    {
        return model.Width;
    }

    public int GetLevelHeight()
    {
        return model.Height;
    }

    public void TimerExpired()
    {
        view.TimerExpired();
    }

    public void SnakeCollided()
    {
        view.SnakeCollided();
    }

}
