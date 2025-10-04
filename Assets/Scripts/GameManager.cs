using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player Stats")]
    public int health = 3;
    public int coins = 0;

    [Header("UI References")]
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text coinText2; // Optional second coin display
    [SerializeField] private GameObject gameOverScreen; // Game Over UI Panel
    [SerializeField] private TMP_Text finalDistanceText;
    [SerializeField] private TMP_Text finalDistanceText2;// Shows final distance on game over
    [SerializeField] private TMP_Text finalCoinsText; // Shows final coins on game over

    [Header("Game References")]
    [SerializeField] private GameObject player; // Reference to player GameObject
    [SerializeField] private GameObject objectSpawner;
    [SerializeField] private Grasscycle background;
    [SerializeField] private Grasscycle grid;
    [SerializeField] private distanceTravveled timer;
    [SerializeField] private Animator playerAnim;

    public static GameManager Instance;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(startGame());
        UpdateCoinUI();

        // Make sure game over screen is hidden at start
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
    }

    public void TakeDamage()
    {
        if (isGameOver) return; // Prevent multiple death triggers

        health--;
        Debug.Log("Player hit! Health: " + health);

        if (health <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        if (isGameOver) return; // Prevent multiple calls
        isGameOver = true;

        Debug.Log("Game Over!");

        // Stop all game elements
        StopAllGameplay();

        // Destroy the player
        if (player != null)
        {
            player.SetActive(false );
        }

        // Show game over screen with final stats
        ShowGameOverScreen();
    }

    private void StopAllGameplay()
    {
        // Stop timer
        if (timer != null)
            timer.StopTracking();

        // Stop animations
        if (playerAnim != null)
            playerAnim.enabled = false;

        // Disable spawner
        if (objectSpawner != null)
            objectSpawner.SetActive(false);

        // Stop background animations
        if (background != null)
            background.activate = false;

        if (grid != null)
            grid.activate = false;
    }

    private void ShowGameOverScreen()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);

            // Display final distance on game over screen
            if (finalDistanceText != null && timer != null)
            {
                float finalDistance = timer.GetDistance();
                finalDistanceText.text = finalDistance.ToString("F0") + "m";
                finalDistanceText2.text = finalDistance.ToString("F0") + "m";
            }

            // Display final coins on game over screen
            if (finalCoinsText != null)
            {
                finalCoinsText.text = coins.ToString();
            }
        }
    }

    public void CollectCoin()
    {
        if (isGameOver) return;

        coins++;
        Debug.Log("Coin collected! Total coins: " + coins);
        UpdateCoinUI();
    }

    public void SetCoinText(TMP_Text text1, TMP_Text text2 = null)
    {
        coinText = text1;
        coinText2 = text2;
        UpdateCoinUI();
    }

    public void SetCoins(int newCoinAmount)
    {
        coins = newCoinAmount;
        UpdateCoinUI();
    }

    public int GetCoins()
    {
        return coins;
    }

    public IEnumerator startGame()
    {
        yield return new WaitForSeconds(3.5f);
        objectSpawner.SetActive(true);
        background.activate = true;
        grid.activate = true;
        timer.StartTracking();
        playerAnim.Play("Run");
    }

    private void UpdateCoinUI()
    {
        if (coinText != null && coinText2 != null)
        {
            string newCoinsText = "" + coins.ToString("F0");
            coinText.text = newCoinsText;
            coinText2.text = newCoinsText;
        }
    }

    // Optional: Restart game method
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    // Optional: Quit to main menu
    public void QuitToMenu()
    {
        // Load your main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}