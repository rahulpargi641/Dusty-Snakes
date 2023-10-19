using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverView : MonoBehaviour
{
    [SerializeField] Button m_PlayAgain;
    [SerializeField] string m_SceneToLoad;
    [SerializeField] LevelController m_LevelController;

    private void Awake()
    {
        m_PlayAgain.onClick.AddListener(RestartGame);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(m_SceneToLoad);
    }
}
