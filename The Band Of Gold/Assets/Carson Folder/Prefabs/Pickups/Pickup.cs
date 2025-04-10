using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Create a static event system
public static class GameEvents
{
    // Define a delegate for coin collection
    public delegate void CoinCollectionHandler(int amount);
    
    // Create an event that other classes can subscribe to
    public static event CoinCollectionHandler OnCoinCollected;
    
    // Method to trigger the event
    public static void CollectCoins(int amount)
    {
        OnCoinCollected?.Invoke(amount);
    }
}

public class Pickup : MonoBehaviour
{
    private enum PickUpType
    {
        GoldCoin,
        StaminaGlobe,
        HealthGlobe,
    }

    [SerializeField] private PickUpType pickUpType;
    [SerializeField] private float pickUpDistance = 5f;
    [SerializeField] private float accelerationRate = .2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = 1f;
    [SerializeField] private int coinValue = 1; // Default value for coins

    private Vector3 moveDir;
    private Rigidbody2D rb;
    AudioManager audioManager;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start() {
        StartCoroutine(AnimCurveSpawnRoutine());
    }

    private void Update() {
        Vector3 playerPos = PlayerController.Instance.transform.position;

        if (Vector3.Distance(transform.position, playerPos) < pickUpDistance) {
            moveDir = (playerPos - transform.position).normalized;
            moveSpeed += accelerationRate;
        } else {
            moveDir = Vector3.zero;
            moveSpeed = 0;
        }
    }

    private void FixedUpdate() {
        rb.velocity = moveDir * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerController>()) {
            DetectPickupType();
            audioManager.PlaySFX(audioManager.itemPickup);
            Destroy(gameObject);
        }
    }

    private IEnumerator AnimCurveSpawnRoutine() {
        Vector2 startPoint = transform.position;
        float randomX = transform.position.x + Random.Range(-2f, 2f);
        float randomY = transform.position.y + Random.Range(-1f, 1f);

        Vector2 endPoint = new Vector2(randomX, randomY);

        float timePassed = 0f;

        while (timePassed < popDuration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / popDuration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(startPoint, endPoint, linearT) + new Vector2(0f, height);
            yield return null;
        }
    }

    private void DetectPickupType() {
        switch (pickUpType)
        {
            case PickUpType.GoldCoin:
                int randomCoins = Random.Range(1, 6); // Generates a random number between 1 and 5
                // Use the event system instead of direct reference
                GameEvents.CollectCoins(randomCoins);
                Debug.Log("GoldCoin: " + randomCoins);
                break;
            case PickUpType.HealthGlobe:
                PlayerHealth.Instance.HealPlayer();
                Debug.Log("HealthGlobe");
                break;
            case PickUpType.StaminaGlobe:
                Stamina.Instance.RefreshStamina();
                Debug.Log("StaminaGlobe");
                break;
        }
    }
    
    // Method to set the coin value (optional)
    public void SetCoinValue(int value) {
        coinValue = value;
    }
}