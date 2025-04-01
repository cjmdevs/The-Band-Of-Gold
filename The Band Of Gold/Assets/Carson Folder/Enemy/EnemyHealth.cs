using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{   
    [SerializeField] public float startingHealth = 3; // health of the enemy
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] FloatingHealthBar healthBar;

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
            GetComponent<enemyDrops>().DropItems();
            Destroy(gameObject);

            
        }
    }
}
