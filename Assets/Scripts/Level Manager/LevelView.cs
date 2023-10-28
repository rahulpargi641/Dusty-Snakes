using UnityEngine;

public class LevelView : MonoBehaviour
{
    [SerializeField] GameObject gamePauseGO;
    [SerializeField] GameObject levelWinGO;
    [SerializeField] GameObject gameOverGO;
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
}
