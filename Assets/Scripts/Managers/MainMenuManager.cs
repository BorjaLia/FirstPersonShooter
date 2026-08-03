using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Level Data")]
    [SerializeField] private string gameplaySceneName = "Level";
    [SerializeField] private LevelConfig levelConfig;

    private void Start()
    {
        ShowPanel(mainPanel);

        ServiceLocator.Get<AudioManager>().PlayMenuMusic();
    }

    public void PlayGame()
    {
        IGameplayManager gameplayManager = ServiceLocator.Get<IGameplayManager>();

        if (levelConfig != null)
        {
            gameplayManager.SetConfig(levelConfig);
        }
        else
        {
            Debug.LogError("No Level Config");
        }

        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OpenSettings() => ShowPanel(settingsPanel);
    public void OpenCredits() => Application.OpenURL("https://github.com/BorjaLia/FirstPersonShooter/blob/main/README.md");
    public void BackToMain() => ShowPanel(mainPanel);

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void ShowPanel(GameObject panelToShow)
    {
        if (mainPanel) mainPanel.SetActive(panelToShow == mainPanel);
        if (settingsPanel) settingsPanel.SetActive(panelToShow == settingsPanel);
    }
}