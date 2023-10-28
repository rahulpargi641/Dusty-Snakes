using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpService : MonoSingletonGeneric<PowerUpService>
{
    [SerializeField] PowerUpView[] powerUps;
    private List<PowerUpController> powerUpControllers = new List<PowerUpController>();

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to the events
    }

    public PowerUpController SpawnRandomPowerUp(Vector2 spawnPoint)
    {
        PowerUpModel powerUpModel = new PowerUpModel();

        int randomIdx = Random.Range(0, powerUps.Length);
        PowerUpView powerUpView = Instantiate(powerUps[randomIdx], spawnPoint, Quaternion.identity);

        PowerUpController powerUpController = new PowerUpController(powerUpModel, powerUpView);

        powerUpControllers.Add(powerUpController);

        return powerUpController;
    }
}
