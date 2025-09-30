
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGameManager : MonoBehaviour 
{
    [Header("Pause UI")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Button gameOverRestartButton;
    [SerializeField] private Button gameOverQuitButton;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text finalCoinsText;

    [Header("Settings UI")]
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private Button settingsBackButton;
    [SerializeField] private Button restartFromSettingsButton;

    [Header("References")]
    [SerializeField] private GameSpawner gameSpawner;
    [SerializeField] private distanceTravveled meterTracker;
    [SerializeField] private Grasscycle grasscycle;
    [SerializeField] private Charactercontrol playerController;

    [Header("Audio (Optional)")]
    [SerializeField] private AudioSource backgroundMusic;

    private bool isPaused = false;
    private bool isGameOver = false;

    public static PauseGameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Find references if not assigned
        FindReferences();

        // Setup button listeners
        SetupButtonListeners();

        // Initialize UI state
        InitializeUI();
    }

    void Update()
    {
        // Check for pause input (ESC key or mobile back button)

    }

    void FindReferences()
    {
        if (gameSpawner == null)
            gameSpawner = Object.FindFirstObjectByType<GameSpawner>();

        if (meterTracker == null)
            meterTracker = Object.FindFirstObjectByType<distanceTravveled>();

        if (grasscycle == null)
            grasscycle = Object.FindFirstObjectByType<Grasscycle>();

        if (playerController == null)
            playerController = Object.FindFirstObjectByType<Charactercontrol>();
    }

    void SetupButtonListeners()
    {
        if (pauseButton != null)
            pauseButton.onClick.AddListener(PauseGame);

        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);

        if (settingsButton != null)
            settingsButton.onClick.AddListener(OpenSettings);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);

        if (gameOverRestartButton != null)
            gameOverRestartButton.onClick.AddListener(RestartGame);

        if (gameOverQuitButton != null)
            gameOverQuitButton.onClick.AddListener(QuitGame);

        if (settingsBackButton != null)
            settingsBackButton.onClick.AddListener(CloseSettings);

        if (restartFromSettingsButton != null)
            restartFromSettingsButton.onClick.AddListener(RestartFromSettings);
    }

    void InitializeUI()
    {
        // Hide pause, game over, and settings UI at start
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        if (settingsUI != null)
            settingsUI.SetActive(false);

        // Ensure game is running
        Time.timeScale = 1f;
        isPaused = false;
        isGameOver = false;
    }

    public void PauseGame()
    {
        if (isGameOver) return;

        isPaused = true;
        Time.timeScale = 0f;

        // Show pause menu
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        // Pause background music
        if (backgroundMusic != null && backgroundMusic.isPlaying)
            backgroundMusic.Pause();

        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        if (isGameOver) return;

        isPaused = false;
        Time.timeScale = 1f;

        // Hide pause menu
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        // Resume background music
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
            backgroundMusic.UnPause();

        Debug.Log("Game Resumed");
    }

    public void OpenSettings()
    {
        if (isGameOver) return;

        // Hide pause menu and show settings
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        if (settingsUI != null)
            settingsUI.SetActive(true);

        // Game remains paused while in settings
        Debug.Log("Settings Opened");
    }

    public void CloseSettings()
    {
        if (isGameOver) return;

        // Hide settings and show pause menu
        if (settingsUI != null)
            settingsUI.SetActive(false);

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        Debug.Log("Settings Closed");
    }

    public void RestartFromSettings()
    {
        // Close settings first
        if (settingsUI != null)
            settingsUI.SetActive(false);

        // Then restart the game
        RestartGame();
    }

    public void RestartGame()
    {
        // Reset time scale
        Time.timeScale = 1f;
        isPaused = false;
        isGameOver = false;

        // Hide all UI menus
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
        if (settingsUI != null)
            settingsUI.SetActive(false);

        // Reset game stats
        if (GameManager.Instance != null)


        // Reset meter tracker
        if (meterTracker != null)
            meterTracker.RestartTracking();

        // Restart background music
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
            backgroundMusic.Play();
        }

        // Clear all spawned objects
        ClearSpawnedObjects();

        Debug.Log("Game Restarted");
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        isPaused = false;
        Time.timeScale = 0f;

        // Update final score display
        UpdateGameOverUI();

        // Show game over UI
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        // Stop background music
        if (backgroundMusic != null && backgroundMusic.isPlaying)
            backgroundMusic.Stop();

        Debug.Log("Game Over!");
    }

    void UpdateGameOverUI()
    {
        if (GameManager.Instance != null)
        {
            if (finalCoinsText != null)
                finalCoinsText.text = "Coins: " + GameManager.Instance.GetCoins().ToString();
        }

        if (meterTracker != null && finalScoreText != null)
        {
            finalScoreText.text = "Distance: " + meterTracker.GetDistance().ToString("F0") + "m";
        }
    }

    void ClearSpawnedObjects()
    {
        // Clear spikes
        GameObject[] spikes = GameObject.FindGameObjectsWithTag("Spike");
        foreach (GameObject spike in spikes)
        {
            Destroy(spike);
        }

        // Clear coins
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        foreach (GameObject coin in coins)
        {
            Destroy(coin);
        }
    }

    // Public methods for external access
    public bool IsPaused()
    {
        return isPaused;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void SetBackgroundMusic(AudioSource music)
    {
        backgroundMusic = music;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");

        isPaused = false;
        Time.timeScale = 1f;

        SceneManager.LoadScene("Menu");
    }

    // Call this from GameManager when health reaches 0
    public void TriggerGameOver()
    {
        GameOver();
    }

    // Check if settings menu is currently open
    public bool IsSettingsOpen()
    {
        return settingsUI != null && settingsUI.activeInHierarchy;
    }
}
