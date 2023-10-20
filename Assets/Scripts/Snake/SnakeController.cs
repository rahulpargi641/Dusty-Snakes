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
        model.snakeMoveTimer += Time.deltaTime;

        if (model.snakeMoveTimer > model.snakeMoveTimerMax)
        {
            model.snakeMoveTimer -= model.snakeMoveTimerMax;

            StoreSnakeHeadPositionAndDirection();

            MoveSnakeHead();

            ProcessIfSnakeMovedWithoutEatingFood();

            MoveSnakeBodyParts();

            ProcessIfSnakeBiteItself();
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

    public void ProcessSnakeEatingFood(FoodType eatenFoodType)
    {
        if (eatenFoodType == FoodType.MassGainer)
        {
            model.MassGainerFoodEatenCounter++;
            AddSnakeBodyPart();
            AudioService.Instance.PlaySound(SoundType.AteFood);
        }
        else if (eatenFoodType == FoodType.MassBurner)
        {
            model.MassBurnerFoodEatenCounter++;
            RemoveSnakeBodyPart();
            AudioService.Instance.PlaySound(SoundType.AteFood);
        }

        InvokeSnakeAteFood(20);  // change function name to InvokeSnakeAteFood
    }

    public void ProcessSnakeEatingPowerUp(PowerUpType eatenPowerUpType)
    {
        if (eatenPowerUpType == PowerUpType.Shield)
        {
            ActivateShieldAsync();
            AudioService.Instance.PlaySound(SoundType.ShieldPickup);
        }
        else if (eatenPowerUpType == PowerUpType.ScoreBoost)
        {
            ActivateScoreBoostAsync();
            AudioService.Instance.PlaySound(SoundType.ScoreBoostPickup);
        }
        else if (eatenPowerUpType == PowerUpType.SpeedBoost)
        {
            ActivateSpeedBoostAsync();
            AudioService.Instance.PlaySound(SoundType.SpeedBoostPickup);
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

    private void ChangeSnakeBodyColor(Color changeToColor)
    {
        view.GetComponent<SpriteRenderer>().color = changeToColor; // change color of snake head
    }

    void ChangeToNormalColor()
    {
        view.GetComponent<SpriteRenderer>().color = Color.white; // change color of snake head
    }

    public void InvokeSnakeAteFood(int pointGain)
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
                ProcessSnakeDeath();
        }
    }

    void AddSnakeBodyPart()
    {
        //model.SnakeBodyParts.Add(new SnakeBodyPart(model.SnakeBodyParts.Count)); // get from the pool
        SnakeBodyPart snakeBodyPart = model.SnakeBodyPartPool.GetSnakeBodyPart();
        model.SnakeBodyParts.Add(snakeBodyPart);
        model.SnakeBodySize++;
        snakeBodyPart.GetSnakeBodyPartGO().gameObject.SetActive(true);
    }

    void RemoveSnakeBodyPart()
    {
        if (model.SnakeBodySize < 1) return;

        SendSnakeBodyPartBackToPool();

        model.SnakeBodyParts.RemoveAt(model.SnakeBodyParts.Count - 1);
        model.SnakeBodySize--;
    }

    private void SendSnakeBodyPartBackToPool()
    {
        SnakeBodyPart snakeBodyPartToRemove = model.SnakeBodyParts[model.SnakeBodyParts.Count - 1];
        GameObject snakeBodyPartToRemoveGO = snakeBodyPartToRemove.GetSnakeBodyPartGO();
        snakeBodyPartToRemoveGO.SetActive(false);
        model.SnakeBodyPartPool.ReturnItem(snakeBodyPartToRemove);
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

    public void ProcessSnakeDeath()
    {
        if (model.ShieldActive) return;

        model.SnakeState = ESnakeState.Dead;
        onSnakeDeath?.Invoke();
        AudioService.Instance.PlaySound(SoundType.SnakeCollide);
    }
}
