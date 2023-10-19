using System.Collections.Generic;
using UnityEngine;

public class PowerUpService : MonoSingletonGeneric<PowerUpService>
{
    [SerializeField] PowerUpSO[] powerUpSOs;
    private List<PowerUpController> powerUpControllers = new List<PowerUpController>();

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to the events
    }

    public PowerUpController SpawnRandomPowerUp(Vector2 spawnPoint)
    {
        int randomIdx = Random.Range(0, powerUpSOs.Length);

        PowerUpModel powerUpModel = new PowerUpModel(powerUpSOs[randomIdx]);
        PowerUpView powerUpView = Instantiate(powerUpSOs[randomIdx].powerUpView, spawnPoint, Quaternion.identity);
        PowerUpController powerUpController = new PowerUpController(powerUpModel, powerUpView);

        powerUpControllers.Add(powerUpController);

        return powerUpController;
    }
}
