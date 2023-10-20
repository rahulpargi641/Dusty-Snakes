using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] Button m_StartButton;
    [SerializeField] Button m_QuitButton;
    [SerializeField] Button m_InfoButton;
    [SerializeField] Button m_BackButton;
    [SerializeField] GameObject MainMenuScreen;
    [SerializeField] GameObject InfoScreen;
    [SerializeField] string m_SceneToLoad;
    private void Awake()
    {
        m_StartButton.onClick.AddListener(StartGame);
        m_QuitButton.onClick.AddListener(QuitGame);
        m_BackButton.onClick.AddListener(GoToMainMenuScreen);
        m_InfoButton.onClick.AddListener(GoToInfoScreen);
    }

    private void StartGame()
    {
        AudioService.Instance.PlaySound(SoundType.ButtonClick);
        AudioService.Instance.PlaySound(SoundType.BgMusic);
        SceneManager.LoadScene(m_SceneToLoad);
    }

    private void QuitGame()
    {
        AudioService.Instance.PlaySound(SoundType.ButtonClick);
        Application.Quit();
    }

    private void GoToInfoScreen()
    {
        AudioService.Instance.PlaySound(SoundType.ButtonClick);
        MainMenuScreen.SetActive(false);
        InfoScreen.SetActive(true);
    }
    private void GoToMainMenuScreen()
    {
        AudioService.Instance.PlaySound(SoundType.ButtonClick);
        InfoScreen.SetActive(false);
        MainMenuScreen.SetActive(true);
    }
}
