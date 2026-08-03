using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject backgroundPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("End Game Panels")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    
    [Header("Input")]
    [SerializeField] private InputActionReference pauseActionReference;

    [Header("Scene Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool isSettingsOpen = false;
    private bool isGameOver = false;

    private IGameplayManager gameplayManager;

    private void Start()
    {
        gameplayManager = ServiceLocator.Get<IGameplayManager>();

        gameplayManager.OnWinConditionMet += HandleWin;
        gameplayManager.OnLoseConditionMet += HandleLose;

        if (winPanel) winPanel.SetActive(false);
        if (losePanel) losePanel.SetActive(false);

        ResumeGame();
    }

    private void Update()
    {
        if (isGameOver) return;

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
        if (gameplayManager != null)
        {
            gameplayManager.OnWinConditionMet -= HandleWin;
            gameplayManager.OnLoseConditionMet -= HandleLose;
        }

        ResetGameState();
    }

    private void HandleWin()
    {
        TriggerEndGameUI(winPanel);
    }

    private void HandleLose()
    {
        TriggerEndGameUI(losePanel);
    }

    private void TriggerEndGameUI(GameObject panelToShow)
    {
        if (isGameOver) return;
        isGameOver = true;

        Time.timeScale = 0f;
        if (gameplayManager != null) gameplayManager.IsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (pausePanel) pausePanel.SetActive(false);
        if (settingsPanel) settingsPanel.SetActive(false);

        if (backgroundPanel) backgroundPanel.SetActive(true);
        if (panelToShow) panelToShow.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        gameplayManager.IsPaused = true;

        if (backgroundPanel) backgroundPanel.SetActive(true);
        if (pausePanel) pausePanel.SetActive(true);
        if (settingsPanel) settingsPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isSettingsOpen = false;

        ResetGameState();

        if (backgroundPanel) backgroundPanel.SetActive(false);
        if (pausePanel) pausePanel.SetActive(false);
        if (settingsPanel) settingsPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ResetGameState()
    {
        Time.timeScale = 1f;
        if (gameplayManager != null)
        {
            gameplayManager.IsPaused = false;
        }
    }

    public void RetryLevel()
    {
        ResetGameState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMainMenu()
    {
        ResetGameState();
        SceneManager.LoadScene(mainMenuSceneName);
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
}