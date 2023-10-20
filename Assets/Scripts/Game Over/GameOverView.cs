using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverView : MonoBehaviour
{
    [SerializeField] Button m_PlayAgain;
    [SerializeField] Button m_QuitButton;

    private void Awake()
    {
        m_PlayAgain.onClick.AddListener(RestartGame);
        m_QuitButton.onClick.AddListener(QuitGame);
    }

    private void RestartGame()
    {
        AudioService.Instance.PlaySound(SoundType.ButtonClick);
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    private void QuitGame()
    {
        AudioService.Instance.PlaySound(SoundType.ButtonClick);
        Application.Quit();
    }
}
