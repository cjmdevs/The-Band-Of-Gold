using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Added for TextMeshPro support

public class EnemyHealth : MonoBehaviour
{   
    [SerializeField] public float startingHealth = 3; // health of the enemy
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] FloatingHealthBar healthBar;
    public GameObject crystalBallPrefab;  // Assign this in the inspector
    public TextMeshProUGUI bossDefeatText;  // Assign this TMP text in the inspector
    public float textDisplayTime = 3f;  // How long the boss defeat text stays visible

    private Knockback knockback;
    private Flash flash;
    public float currentHealth;
    AudioManager audioManager;

    private void Awake() {
        flash = GetComponent<Flash>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        knockback = GetComponent<Knockback>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    
    private void Start() {
        currentHealth = startingHealth;
        healthBar.UpdateHealthBar(currentHealth, startingHealth);
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth, startingHealth);
        audioManager.PlaySFX(audioManager.enemyHit);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine() {
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        DetectDeath();
    }
    
    private IEnumerator DisableTextAfterDelay(GameObject text) {
        yield return new WaitForSeconds(textDisplayTime);
        text.SetActive(false);
    }

    public void DetectDeath() {
        if (currentHealth <= 0) {
            audioManager.PlaySFX(audioManager.enemyDeath);
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            
            // Check if this enemy has the Boss tag
            if (gameObject.CompareTag("Boss")) {
                // Display boss defeat text if assigned
                if (bossDefeatText != null) {
                    bossDefeatText.gameObject.SetActive(true);
                    
                    // Disable the text after the display time
                    StartCoroutine(DisableTextAfterDelay(bossDefeatText.gameObject));
                }
                
                // Spawn crystal ball that player can pick up
                Instantiate(crystalBallPrefab, transform.position, Quaternion.identity);
            }
            
            GetComponent<enemyDrops>().DropItems();
            Destroy(gameObject);
        }
    }
}