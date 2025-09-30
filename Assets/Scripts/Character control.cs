using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Charactercontrol : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float fallMultiplier = 2.5f;   // Makes falling faster
    [SerializeField] private float lowJumpMultiplier = 2f;  // Makes short hop if button released early
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    private bool isGrounded;
    private int jumpsRemaining;
    private float groundCheckRadius = 0.2f;

    [Header("Ability Boosts")]
    public int extraJumpsAllowed = 0; 
    public float coinPickupRange = 1.5f;

    [Header("Ability Settings")]
    [SerializeField] private float wolfDuration = 2.5f;
    [SerializeField] private float bearDuration = 1.5f;
    [SerializeField] private float birdDuration = 2f;
    [SerializeField] private float cooldownDuration = 5f;

    [Header("Transformation Settings")]
            //[SerializeField] private Sprite normalSprite;
            //[SerializeField] private Sprite wolfSprite;
            //[SerializeField] private Sprite bearSprite;
            //[SerializeField] private Sprite birdSprite;
    [SerializeField] private Animator animator;

    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool isWolf;
    private bool isBear;
    private bool isBird;

    [Header("UI")]

    [SerializeField] private ButtonCD[] abilityButtons;

    private bool isTransformed = false;
    private bool isOnCooldown = false;


    [SerializeField] GameObject player;

    [Header("References")]
    [SerializeField] private GameObject coinPrefab; // Prefab to spawn when spikes are converted
    [SerializeField] private float coinPickupRangeBoost = 3f; // extra range for ability 3
    [SerializeField] private int extraJumps = 2; // ability 3 jump count

    // For spike destruction
    private bool convertSpikesToCoins = false;
    private bool destroySpikes = false;
    private float originalPickupRange;


    void Update()
    {
        // If player is falling, apply stronger gravity
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        if (isGrounded)
            jumpsRemaining = extraJumpsAllowed; // reset when grounded

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            Vector3 screenPos = Pointer.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            worldPos.z = 0;

            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if ((hit.collider != null && hit.collider.gameObject == gameObject) && (isGrounded || jumpsRemaining > 0))
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

                if (!isGrounded)
                    jumpsRemaining--;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    public void changeWolf()
    {
        if (!isTransformed && !isOnCooldown)
        {
            StartCoroutine(TransformRoutine("Wolf", 0, wolfDuration));
        }

    }

    public void changeBear()
    {
        if (!isTransformed && !isOnCooldown)
        {
            StartCoroutine(TransformRoutine("Bear", 1, bearDuration));
        }

    }

    public void changeBird()
    {
        if (!isTransformed && !isOnCooldown)
        {
            StartCoroutine(TransformRoutine("Bird", 2, birdDuration));
        }

    }
    private IEnumerator TransformRoutine(string transformedSprite, int buttonIndex, float transformDuration)
    {
        // Transform player
        isTransformed = true;
            //if (spriteRenderer != null && transformedSprite != null)
            //{
            //    spriteRenderer.sprite = transformedSprite;
            //}
        if (animator != null && transformedSprite != null)
        {
            animator.Play(transformedSprite);
        }
        switch (buttonIndex)
        {
            case 0: // ability 1: turn first spike into coins
                convertSpikesToCoins = true;
                break;
            case 1: // ability 2: destroy spikes while transformed
                destroySpikes = true;
                break;
            case 2: // ability 3: coin pickup + multi-jump
                extraJumpsAllowed = extraJumps;
                coinPickupRange += coinPickupRangeBoost;
                break;
        }

        if (abilityButtons[buttonIndex] != null)
            abilityButtons[buttonIndex].StartTimer(transformDuration);

        for (int i = 0; i < abilityButtons.Length; i++)
        {
            if (i != buttonIndex && abilityButtons[i] != null)
                abilityButtons[i].StartTimer(transformDuration); // They show same duration as lockout
        }

        // Wait for duration
        yield return new WaitForSeconds(transformDuration);

        // Revert transformation
        isTransformed = false;
            //if (spriteRenderer != null && normalSprite != null)
            //{
            //    spriteRenderer.sprite = normalSprite;
            //}
        if (animator != null && transformedSprite != null)
        {
            animator.Play("Run");
        }

        // Start cooldown
        destroySpikes = false;
        convertSpikesToCoins = false;
        isOnCooldown = true;

        foreach (var button in abilityButtons)
        {
            if (button != null) button.StartTimer(cooldownDuration);
        }
            extraJumpsAllowed = 0;
            coinPickupRange = originalPickupRange;

        yield return new WaitForSeconds(cooldownDuration);
        isOnCooldown = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Spike"))
        {
            if (convertSpikesToCoins && coinPrefab != null)
            {
                Vector3 pos = col.transform.position;
                Destroy(col.gameObject);
                Instantiate(coinPrefab, pos, Quaternion.identity);
            }
            else if (destroySpikes)
            {
                Destroy(col.gameObject);
            }
        }
    }

}

