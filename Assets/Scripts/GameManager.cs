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

    public static GameManager Instance;

    [SerializeField] GameObject objectSpawner;
    [SerializeField] Grasscycle background;
    [SerializeField] Grasscycle grid;
    [SerializeField] distanceTravveled timer;
    [SerializeField] Animator playerAnim;

    public 
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
    }
    public void TakeDamage()
    {
        health--;
        Debug.Log("Player hit! Health: " + health);

        if (health <= 0)
        {
            Debug.Log("Game Over!");
            // Handle game over logic here
        }
    }

    public void CollectCoin()
    {
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

}
