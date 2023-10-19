using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SnakeController
{
    public static event Action<int> onFoodEaten;
    public static event Action onSnakeDeath;

    private SnakeModel model;
    private SnakeView view;

    public SnakeController(SnakeModel model, SnakeView view)
    {
        this.model = model;
        this.view = view;

        this.view.Controller = this;
    }

    public void CreateTwoBodyParts()
    {
        for (int i = 0; i < 2; i++)
            AddSnakeBodyPart();
    }

    public void ProcessSnakeTranslation() // called every frame
    {
        switch (model.SnakeState)
        {
            case ESnakeState.Alive:
                SetFacingDirection(); // Set Snake Facing Direction based on Input
                ProcessTranslation(); // Moving Snake One point to another
                break;
            case ESnakeState.Dead:
                break;
        }
    }

    private void SetFacingDirection() // called every frame
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (model.CurrentFacingDir != EDirection.Down)
            {
                model.CurrentFacingDir = EDirection.Up;
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (model.CurrentFacingDir != EDirection.Up)
            {
                model.CurrentFacingDir = EDirection.Down;
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (model.CurrentFacingDir != EDirection.Right)
            {
                model.CurrentFacingDir = EDirection.Left;
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (model.CurrentFacingDir != EDirection.Left)
            {
                model.CurrentFacingDir = EDirection.Right;
            }
        }
    }

    private void ProcessTranslation()
    {
        model.TimePassed += Time.deltaTime;

        if (model.TimePassed > model.MoveTimerMax)
        {
            model.TimePassed -= model.MoveTimerMax;

            StoreSnakeHeadPositionAndDirection();

            MoveSnakeHead();

            ProcessIfSnakeMovedWithoutEatingFood();

            MoveSnakeBodyParts();

            ProcessIfSnakeBiteItself();

            //ProcessSnakeBitingOtherSnake();
        }
    }

    private void StoreSnakeHeadPositionAndDirection()
    {
        SnakeVector snakeHeadVector = new SnakeVector(model.CurrentSnakeHeadPos, model.CurrentFacingDir);
        model.SnakeHeadPosVectors.Insert(0, snakeHeadVector);
    }

    private void MoveSnakeHead()
    {
        Vector2Int nextMmovePoint = CalculateNextMovePoint();
        model.CurrentSnakeHeadPos += nextMmovePoint;
        model.CurrentSnakeHeadPos = GetNewMovePointIfSnakeWentOutsideGrid(model.CurrentSnakeHeadPos);

        view.transform.position = new Vector3(model.CurrentSnakeHeadPos.x, model.CurrentSnakeHeadPos.y);
    }

    private Vector2Int CalculateNextMovePoint()
    {
        Vector2Int nextPoint;

        switch (model.CurrentFacingDir)
        {
            default:
            case EDirection.Right:
                nextPoint = new Vector2Int(+1, 0);
                break;
            case EDirection.Left:
                nextPoint = new Vector2Int(-1, 0);
                break;
            case EDirection.Up:
                nextPoint = new Vector2Int(0, +1);
                break;
            case EDirection.Down:
                nextPoint = new Vector2Int(0, -1);
                break;
        }

        return nextPoint;
    }

    public Vector2Int GetNewMovePointIfSnakeWentOutsideGrid(Vector2Int currentSnakeHeadPos)
    {
        if (currentSnakeHeadPos.x < 0)
        {
            currentSnakeHeadPos.x = LevelService.Instance.GetLevelWidth() - 1;
        }
        if (currentSnakeHeadPos.x > LevelService.Instance.GetLevelWidth() - 1)
        {
            currentSnakeHeadPos.x = 0;
        }
        if (currentSnakeHeadPos.y < 0)
        {
            currentSnakeHeadPos.y = LevelService.Instance.GetLevelHeight() - 1;
        }
        if (currentSnakeHeadPos.y > LevelService.Instance.GetLevelHeight() - 1)
        {
            currentSnakeHeadPos.y = 0;
        }
        return currentSnakeHeadPos;
    }

    private void ProcessIfSnakeMovedWithoutEatingFood()
    {
        if (model.SnakeHeadPosVectors.Count >= model.SnakeBodySize + 1)
        {
            model.SnakeHeadPosVectors.RemoveAt(model.SnakeHeadPosVectors.Count - 1);
        }
    }

    private void MoveSnakeBodyParts()
    {
        for (int i = 0; i < model.SnakeBodyParts.Count; i++)
        {
            model.SnakeBodyParts[i].SetSnakeBodyPartTransform(model.SnakeHeadPosVectors[i]);
        }
    }

    public void ProcessSnakeEatingFood(FoodView eatenFood)
    {
        if (eatenFood.foodType == FoodType.MassGainer)
        {
            model.MassGainerFoodEatenCounter++;
            AddSnakeBodyPart();
            Debug.LogWarning("Snake ate the MassGainer food!" + model.MassGainerFoodEatenCounter);
            AudioService.Instance.PlaySound(SoundType.AteFood);
        }
        else if (eatenFood.foodType == FoodType.MassBurner)
        {
            model.MassBurnerFoodEatenCounter++;
            RemoveSnakeBodyPart();
            Debug.LogWarning("Snake ate the MassGainer food!" + model.MassGainerFoodEatenCounter);
            AudioService.Instance.PlaySound(SoundType.AteFood);
        }
        else
        {
            Debug.LogError("Wrong Logic, Food was not Eaten");
        }

        AddScore(eatenFood.pointGain);
    }

    public void ProcessSnakeEatingPowerUp(PowerUpView eatenPowerUp)
    {
        if (eatenPowerUp.powerUpType == PowerUpType.Shield)
        {
            ActivateShieldAsync();
            AudioService.Instance.PlaySound(SoundType.PowerupShiledPickup);
            Debug.Log("SHield Power Eaten");
        }
        else if (eatenPowerUp.powerUpType == PowerUpType.ScoreBoost)
        {
            ActivateScoreBoostAsync();

            AudioService.Instance.PlaySound(SoundType.PowerupScoreBoosterPickup);
            Debug.Log("ScooreBoost Eaten");
        }
        else if (eatenPowerUp.powerUpType == PowerUpType.SpeedUp)
        {
            ActivateSpeedBoostAsync();
            AudioService.Instance.PlaySound(SoundType.PowerupSpeedUpPickup);
            Debug.LogError("SPeed Up Consumed");
        }
        else
        {
            Debug.Log("Eaten Powerup type is not set.");
        }
    }

    private async void ActivateScoreBoostAsync()
    {
        model.ScoreBoostActive = true;
        ChangeSnakeBodyColor(ColorController.Instance.Violet);

        await Task.Delay(model.PowerUpCoolDownTime * 1000);

        model.ScoreBoostActive = false;
        ChangeToNormalColor();

    }

    private async void ActivateShieldAsync()
    {
        model.ShieldActive = true;
        ChangeSnakeBodyColor(ColorController.Instance.Blue);

        await Task.Delay(model.PowerUpCoolDownTime * 1000);

        model.ShieldActive = false;
        ChangeToNormalColor();
    }

    private async void ActivateSpeedBoostAsync()
    {
        Time.timeScale = 2f;
        ChangeSnakeBodyColor(ColorController.Instance.Red);

        await Task.Delay(model.PowerUpCoolDownTime * 1000);
        
        Time.timeScale = 1f;
        ChangeToNormalColor();
    }

    private void ChangeSnakeBodyColor(Color changeToColor)  // change entire snake body
    {
        view.GetComponent<SpriteRenderer>().color = changeToColor;
    }

    void ChangeToNormalColor()
    {
        view.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void AddScore(int pointGain)
    {
        if (model.ScoreBoostActive)
            pointGain *= 2;

        onFoodEaten?.Invoke(pointGain);
    }

    private void ProcessIfSnakeBiteItself()
    {
        foreach (SnakeBodyPart snakeBodyPart in model.SnakeBodyParts)
        {
            Vector2Int snakeBodyPartPos = snakeBodyPart.GetSnakeBodyPartPosition();

            if (model.CurrentSnakeHeadPos == snakeBodyPartPos)
            {
                if (model.ShieldActive) return;

                model.SnakeState = ESnakeState.Dead;
                onSnakeDeath?.Invoke();
                AudioService.Instance.PlaySound(SoundType.Death);
            }
        }
    }

    void AddSnakeBodyPart()
    {
        model.SnakeBodyParts.Add(new SnakeBodyPart(model.SnakeBodyParts.Count)); // get from the pool
        model.SnakeBodySize++;
    }

    void RemoveSnakeBodyPart()
    {
        if (model.SnakeBodySize < 1) return;

        GameObject snakeBodyPartToDestroy =  model.SnakeBodyParts[model.SnakeBodyParts.Count - 1].GetSnakeBodyPart();
        snakeBodyPartToDestroy.SetActive(false); // send back to pool

        model.SnakeBodyParts.RemoveAt(model.SnakeBodyParts.Count - 1);
        model.SnakeBodySize--;
    }

    public List<Vector2Int> GetWholeSnakeBodyPositions()  // Return the full list of positions occupied by the snake: Head + Body 
    {
        List<Vector2Int> snakeFullBodyPosVectors = new List<Vector2Int>() { model.CurrentSnakeHeadPos };

        foreach(SnakeVector snakeHeadVector in model.SnakeHeadPosVectors)
        {
            snakeFullBodyPosVectors.Add(snakeHeadVector.GetSnakePosition());
        }
        return snakeFullBodyPosVectors;
    }


    // Later***

    //private void ProcessSnakeBitingOtherSnake()
    //{
    //    Vector2Int otherSnakeHeadCurrentPos = m_OtherSnake.GetCurrentSnakeHeadPos();

    //    foreach (SnakeBodyPart snakeBodyPart in m_SnakeBodyParts)
    //    {
    //        Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetSnakeBodyPartPosition();
    //        if (otherSnakeHeadCurrentPos == snakeBodyPartGridPosition)
    //        {
    //            if (m_ShieldActive)
    //            {
    //                Debug.LogError("SHiled is Active Snake cannot die");
    //                return;
    //            }
    //            snakeState = ESnakeState.Dead;
    //            //m_LevelController.SnakeCollided();
    //            onSnakeDeath?.Invoke();
    //            Debug.Log("Snake Collied With Other Snake Body!");
    //            AudioService.Instance.PlaySound(SoundType.Death);
    //        }
    //    }

    //}

}
