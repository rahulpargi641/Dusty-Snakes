using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController
{
    private PowerUpModel model;
    private PowerUpView view;

    public PowerUpController(PowerUpModel model, PowerUpView view)
    {
        this.model = model;
        this.view = view;

        view.Controller = this;
        model.Controller = this;
    }
}
