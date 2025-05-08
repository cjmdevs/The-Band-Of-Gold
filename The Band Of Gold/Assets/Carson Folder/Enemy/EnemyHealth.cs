using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{   
    [SerializeField] public float startingHealth = 3; // health of the enemy
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] FloatingHealthBar healthBar;
    public GameObject crystalBallPrefab;  // Assign this in the inspector
    public GameObject bossDefeatTextPrefab;  // Assign this in the inspector


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

    public void DetectDeath() {
        if (currentHealth <= 0) {
            audioManager.PlaySFX(audioManager.enemyDeath);
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            
            // Check if this enemy is on the boss layer
            if (gameObject.layer == LayerMask.NameToLayer("Boss")) {
                // Display boss defeat text
                GameObject defeatText = Instantiate(bossDefeatTextPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
                
                // Spawn crystal ball that player can pick up
                Instantiate(crystalBallPrefab, transform.position, Quaternion.identity);
            }
            
            GetComponent<enemyDrops>().DropItems();
            Destroy(gameObject);
        }
    }

}
