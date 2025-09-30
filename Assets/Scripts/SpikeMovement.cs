using UnityEngine;

public class SpikeMovement : MonoBehaviour
{
    private Transform player;
    private float speed;
    private bool isInitialized = false;
    private GameSpawner spawner;

    public void SetSpawner(GameSpawner gameSpawner)
    {
        spawner = gameSpawner;
    }

    public void Initialize(Transform playerTarget, float moveSpeed)
    {
        player = playerTarget;
        speed = moveSpeed;
        isInitialized = true;
    }

    void Update()
    {
        if (!isInitialized || player == null) return;

        // Move horizontally to the left
        transform.Translate(-speed * Time.deltaTime, 0, 0);

        // Check if spike is 15 units behind the player and return to pool
        if (transform.position.x < player.position.x - 20f)
        {
            ReturnToPool();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Handle player hit by spike
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TakeDamage();
            }
            ReturnToPool();
        }
    }

    void ReturnToPool()
    {
        isInitialized = false;
        if (spawner != null)
        {
            spawner.ReturnSpikeToPool(gameObject);
        }
    }
}
