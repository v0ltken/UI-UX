using UnityEngine;
using TMPro;

public class distanceTravveled : MonoBehaviour
{
    [Header("Display Settings")]
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private TMP_Text distanceText2;
    [SerializeField] bool isTracking = false;

    [Header("Speed Sync")]
    [SerializeField] private GameSpawner gameSpawner; // Reference to the spawner
    [SerializeField] private bool syncWithSpikes = true; // Toggle sync on/off
    [SerializeField] private float baseSpeed = 8f; // Fallback speed if no spawner found

    private float currentSpeed;
    private float distanceTravelled = 0f;

    void Start()
    {
        // Find GameSpawner if not assigned
        if (gameSpawner == null && syncWithSpikes)
        {
            gameSpawner = Object.FindFirstObjectByType<GameSpawner>();
            if (gameSpawner == null)
            {
                Debug.LogWarning("GameSpawner not found! MeterTracker will use base speed.");
                syncWithSpikes = false;
            }
        }

        currentSpeed = baseSpeed;
    }

    void Update()
    {
        if (!isTracking) return; // Do nothing if tracking is stopped

        // Update speed to match spike speed
        UpdateSpeed();

        // Increase distance based on speed * time
        distanceTravelled += currentSpeed * Time.deltaTime;

        // Display on UI if assigned
        UpdateUI();
    }

    private void UpdateSpeed()
    {
        if (syncWithSpikes && gameSpawner != null)
        {
            // Get current speed from the spawner (same speed as spikes)
            currentSpeed = gameSpawner.GetCurrentSpeed();
        }
        else
        {
            // Use base speed if sync is disabled or spawner not found
            currentSpeed = baseSpeed;
        }
    }

    private void UpdateUI()
    {
        if (distanceText != null && distanceText2 != null)
        {
            string formattedDistance = "<mspace=0.6em>" + distanceTravelled.ToString("F0") + "</mspace>.m";
            distanceText.text = formattedDistance;
            distanceText2.text = formattedDistance;
        }
    }

    // Call this if you want to set speed manually (overrides sync temporarily)
    public void SetSpeed(float newSpeed)
    {
        currentSpeed = newSpeed;
        syncWithSpikes = false; // Disable sync when manually setting speed
    }

    // Re-enable sync with spawner
    public void EnableSpeedSync()
    {
        syncWithSpikes = true;
    }

    // Disable sync and use base speed
    public void DisableSpeedSync()
    {
        syncWithSpikes = false;
        currentSpeed = baseSpeed;
    }

    // Public method to manually set spawner reference
    public void SetGameSpawner(GameSpawner spawner)
    {
        gameSpawner = spawner;
        syncWithSpikes = spawner != null;
    }

    // Getter if you need the distance elsewhere
    public float GetDistance()
    {
        return distanceTravelled;
    }

    // Get current speed (for debugging)
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    // Start/Stop tracking
    public void StartStopTracking()
    {
        isTracking = !isTracking;
    }

    // Start tracking
    public void StartTracking()
    {
        isTracking = true;
    }

    // Stop tracking
    public void StopTracking()
    {
        isTracking = false;
    }

    // Reset distance (optional)
    public void ResetDistance()
    {
        distanceTravelled = 0f;
        UpdateUI();
    }

    // Reset distance and restart tracking
    public void RestartTracking()
    {
        ResetDistance();
        StartTracking();
    }
}
