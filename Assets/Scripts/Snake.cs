using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    enum EDirection
    {
        Left, Right, Up, Down
    }
    enum EState
    {
        Alive, Dead
    }
    enum EInput
    {
        WASDKeys, ArrowKeys
    }

    EDirection EDm_FacingDirection;
    EState ESm_State;

    [SerializeField] EInput m_InputPreference;
    [SerializeField] Vector2Int m_InititalSnakePos;
    [SerializeField] Snake m_OtherSnake;

    [SerializeField] LevelController m_LevelController;
    [SerializeField] ScoreController m_ScoreController;
    [SerializeField] ItemsController m_ItemController;
    [SerializeField] ColorController m_ColorController;

    Vector2Int m_CurrentSnakeHeadPos;
    int m_SnakeBodySize;
    List<PositionVector> m_SnakeHeadPositonVectorList;
    List<SnakeBodyPart> m_SnakeBodyPartsList;

    float m_MoveTimer;
    float m_MoveTimerMax;
    int m_MassGainerFoodEatenCounter;
    int m_MassBurnerFoodEatenCounter;
    bool m_ShieldActive = false;
    bool m_ScoreBoostActive = false;

    private void Awake()
    {
        m_SnakeHeadPositonVectorList = new List<PositionVector>();
        m_SnakeBodyPartsList = new List<SnakeBodyPart>();

        m_CurrentSnakeHeadPos = m_InititalSnakePos;
        EDm_FacingDirection = EDirection.Right;
        m_SnakeBodySize = 0;
        ESm_State = EState.Alive;

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
        switch(ESm_State)
        {
            case EState.Alive:
                ProcessFacingDirection(); // Snake Facing Direction based on Input
                ProcessTranslation(); // Moving Snake One point to another
                break;
            case EState.Dead:
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
            if (EDm_FacingDirection != EDirection.Down)
            {
                EDm_FacingDirection = EDirection.Up;
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (EDm_FacingDirection != EDirection.Up)
            {
                EDm_FacingDirection = EDirection.Down;
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (EDm_FacingDirection != EDirection.Right)
            {
                EDm_FacingDirection = EDirection.Left;
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (EDm_FacingDirection != EDirection.Left)
            {
                EDm_FacingDirection = EDirection.Right;
            }
        }
    }

    private void ProcessBasedOnInputPreference2()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (EDm_FacingDirection != EDirection.Down)
            {
                EDm_FacingDirection = EDirection.Up;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (EDm_FacingDirection != EDirection.Up)
            {
                EDm_FacingDirection = EDirection.Down;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (EDm_FacingDirection != EDirection.Right)
            {
                EDm_FacingDirection = EDirection.Left;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (EDm_FacingDirection != EDirection.Left)
            {
                EDm_FacingDirection = EDirection.Right;
            }
        }
    }

    private void ProcessTranslation()
    {
        m_MoveTimer += Time.deltaTime;

        if (m_MoveTimer > m_MoveTimerMax)
        {
            m_MoveTimer -= m_MoveTimerMax;

            ProcessStoreSnakeHeadPositionAndFacingDirection();
            ProcessSnakeHeadTranslation();
            ProcessInCaseEatingFoodItem();
            ProcessInCaseEatingPowerUpItem();
            ProcessMovingWithoutEatingFood();
            ProcessSnakeBodyPartsTranslation();
            ProcessSnakeAliveOrNot();
            ProcessSnakeBitingOtherSnake();
        }
    }

    private void ProcessSnakeBitingOtherSnake()
    {
         Vector2Int otherSnakeHeadCurrentPos = m_OtherSnake.GetCurrentOtherSnakeHeadPos();
     
        foreach (SnakeBodyPart snakeBodyPart in m_SnakeBodyPartsList)
        {
            Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetSnakeBodyPartGridPosition();
            if (otherSnakeHeadCurrentPos == snakeBodyPartGridPosition)
            {
                if (m_ShieldActive)
                {
                    Debug.LogError("SHiled is Active Snake cannot die");
                    return;
                }
                ESm_State = EState.Dead;
                m_LevelController.SnakeCollided();
                Debug.Log("Snake Collied With Other Snake Body!");
                SoundManager.PlaySound(SoundManager.ESound.DeathVoice);
            }
        }

    }

    private void ProcessSnakeHeadTranslation()
    {
        Vector2Int moveToPosition = CalculateMoveToPosition();
        m_CurrentSnakeHeadPos += moveToPosition;
        m_CurrentSnakeHeadPos = m_LevelController.ProcessIfSnakeWentOutsideTheGrid(m_CurrentSnakeHeadPos);
        transform.position = new Vector3(m_CurrentSnakeHeadPos.x, m_CurrentSnakeHeadPos.y);
    }

    private void ProcessStoreSnakeHeadPositionAndFacingDirection()
    {
        PositionVector snakeHeadPositionVector = new PositionVector(m_CurrentSnakeHeadPos, EDm_FacingDirection);
        m_SnakeHeadPositonVectorList.Insert(0, snakeHeadPositionVector);
    }

    private void ProcessInCaseEatingFoodItem()
    {
        bool foodItemEaten = m_ItemController.FoodItemEaten(m_CurrentSnakeHeadPos);

        if (foodItemEaten)
        {
            Food EatenFoodItem = m_ItemController.GetEatenFoodItem();
            Food.EFoodType EatenFoodItemType= EatenFoodItem.EF_FoodType;
            if (EatenFoodItemType == Food.EFoodType.MassGainer)
            {
                m_MassGainerFoodEatenCounter++;
                AddSnakeBodyPart();
                AddScore(EatenFoodItem.m_PointGain);
                Debug.LogWarning("Snake ate the MassGainer food!" + m_MassGainerFoodEatenCounter);
                SoundManager.PlaySound(SoundManager.ESound.AteFood);
            }
            else if (EatenFoodItemType == Food.EFoodType.MassBurner)
            {
                m_MassBurnerFoodEatenCounter++;
                RemoveSnakeBodyPart();
                AddScore(EatenFoodItem.m_PointGain);
                Debug.LogWarning("Snake ate the MassGainer food!" + m_MassGainerFoodEatenCounter);
                SoundManager.PlaySound(SoundManager.ESound.AteFood);
            }
            else
            {
                Debug.LogError("Wrong Logic, Food was not Eaten");
            }
        }
    }
    private void ProcessInCaseEatingPowerUpItem()
    {
        bool powerUpEaten = m_ItemController.PowerUpItemEaten(m_CurrentSnakeHeadPos);
        if(powerUpEaten)
        {
            Debug.LogError("Snake Ate the Powerup Item");
            PowerUps.EPowerType EatenPowerUpItemType = m_ItemController.GetEatenPowerUpItemType();
            if(EatenPowerUpItemType == PowerUps.EPowerType.Shield)
            {
                m_ShieldActive = true;
                StartCoroutine(ShieldCoolDown());
                SoundManager.PlaySound(SoundManager.ESound.PowerupShiledPickup);
                ChangeSnakeBodyColor(m_ColorController.Blue);
                Debug.Log("SHield Power Eaten");
            }
            else if (EatenPowerUpItemType == PowerUps.EPowerType.ScoreBoost)
            {
                m_ScoreBoostActive = true;
                StartCoroutine(ScoreBoostCoolDown());
                SoundManager.PlaySound(SoundManager.ESound.PowerupScoreBoosterPickup);
                ChangeSnakeBodyColor(m_ColorController.Violet);
                Debug.Log("ScooreBoost Eaten");
            }
            else if(EatenPowerUpItemType == PowerUps.EPowerType.SpeedUp)
            {
                Time.timeScale = 2f;
                StartCoroutine(SpeedUpCoolDown());
                SoundManager.PlaySound(SoundManager.ESound.PowerupSpeedUpPickup);
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
        if (m_SnakeHeadPositonVectorList.Count >= m_SnakeBodySize + 1)
        {
            m_SnakeHeadPositonVectorList.RemoveAt(m_SnakeHeadPositonVectorList.Count - 1);
        }
    }

    private Vector2Int CalculateMoveToPosition()
    {
        Vector2Int moveToDirectionVector;
        switch (EDm_FacingDirection)
        {
            default:
            case EDirection.Right:
                moveToDirectionVector = new Vector2Int(+1, 0);
                break;
            case EDirection.Left:
                moveToDirectionVector = new Vector2Int(-1, 0);
                break;
            case EDirection.Up:
                moveToDirectionVector = new Vector2Int(0, +1);
                break;
            case EDirection.Down:
                moveToDirectionVector = new Vector2Int(0, -1);
                break;
        }

        return moveToDirectionVector;
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
        foreach (SnakeBodyPart snakeBodyPart in m_SnakeBodyPartsList)
        {
            Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetSnakeBodyPartGridPosition();
            if (m_CurrentSnakeHeadPos == snakeBodyPartGridPosition)
            {
                if (m_ShieldActive)
                {
                    return;
                }
                ESm_State = EState.Dead;
                m_LevelController.SnakeCollided();
                SoundManager.PlaySound(SoundManager.ESound.DeathVoice);
            }
        }
    }

    void AddSnakeBodyPart()
    {
        m_SnakeBodyPartsList.Add(new SnakeBodyPart(m_SnakeBodyPartsList.Count));
        m_SnakeBodySize++;
    }

    void RemoveSnakeBodyPart()
    {
        if (m_SnakeBodySize < 1) return;

        GameObject snakeBodyPartToDestroy =  m_SnakeBodyPartsList[m_SnakeBodyPartsList.Count - 1].GetSnakeBodyPartGameObject();
        Destroy(snakeBodyPartToDestroy);
        m_SnakeBodyPartsList.RemoveAt(m_SnakeBodyPartsList.Count - 1);
        m_SnakeBodySize--;
    }

    private void ProcessSnakeBodyPartsTranslation()
    {
        for (int i = 0; i < m_SnakeBodyPartsList.Count; i++)
        {
            m_SnakeBodyPartsList[i].SetSnakeBodyPartPosition(m_SnakeHeadPositonVectorList[i]);
        }
    }

    private void ChangeSnakeBodyColor(Color changeToColor)
    {
        gameObject.GetComponent<SpriteRenderer>().color = changeToColor;
    }

    Vector2Int GetCurrentOtherSnakeHeadPos()
    {
        return m_CurrentSnakeHeadPos;
    }
    public Vector2Int GetGridPos()
    {
        return m_CurrentSnakeHeadPos;
    }

    // Return the full list of positions occupied by the snake: Head + Body 
    public List<Vector2Int> GetFullSnakeGridPositionList()
    {
        List<Vector2Int> snakeFullBodyPositionList = new List<Vector2Int>() { m_CurrentSnakeHeadPos };

        foreach(PositionVector snakeHeadPositionVector in m_SnakeHeadPositonVectorList)
        {
            snakeFullBodyPositionList.Add(snakeHeadPositionVector.GetGridPosition());
        }
        return snakeFullBodyPositionList;
    }

    class PositionVector
    {
        Vector2Int gridPosition;
        EDirection direction;

        public PositionVector(Vector2Int gridPosition, EDirection direction)
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
        PositionVector snakeBodyPartPositionVector;
        Transform transform;
        GameObject snakeBodyGameObject;
        public SnakeBodyPart(int bodyIndex) // body index - count of SnakeBodyPart list
        {
            snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAsset.Instance.m_SnakeBody.GetComponent<SpriteRenderer>().sprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetSnakeBodyPartPosition(PositionVector snakeHeadPositionVector)
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

        public Vector2Int GetSnakeBodyPartGridPosition()
        {
            return snakeBodyPartPositionVector.GetGridPosition();
        }

        public GameObject GetSnakeBodyPartGameObject()
        {
            return snakeBodyGameObject;
        }
    }
}
