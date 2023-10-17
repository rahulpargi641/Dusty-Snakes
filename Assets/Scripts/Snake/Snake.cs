using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    enum EDirection
    {
        Left, Right, Up, Down
    }
    enum ESnakeState
    {
        Alive, Dead
    }
    enum EInput
    {
        WASDKeys, ArrowKeys
    }

    EDirection ED_CurrentFacingDir;
    ESnakeState ES_SnakeState;

    [SerializeField] EInput m_InputPreference;
    [SerializeField] Vector2Int m_InititalSnakePos;
    [SerializeField] Snake m_OtherSnake;

    [SerializeField] LevelController m_LevelController;
    [SerializeField] ScoreController m_ScoreController;
    [SerializeField] ItemsController m_ItemController;
    [SerializeField] ColorController m_ColorController;

    Vector2Int m_CurrentSnakeHeadPos;
    int m_SnakeBodySize;
    List<SnakeVector> m_SnakeHeadPositions;
    List<SnakeBodyPart> m_SnakeBodyParts;

    float m_MoveTimer;
    float m_MoveTimerMax;
    int m_MassGainerFoodEatenCounter;
    int m_MassBurnerFoodEatenCounter;
    bool m_ShieldActive = false;
    bool m_ScoreBoostActive = false;

    private void Awake()
    {
        m_SnakeHeadPositions = new List<SnakeVector>();
        m_SnakeBodyParts = new List<SnakeBodyPart>();

        m_CurrentSnakeHeadPos = m_InititalSnakePos;
        ED_CurrentFacingDir = EDirection.Right;
        m_SnakeBodySize = 0;
        ES_SnakeState = ESnakeState.Alive;

        m_MoveTimerMax = 0.2f;
        m_MoveTimer = m_MoveTimerMax;
    }

    private void Start()
    {
        for(int i=0; i< 2; i++) // Create 2 Snake Body Part
        AddSnakeBodyPart();
    }

    private void Update()
    {
        switch(ES_SnakeState)
        {
            case ESnakeState.Alive:
                ProcessFacingDirection(); // Snake Facing Direction based on Input
                ProcessTranslation(); // Moving Snake One point to another
                break;
            case ESnakeState.Dead:
                break;
        }
    }

    private void ProcessFacingDirection()
    {
        if(m_InputPreference == EInput.ArrowKeys)
            ProcessBasedOnInputPreference1();

        if (m_InputPreference == EInput.WASDKeys)
            ProcessBasedOnInputPreference2();
    }

    private void ProcessBasedOnInputPreference1()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (ED_CurrentFacingDir != EDirection.Down)
            {
                ED_CurrentFacingDir = EDirection.Up;
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (ED_CurrentFacingDir != EDirection.Up)
            {
                ED_CurrentFacingDir = EDirection.Down;
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (ED_CurrentFacingDir != EDirection.Right)
            {
                ED_CurrentFacingDir = EDirection.Left;
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (ED_CurrentFacingDir != EDirection.Left)
            {
                ED_CurrentFacingDir = EDirection.Right;
            }
        }
    }

    private void ProcessBasedOnInputPreference2()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (ED_CurrentFacingDir != EDirection.Down)
            {
                ED_CurrentFacingDir = EDirection.Up;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (ED_CurrentFacingDir != EDirection.Up)
            {
                ED_CurrentFacingDir = EDirection.Down;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (ED_CurrentFacingDir != EDirection.Right)
            {
                ED_CurrentFacingDir = EDirection.Left;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (ED_CurrentFacingDir != EDirection.Left)
            {
                ED_CurrentFacingDir = EDirection.Right;
            }
        }
    }

    private void ProcessTranslation()
    {
        m_MoveTimer += Time.deltaTime;

        if (m_MoveTimer > m_MoveTimerMax)
        {
            m_MoveTimer -= m_MoveTimerMax;

            StoreSnakeHeadPositionAndDirection();

            ProcessSnakeHeadTranslation();

            ProcessInCaseSnakeEatsFood();

            ProcessInCaseSnakeEatsPowerUp();

            ProcessMovingWithoutEatingFood();

            ProcessSnakeBodyPartsTranslation();

            ProcessSnakeAliveOrNot();

            ProcessSnakeBitingOtherSnake();
        }
    }

    private void StoreSnakeHeadPositionAndDirection()
    {
        SnakeVector snakeHeadVector = new SnakeVector(m_CurrentSnakeHeadPos, ED_CurrentFacingDir);
        m_SnakeHeadPositions.Insert(0, snakeHeadVector);
    }

    private void ProcessSnakeHeadTranslation()
    {
        Vector2Int nextPoint = CalculateNextPoint();
        m_CurrentSnakeHeadPos += nextPoint;
        m_CurrentSnakeHeadPos = m_LevelController.InCaseSnakeWentOusideCalculateNewPos(m_CurrentSnakeHeadPos);

        transform.position = new Vector3(m_CurrentSnakeHeadPos.x, m_CurrentSnakeHeadPos.y);
    }

    private Vector2Int CalculateNextPoint()
    {
        Vector2Int nextPoint;

        switch (ED_CurrentFacingDir)
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


    private void ProcessInCaseSnakeEatsFood()
    {
        bool foodItemEaten = m_ItemController.IsFoodEaten(m_CurrentSnakeHeadPos);

        if (foodItemEaten)
        {
            Food EatenFoodItem = m_ItemController.GetEatenFood();
            Food.EFoodType EatenFoodItemType= EatenFoodItem.EF_FoodType;
            if (EatenFoodItemType == Food.EFoodType.MassGainer)
            {
                m_MassGainerFoodEatenCounter++;
                AddSnakeBodyPart();
                AddScore(EatenFoodItem.m_PointGain);
                Debug.LogWarning("Snake ate the MassGainer food!" + m_MassGainerFoodEatenCounter);
                AudioService.Instance.PlaySound(SoundType.AteFood);
            }
            else if (EatenFoodItemType == Food.EFoodType.MassBurner)
            {
                m_MassBurnerFoodEatenCounter++;
                RemoveSnakeBodyPart();
                AddScore(EatenFoodItem.m_PointGain);
                Debug.LogWarning("Snake ate the MassGainer food!" + m_MassGainerFoodEatenCounter);
                AudioService.Instance.PlaySound(SoundType.AteFood);
            }
            else
            {
                Debug.LogError("Wrong Logic, Food was not Eaten");
            }
        }
    }
    private void ProcessInCaseSnakeEatsPowerUp()
    {
        bool powerUpEaten = m_ItemController.IsPowerUpEaten(m_CurrentSnakeHeadPos);
        if(powerUpEaten)
        {
            Debug.LogError("Snake Ate the Powerup Item");
            PowerUps.EPowerType EatenPowerUpItemType = m_ItemController.GetEatenPowerUpItemType();
            if(EatenPowerUpItemType == PowerUps.EPowerType.Shield)
            {
                m_ShieldActive = true;
                StartCoroutine(ShieldCoolDown());
                AudioService.Instance.PlaySound(SoundType.PowerupShiledPickup);
                ChangeSnakeBodyColor(m_ColorController.Blue);
                Debug.Log("SHield Power Eaten");
            }
            else if (EatenPowerUpItemType == PowerUps.EPowerType.ScoreBoost)
            {
                m_ScoreBoostActive = true;
                StartCoroutine(ScoreBoostCoolDown());
                AudioService.Instance.PlaySound(SoundType.PowerupScoreBoosterPickup);
                ChangeSnakeBodyColor(m_ColorController.Violet);
                Debug.Log("ScooreBoost Eaten");
            }
            else if(EatenPowerUpItemType == PowerUps.EPowerType.SpeedUp)
            {
                Time.timeScale = 2f;
                StartCoroutine(SpeedUpCoolDown());
                AudioService.Instance.PlaySound(SoundType.PowerupSpeedUpPickup);
                ChangeSnakeBodyColor(m_ColorController.Red);
                Debug.LogError("SPeed Up Consumed");
            }
            else
            {
                Debug.Log("Wrong Logic, Eaten Powerup Item Must be set");
            }
        }
    }

    private void ProcessMovingWithoutEatingFood()
    {
        if (m_SnakeHeadPositions.Count >= m_SnakeBodySize + 1)
        {
            m_SnakeHeadPositions.RemoveAt(m_SnakeHeadPositions.Count - 1);
        }
    }

    public bool GetScoreBoostStatus()
    {
        return m_ScoreBoostActive;
    }

    internal void AddScore(int pointGain)
    {
        if (m_ScoreBoostActive) pointGain *= 2;
        m_ScoreController.UpdateScore(pointGain);
    }

    private IEnumerator ScoreBoostCoolDown()
    {
        yield return new WaitForSeconds(10f);
        m_ScoreBoostActive = false;
        ChangeToNormalColor();

    }

    private IEnumerator ShieldCoolDown()
    {
        yield return new WaitForSeconds(10f);
        m_ShieldActive = false;
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

    private void ProcessSnakeAliveOrNot()
    {
        foreach (SnakeBodyPart snakeBodyPart in m_SnakeBodyParts)
        {
            Vector2Int snakeBodyPartPos = snakeBodyPart.GetSnakeBodyPartPosition();
            if (m_CurrentSnakeHeadPos == snakeBodyPartPos)
            {
                if (m_ShieldActive)
                {
                    return;
                }
                ES_SnakeState = ESnakeState.Dead;
                m_LevelController.SnakeCollided();
                AudioService.Instance.PlaySound(SoundType.Death);
            }
        }
    }

    void AddSnakeBodyPart()
    {
        m_SnakeBodyParts.Add(new SnakeBodyPart(m_SnakeBodyParts.Count));
        m_SnakeBodySize++;
    }

    void RemoveSnakeBodyPart()
    {
        if (m_SnakeBodySize < 1) return;

        GameObject snakeBodyPartToDestroy =  m_SnakeBodyParts[m_SnakeBodyParts.Count - 1].GetSnakeBodyPart();
        Destroy(snakeBodyPartToDestroy);

        m_SnakeBodyParts.RemoveAt(m_SnakeBodyParts.Count - 1);
        m_SnakeBodySize--;
    }

    private void ProcessSnakeBodyPartsTranslation()
    {
        for (int i = 0; i < m_SnakeBodyParts.Count; i++)
        {
            m_SnakeBodyParts[i].SetSnakeBodyPartPosition(m_SnakeHeadPositions[i]);
        }
    }

    private void ChangeSnakeBodyColor(Color changeToColor)
    {
        gameObject.GetComponent<SpriteRenderer>().color = changeToColor;
    }

    Vector2Int GetCurrentSnakeHeadPos()
    {
        return m_CurrentSnakeHeadPos;
    }
    public Vector2Int GetGridPos()
    {
        return m_CurrentSnakeHeadPos;
    }

    // Return the full list of positions occupied by the snake: Head + Body 
    public List<Vector2Int> GetWholeSnakeBodyPositions()
    {
        List<Vector2Int> snakeFullBodyPositionList = new List<Vector2Int>() { m_CurrentSnakeHeadPos };

        foreach(SnakeVector snakeHeadPositionVector in m_SnakeHeadPositions)
        {
            snakeFullBodyPositionList.Add(snakeHeadPositionVector.GetGridPosition());
        }
        return snakeFullBodyPositionList;
    }

    private void ProcessSnakeBitingOtherSnake()
    {
        Vector2Int otherSnakeHeadCurrentPos = m_OtherSnake.GetCurrentSnakeHeadPos();

        foreach (SnakeBodyPart snakeBodyPart in m_SnakeBodyParts)
        {
            Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetSnakeBodyPartPosition();
            if (otherSnakeHeadCurrentPos == snakeBodyPartGridPosition)
            {
                if (m_ShieldActive)
                {
                    Debug.LogError("SHiled is Active Snake cannot die");
                    return;
                }
                ES_SnakeState = ESnakeState.Dead;
                m_LevelController.SnakeCollided();
                Debug.Log("Snake Collied With Other Snake Body!");
                AudioService.Instance.PlaySound(SoundType.Death);
            }
        }

    }

    class SnakeVector
    {
        Vector2Int gridPosition;
        EDirection direction;

        public SnakeVector(Vector2Int gridPosition, EDirection direction)
        {
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }

        public EDirection GetDirection()
        {
            return direction;
        }
    }

    class SnakeBodyPart
    {
        SnakeVector snakeBodyPartPositionVector;
        Transform transform;
        GameObject snakeBody;
        public SnakeBodyPart(int bodyIndex) // body index - count of SnakeBodyPart list
        {
            snakeBody = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBody.GetComponent<SpriteRenderer>().sprite = GameAsset.Instance.m_SnakeBody.GetComponent<SpriteRenderer>().sprite;
            snakeBody.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBody.transform;
        }

        public void SetSnakeBodyPartPosition(SnakeVector snakeHeadPositionVector)
        {
            snakeBodyPartPositionVector = snakeHeadPositionVector;
            transform.position = new Vector3(snakeHeadPositionVector.GetGridPosition().x, snakeHeadPositionVector.GetGridPosition().y);
            float angle;
            switch(snakeHeadPositionVector.GetDirection())
            {
                default:
                case EDirection.Up:  // Currently going up
                    angle = 0;
                    break;
                    
                case EDirection.Down: // Currently going down
                    angle = 180;
                    break;

                case EDirection.Left: // Currently going to the left 
                    angle = -90; 
                    break;

                case EDirection.Right: // Currently going to the Right
                    angle = 90;
                    break;
            }
                    
            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        public Vector2Int GetSnakeBodyPartPosition()
        {
            return snakeBodyPartPositionVector.GetGridPosition();
        }

        public GameObject GetSnakeBodyPart()
        {
            return snakeBody;
        }
    }
}
