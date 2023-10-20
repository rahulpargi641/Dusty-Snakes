using UnityEngine;

public class LevelView : MonoBehaviour
{
    [SerializeField] GameObject gamePauseGO;
    [SerializeField] GameObject levelWinGO;
    [SerializeField] GameObject gameOverGO;
    [SerializeField] GameObject speedBoostGO;
    [SerializeField] GameObject scoreBoostGO;
    [SerializeField] GameObject shieldGO;

    public LevelController Controller { private get; set; }

    private void Update()
    {
        ProcessGameStateChange();

        Controller.ProcessIfLevelWin(); // Achivement System
    }

    private void ProcessGameStateChange()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Controller.GetGameState() == EGameState.Running)
                PauseGame();
            else if (Controller.GetGameState() == EGameState.Pause)
                ResumeGame();
        }
    }
  
    private void ResumeGame()
    {
        Time.timeScale = 1;
        Controller.SetGameState(EGameState.Running);
        gamePauseGO.SetActive(false);
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        Controller.SetGameState(EGameState.Pause);
        gamePauseGO.SetActive(true);
    }

    public void EnableLevelWinGO()
    {
        levelWinGO.SetActive(true);
    }

    public void EnableGameOverGO()
    {
        gameOverGO.SetActive(true);
    }

    public void EnableSpeedBoostText()
    {
        speedBoostGO.SetActive(true);
    }
    public void DisableSpeedBoostText()
    {
        speedBoostGO.SetActive(false);
    }

    public void EnableScoreBoostText()
    {
        scoreBoostGO.SetActive(true);
    }
    public void DisableScoreBoostText()
    {
        scoreBoostGO.SetActive(false);
    }

    public void EnableShieldText()
    {
        shieldGO.SetActive(true);
    }

    public void DisableShieldText()
    {
        shieldGO.SetActive(false);
    }
}
