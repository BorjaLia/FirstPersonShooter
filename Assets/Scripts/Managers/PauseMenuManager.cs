using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Input")]
    [SerializeField] private InputActionReference pauseActionReference;

    [Header("Scene Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool isSettingsOpen = false;

    private IGameplayManager gameplayManager;

    private void Start()
    {
        gameplayManager = ServiceLocator.Get<IGameplayManager>();

        ResumeGame();
    }

    private void Update()
    {
        if (pauseActionReference != null && pauseActionReference.action.WasPressedThisFrame())
        {
            if (isSettingsOpen)
            {
                CloseSettings();
            }
            else if (gameplayManager.IsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void OnDestroy()
    {
        ResumeGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;

        gameplayManager.IsPaused = true;

        if (pausePanel) pausePanel.SetActive(true);
        if (settingsPanel) settingsPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isSettingsOpen = false;
        Time.timeScale = 1f;

        gameplayManager.IsPaused = false;

        if (pausePanel) pausePanel.SetActive(false);
        if (settingsPanel) settingsPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RetryLevel()
    {
        ResumeGame();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenSettings()
    {
        isSettingsOpen = true;
        if (pausePanel) pausePanel.SetActive(false);
        if (settingsPanel) settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        isSettingsOpen = false;
        if (settingsPanel) settingsPanel.SetActive(false);
        if (pausePanel) pausePanel.SetActive(true);
    }

    public void ExitToMainMenu()
    {
        ResumeGame();
        SceneManager.LoadScene(mainMenuSceneName);
    }
}