using UnityEngine;
using static UnityEngine.Rigidbody2D;
using System.Collections.Generic;

public class GameSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject spikePrefab;
    public GameObject coinPrefab;

    [Header("Player Reference")]
    public Transform player;

    [Header("Spawn Settings")]
    public float initialSpikeSpawnRate = 2f; // Initial time between spike spawns
    public float initialCoinSpawnRate = 3f;  // Initial time between coin spawns
    public float minSpikeSpawnRate = 0.3f;   // Minimum time between spike spawns (cap)
    public float minCoinSpawnRate = 0.8f;    // Minimum time between coin spawns (cap)
    public float spawnRateDecreaseSpeed = 0.1f; // How fast spawn rates increase over time
    public float spawnDistance = 15f; // Distance from player to spawn objects

    [Header("Movement Settings")]
    public float initialSpeed = 5f;
    public float maxSpeed = 15f;
    public float speedIncreaseRate = 0.5f; // Speed increase per second

    [Header("Pool Settings")]
    public int spikePoolSize = 20; // Number of spikes to pre-instantiate

    [Header("Spawn Area")]
    public float spawnAreaWidth = 10f; // Width of spawn area
    public float spawnAreaHeight = 8f;  // Height of spawn area

    [SerializeField] private float currentSpeed;
    [SerializeField] private float currentSpikeSpawnRate;
    private float currentCoinSpawnRate;
    [SerializeField] private float spikeSpawnTimer;
    private float coinSpawnTimer;
    private float gameTime;

    // Object Pool for spikes
    private Queue<GameObject> spikePool = new Queue<GameObject>();
    private List<GameObject> activeSpikesList = new List<GameObject>();

    void Start()
    {
        currentSpeed = initialSpeed;
        currentSpikeSpawnRate = initialSpikeSpawnRate;
        currentCoinSpawnRate = initialCoinSpawnRate;

        // Initialize spike pool
        InitializeSpikePool();

        // Find player if not assigned
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null)
            {
                Debug.LogWarning("Player not found! Please assign player transform or add 'Player' tag to player GameObject.");
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        // Update game time and speed
        gameTime += Time.deltaTime;
        currentSpeed = Mathf.Min(initialSpeed + (speedIncreaseRate * gameTime), maxSpeed);

        // Update spawn rates (decrease time = increase frequency)
        currentSpikeSpawnRate = Mathf.Max(initialSpikeSpawnRate - (spawnRateDecreaseSpeed * gameTime), minSpikeSpawnRate);
        currentCoinSpawnRate = Mathf.Max(initialCoinSpawnRate - (spawnRateDecreaseSpeed * gameTime), minCoinSpawnRate);

        // Handle spike spawning with pooling
        spikeSpawnTimer += Time.deltaTime;
        if (spikeSpawnTimer >= currentSpikeSpawnRate)
        {
            SpawnSpikeFromPool();
            spikeSpawnTimer = 0f;
        }

        // Handle coin spawning
        coinSpawnTimer += Time.deltaTime;
        if (coinSpawnTimer >= currentCoinSpawnRate)
        {
            SpawnCoin();
            coinSpawnTimer = 0f;
        }
    }

    void InitializeSpikePool()
    {
        if (spikePrefab == null)
        {
            Debug.LogWarning("Spike prefab not assigned! Pool initialization failed.");
            return;
        }

        // Create parent object for organization
        GameObject poolParent = new GameObject("Spike Pool");
        poolParent.transform.SetParent(transform);

        // Pre-instantiate spikes and add to pool
        for (int i = 0; i < spikePoolSize; i++)
        {
            GameObject spike = Instantiate(spikePrefab, poolParent.transform);
            spike.SetActive(false);

            // Ensure spike has the movement component
            SpikeMovement spikeMovement = spike.GetComponent<SpikeMovement>();
            if (spikeMovement == null)
            {
                spikeMovement = spike.AddComponent<SpikeMovement>();
            }
            spikeMovement.SetSpawner(this);

            spikePool.Enqueue(spike);
        }
    }

    void SpawnSpikeFromPool()
    {
        if (spikePool.Count == 0)
        {
            Debug.LogWarning("Spike pool is empty! Consider increasing pool size.");
            return;
        }

        GameObject spike = spikePool.Dequeue();
        Vector3 spawnPosition = GetSpikeSpawnPosition();

        spike.transform.position = spawnPosition;
        spike.SetActive(true);

        SpikeMovement spikeMovement = spike.GetComponent<SpikeMovement>();
        spikeMovement.Initialize(player, currentSpeed);

        activeSpikesList.Add(spike);
    }

    public void ReturnSpikeToPool(GameObject spike)
    {
        spike.SetActive(false);
        activeSpikesList.Remove(spike);
        spikePool.Enqueue(spike);
    }

    void SpawnCoin()
    {
        if (coinPrefab == null) return;

        Vector3 spawnPosition = GetCoinSpawnPosition();
        GameObject coin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);

        // Add movement component to coin
        CoinMovement coinMovement = coin.GetComponent<CoinMovement>();
        if (coinMovement == null)
        {
            coinMovement = coin.AddComponent<CoinMovement>();
        }
        coinMovement.Initialize(player, currentSpeed); // Coins move slightly slower
    }

    Vector3 GetSpikeSpawnPosition()
    {
        // Spikes spawn at +10 units to the right of the player, at y -2.2 and z -1
        Vector3 spawnPos = new Vector3(player.position.x + spawnDistance, -2.2f, -1f);
        return spawnPos;
    }

    Vector3 GetCoinSpawnPosition()
    {
        // Coins spawn at +10 units to the right of the player, between y -2 to 0, at z -1
        Vector3 spawnPos = new Vector3(player.position.x + 10f, Random.Range(-2f, 0f), -1f);
        return spawnPos;
    }

    // Get current speed for UI display or other systems
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    // Get game time for scoring systems
    public float GetGameTime()
    {
        return gameTime;
    }

    // Get current spawn rates for UI display or other systems
    public float GetCurrentSpikeSpawnRate()
    {
        return currentSpikeSpawnRate;
    }

    public float GetCurrentCoinSpawnRate()
    {
        return currentCoinSpawnRate;
    }
}
