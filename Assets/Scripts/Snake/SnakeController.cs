using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController: MonoBehaviour
{
    public static event Action<int> onFoodEaten;
    public static event Action onSnakeDeath;

    private SnakeModel model;
    private SnakeView view;

    public SnakeController(SnakeModel model, SnakeView view)
    {
        this.model = model;
        this.view = view;

        view.Controller = this;
    }


    private void Start()
    {
        model = new SnakeModel();

        for (int i=0; i< 2; i++) // Create 2 Snake Body Part
        AddSnakeBodyPart();

    }

    private void Update()
    {
        switch(model.SnakeState)
        {
            case ESnakeState.Alive:
                SetFacingDirection(); // Snake Facing Direction based on Input
                ProcessTranslation(); // Moving Snake One point to another
                break;
            case ESnakeState.Dead:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Food eatenFood = collision.GetComponent<Food>();
        if (eatenFood)
        {
            ProcessSnakeEating(eatenFood);
            eatenFood.gameObject.SetActive(false);
            // Spawn food
            return;
        }

        PowerUp eatenPowerUp = collision.GetComponent<PowerUp>();
        if(eatenPowerUp)
        {
            ProcessSnakePoweringUp(eatenPowerUp);
            eatenPowerUp.gameObject.SetActive(false);
            // Spawn Powerup
        }
    }

    private void SetFacingDirection()
    {
        //if(m_InputPreference == EInput.ArrowKeys)
            ProcessBasedOnInputPreference1();

        //if (m_InputPreference == EInput.WASDKeys)
        //    ProcessBasedOnInputPreference2();
    }

    private void ProcessBasedOnInputPreference1()
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

    //private void ProcessBasedOnInputPreference2()
    //{
    //    if (Input.GetKeyDown(KeyCode.UpArrow))
    //    {
    //        if (currentFacingDir != EDirection.Down)
    //        {
    //            currentFacingDir = EDirection.Up;
    //        }
    //    }

    //    if (Input.GetKeyDown(KeyCode.DownArrow))
    //    {
    //        if (currentFacingDir != EDirection.Up)
    //        {
    //            currentFacingDir = EDirection.Down;
    //        }
    //    }

    //    if (Input.GetKeyDown(KeyCode.LeftArrow))
    //    {
    //        if (currentFacingDir != EDirection.Right)
    //        {
    //            currentFacingDir = EDirection.Left;
    //        }
    //    }

    //    if (Input.GetKeyDown(KeyCode.RightArrow))
    //    {
    //        if (currentFacingDir != EDirection.Left)
    //        {
    //            currentFacingDir = EDirection.Right;
    //        }
    //    }
    //}

    private void ProcessTranslation()
    {
        model.MoveTimer += Time.deltaTime;

        if (model.MoveTimer > model.MoveTimerMax)
        {
            model.MoveTimer -= model.MoveTimerMax;

            StoreSnakeHeadPositionAndDirection();

            MoveSnakeHead();

            ProcessIfMovedWithoutEatingFood();

            MoveSnakeBodyParts();

            ProcessIfSnakeBiteItself();

            //ProcessSnakeBitingOtherSnake();
        }
    }

    private void StoreSnakeHeadPositionAndDirection()
    {
        SnakeVector snakeHeadVector = new SnakeVector(model.CurrentSnakeHeadPos, model.CurrentFacingDir);
        model.SnakeHeadPositions.Insert(0, snakeHeadVector);
    }

    private void MoveSnakeHead()
    {
        Vector2Int nextMmovePoint = CalculateNextMovePoint();
        model.CurrentSnakeHeadPos += nextMmovePoint;

        ///m_CurrentSnakeHeadPos = m_LevelController.GetNewMovePointIfSnakeWentOutsideGrid(m_CurrentSnakeHeadPos);
        model.CurrentSnakeHeadPos = GetNewMovePointIfSnakeWentOutsideGrid(model.CurrentSnakeHeadPos);

        transform.position = new Vector3(model.CurrentSnakeHeadPos.x, model.CurrentSnakeHeadPos.y);
    }

    public Vector2Int GetNewMovePointIfSnakeWentOutsideGrid(Vector2Int snakePosition)
    {
        if (snakePosition.x < 0)
        {
            snakePosition.x = LevelService.Instance.GetLevelWidth() - 1;
        }
        if (snakePosition.x > LevelService.Instance.GetLevelWidth() - 1)
        {
            snakePosition.x = 0;
        }
        if (snakePosition.y < 0)
        {
            snakePosition.y = LevelService.Instance.GetLevelHeight() - 1;
        }
        if (snakePosition.y > LevelService.Instance.GetLevelHeight() - 1)
        {
            snakePosition.y = 0;
        }
        return snakePosition;
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


    private void ProcessSnakeEating(Food eatenFood)
    {

        if (eatenFood.foodType == FoodType.MassGainer)
        {
            model.MassGainerFoodEatenCounter++;
            AddSnakeBodyPart();
            AddScore(eatenFood.m_PointGain);
            Debug.LogWarning("Snake ate the MassGainer food!" + model.MassGainerFoodEatenCounter);
            AudioService.Instance.PlaySound(SoundType.AteFood);
        }
        else if (eatenFood.foodType == FoodType.MassBurner)
        {
            model.MassBurnerFoodEatenCounter++;
            RemoveSnakeBodyPart();
            AddScore(eatenFood.m_PointGain);
            Debug.LogWarning("Snake ate the MassGainer food!" + model.MassGainerFoodEatenCounter);
            AudioService.Instance.PlaySound(SoundType.AteFood);
        }
        else
        {
            Debug.LogError("Wrong Logic, Food was not Eaten");
        }
    }

    private void ProcessSnakePoweringUp(PowerUp eatenPowerUp)
    {
        if (eatenPowerUp.powerUpType == PowerUpType.Shield)
        {
            model.ShieldActive = true;
            StartCoroutine(ShieldCoolDown());
            AudioService.Instance.PlaySound(SoundType.PowerupShiledPickup);
            ChangeSnakeBodyColor(ColorController.Instance.Blue);
            Debug.Log("SHield Power Eaten");
        }
        else if (eatenPowerUp.powerUpType == PowerUpType.ScoreBoost)
        {
            model.ScoreBoostActive = true;
            StartCoroutine(ScoreBoostCoolDown());
            AudioService.Instance.PlaySound(SoundType.PowerupScoreBoosterPickup);
            ChangeSnakeBodyColor(ColorController.Instance.Violet);
            Debug.Log("ScooreBoost Eaten");
        }
        else if (eatenPowerUp.powerUpType == PowerUpType.SpeedUp)
        {
            Time.timeScale = 2f;
            StartCoroutine(SpeedUpCoolDown());
            AudioService.Instance.PlaySound(SoundType.PowerupSpeedUpPickup);
            ChangeSnakeBodyColor(ColorController.Instance.Red);
            Debug.LogError("SPeed Up Consumed");
        }
        else
        {
            Debug.Log("Wrong Logic, Eaten Powerup Item Must be set");
        }
    }

    private void ProcessIfMovedWithoutEatingFood()
    {
        if (model.SnakeHeadPositions.Count >= model.SnakeBodySize + 1)
        {
            model.SnakeHeadPositions.RemoveAt(model.SnakeHeadPositions.Count - 1);
        }
    }


    internal void AddScore(int pointGain)
    {
        if (model.ScoreBoostActive) pointGain *= 2;
        onFoodEaten?.Invoke(pointGain);
    }

    private IEnumerator ScoreBoostCoolDown()
    {
        yield return new WaitForSeconds(10f);
        model.ScoreBoostActive = false;
        ChangeToNormalColor();

    }

    private IEnumerator ShieldCoolDown()
    {
        yield return new WaitForSeconds(10f);
        model.ShieldActive = false;
        ChangeToNormalColor();
    }

    private IEnumerator SpeedUpCoolDown()
    {
        yield return new WaitForSeconds(30f);
        Time.timeScale = 1f;
        ChangeToNormalColor();
    }

    void ChangeToNormalColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void ProcessIfSnakeBiteItself()
    {
        foreach (SnakeBodyPart snakeBodyPart in model.SnakeBodyParts)
        {
            Vector2Int snakeBodyPartPos = snakeBodyPart.GetSnakeBodyPartPosition();

            if (model.CurrentSnakeHeadPos == snakeBodyPartPos)
            {
                if (model.ShieldActive)
                {
                    return;
                }
                model.SnakeState = ESnakeState.Dead;
                //m_LevelController.SnakeCollided();
                onSnakeDeath?.Invoke();
                AudioService.Instance.PlaySound(SoundType.Death);
            }
        }
    }

    void AddSnakeBodyPart()
    {
        model.SnakeBodyParts.Add(new SnakeBodyPart(model.SnakeBodyParts.Count));
        model.SnakeBodySize++;
    }

    void RemoveSnakeBodyPart()
    {
        if (model.SnakeBodySize < 1) return;

        GameObject snakeBodyPartToDestroy =  model.SnakeBodyParts[model.SnakeBodyParts.Count - 1].GetSnakeBodyPart();
        Destroy(snakeBodyPartToDestroy);

        model.SnakeBodyParts.RemoveAt(model.SnakeBodyParts.Count - 1);
        model.SnakeBodySize--;
    }

    private void MoveSnakeBodyParts()
    {
        for (int i = 0; i < model.SnakeBodyParts.Count; i++)
        {
            model.SnakeBodyParts[i].SetSnakeBodyPartPosition(model.SnakeHeadPositions[i]);
        }
    }

    private void ChangeSnakeBodyColor(Color changeToColor)
    {
        gameObject.GetComponent<SpriteRenderer>().color = changeToColor;
    }

    Vector2Int GetCurrentSnakeHeadPos()
    {
        return model.CurrentSnakeHeadPos;
    }
 

    // Return the full list of positions occupied by the snake: Head + Body 
    public List<Vector2Int> GetWholeSnakeBodyPositions()
    {
        List<Vector2Int> snakeFullBodyPositionList = new List<Vector2Int>() { model.CurrentSnakeHeadPos };

        foreach(SnakeVector snakeHeadPositionVector in model.SnakeHeadPositions)
        {
            snakeFullBodyPositionList.Add(snakeHeadPositionVector.GetGridPosition());
        }
        return snakeFullBodyPositionList;
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
