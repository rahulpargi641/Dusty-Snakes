using System.Collections.Generic;
using UnityEngine;

public class PowerUpService : MonoSingletonGeneric<PowerUpService>
{
    [SerializeField] PowerUpSO[] powerUpSOs;
    private List<PowerUpController> powerUpControllers = new List<PowerUpController>();
    private Dictionary<PowerUpSO, PowerUpPool> powerUpPools = new Dictionary<PowerUpSO, PowerUpPool>();

    public PowerUpController SpawnRandomPowerUp(Vector2 spawnPoint)
    {
        PowerUpPool selectedPool = SelectPowerUpPool();

        PowerUpController powerUpController = selectedPool.GetFood();
        powerUpController.SetTransform(spawnPoint);
        powerUpController.EnablePowerUp();

        powerUpControllers.Add(powerUpController);

        return powerUpController;
    }

    private PowerUpPool SelectPowerUpPool()
    {
        int randIdx = Random.Range(0, powerUpSOs.Length);
        PowerUpSO selectedPowerUpSO = powerUpSOs[randIdx];

        if (!powerUpPools.ContainsKey(selectedPowerUpSO))
            CreateFoodPool(selectedPowerUpSO);

        PowerUpPool selectedPool = powerUpPools[selectedPowerUpSO];
        return selectedPool;
    }

    private void CreateFoodPool(PowerUpSO selectedPowerUpSO)
    {
        PowerUpModel foodModel = new PowerUpModel(selectedPowerUpSO);
        PowerUpView foodView = selectedPowerUpSO.powerUpView;

        PowerUpPool foodPool = new PowerUpPool();
        foodPool.Initialize(foodModel, foodView);

        powerUpPools[selectedPowerUpSO] = foodPool;
    }

    public void ReturnPowerUpToPool(PowerUpController powerUpController)
    {
        powerUpController.DisablePowerUp();

        foreach (var pool in powerUpPools.Values)
            pool.ReturnItem(powerUpController);
    }
}
