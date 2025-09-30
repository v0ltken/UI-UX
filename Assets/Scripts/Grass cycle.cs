using UnityEngine;

public class Grasscycle : MonoBehaviour
{
    [SerializeField] private GameObject[] objects; 
    [SerializeField] private float baseSpeed = 2f;     
    [SerializeField] private float resetX = -10f;  
    [SerializeField] private float startX = 10f;
    [SerializeField] public bool activate = false;

    [Header("Speed Sync")]
    [SerializeField] private GameSpawner gameSpawner; // Reference to the spawner
    [SerializeField] private bool syncWithSpikes = true; // Toggle sync on/off

    private float currentSpeed;

    void Start()
    {
        // Find GameSpawner if not assigned
        if (gameSpawner == null && syncWithSpikes)
        {
            gameSpawner = Object.FindFirstObjectByType<GameSpawner>();
            if (gameSpawner == null)
            {
                Debug.LogWarning("GameSpawner not found! Grasscycle will use base speed.");
                syncWithSpikes = false;
            }
        }

        currentSpeed = baseSpeed;
    }

    void Update()
    {
        if (activate)
        {
            // Update speed to match spike speed
            UpdateSpeed();

            // Move all grass objects
            foreach (GameObject obj in objects)
            {
                obj.transform.Translate(Vector2.left * currentSpeed * Time.deltaTime);
                if (obj.transform.position.x <= resetX)
                {
                    obj.transform.position = new Vector3(startX, obj.transform.position.y, obj.transform.position.z);
                }
            }
        }
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

    // Public method to manually set spawner reference
    public void SetGameSpawner(GameSpawner spawner)
    {
        gameSpawner = spawner;
        syncWithSpikes = spawner != null;
    }

    // Get current movement speed (for debugging)
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}
