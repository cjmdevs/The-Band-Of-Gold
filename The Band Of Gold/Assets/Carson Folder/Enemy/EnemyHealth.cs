using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{   
    [SerializeField] public float startingHealth = 3; // health of the enemy
    [SerializeField] private GameObject deathVFXPrefab;

    private Knockback knockback;
    private Flash flash;
    public float currentHealth;
    AudioManager audioManager;

    private void Awake() {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void Start() {
        currentHealth = startingHealth;
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
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
            GetComponent<PickUpSpawner>().DropItems();
            Destroy(gameObject);

            
        }
    }
}
