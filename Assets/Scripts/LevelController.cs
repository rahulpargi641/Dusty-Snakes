using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    enum EGameState
    {
        Pause, Running
    }

    EGameState EG_GameState;

    [SerializeField] GameObject PauseGO;
    [SerializeField] GameObject GameWinGO;
    [SerializeField] GameObject GameOverGO;
    [SerializeField] ScoreController m_ScoreController;
    [SerializeField] int m_MaxScore;
    [SerializeField] int m_Width;
    [SerializeField] int m_Height;

    bool m_SnakeCollided;
    bool m_TimerExpired;

    private void Awake()
    {
        EG_GameState = EGameState.Running;
        m_SnakeCollided = false;
        m_TimerExpired = false;
    }

    private void Update()
    {
        ProcessPauseResumeCheck();
        ProcessGameWinCheck();
        ProceesGameOverCheck();
    }

    private void ProceesGameOverCheck()
    {
        if(m_SnakeCollided)
        {
            GameOverGO.SetActive(true);
        }
        else if(m_TimerExpired)
        {
            GameOverGO.SetActive(true);
        }
    }

    private void ProcessPauseResumeCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space) && EG_GameState == EGameState.Running)
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && EG_GameState == EGameState.Pause)
        {
            ResumeGame();
        }
    }

    private void ProcessGameWinCheck()
    {
        int currentScore = m_ScoreController.GetCurrentScore();
        if (currentScore >= m_MaxScore)
        {
            GameWinGO.SetActive(true);
        }
    }
    private void ResumeGame()
    {
        Time.timeScale = 1;
        EG_GameState = EGameState.Running;
        PauseGO.SetActive(false);
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        EG_GameState = EGameState.Pause;
        PauseGO.SetActive(true);
    }

    public int GetLevelGridHeight()
    {
        return m_Height;
    }
    public int GetLevelGridWidth()
    {
        return m_Width;
    }

    public void SnakeCollided()
    {
        m_SnakeCollided = true;
    }
    public void TimerExpired()
    {
        m_TimerExpired = true;
    }

 
    public Vector2Int ProcessIfSnakeWentOutsideTheGrid(Vector2Int snakePosition)
    {
        if(snakePosition.x < 0)
        {
            snakePosition.x = m_Width - 1;
        }
        if(snakePosition.x > m_Width - 1)
        {
            snakePosition.x = 0;
        }
        if(snakePosition.y < 0)
        {
            snakePosition.y = m_Height - 1;
        }
        if(snakePosition.y > m_Width-1)
        {
            snakePosition.y = 0;
        }
        return snakePosition;
    }
}
