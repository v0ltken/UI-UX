using UnityEngine;
using UnityEngine.UIElements;

public class CoinMovement : MonoBehaviour
{
    private Transform player;
    private float speed;
    private bool isInitialized = false;

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

        // Check if coin is 15 units behind the player and destroy it
        if (transform.position.x < player.position.x - 15f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if player is within pickup range
            Charactercontrol characterControl = other.GetComponent<Charactercontrol>();
            float pickupRange = characterControl != null ? characterControl.coinPickupRange : 1.5f;

            float distance = Vector2.Distance(transform.position, other.transform.position);
            if (distance <= pickupRange)
            {
                // Handle coin collection
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.CollectCoin();
                }

                // Optional: Add coin collection effect/sound here
                Destroy(gameObject);
            }
        }
    }
}
