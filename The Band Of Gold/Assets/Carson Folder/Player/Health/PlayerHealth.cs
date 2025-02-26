using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public Image healthBar;
    private bool isDead;
    public GameManagerScript gameManager;

    AudioManager audioManager;
    
    [SerializeField] private float healInterval = 10f; // Time in seconds before healing

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        StatsManager.Instance.maxHealth = StatsManager.Instance.currentHealth;   
        UpdateHealthBar();

        // Start passive healing coroutine
        StartCoroutine(PassiveHeal());
    }

    public void HealPlayer() {
        if (StatsManager.Instance.currentHealth < StatsManager.Instance.maxHealth) {
            StatsManager.Instance.currentHealth += 1;
            UpdateHealthBar();
        }
    }

    public void ChangeHealth(int amount)
    {
        StatsManager.Instance.currentHealth += amount;
        StatsManager.Instance.currentHealth = Mathf.Clamp(StatsManager.Instance.currentHealth, 0, StatsManager.Instance.maxHealth);

        UpdateHealthBar();
        ShakeManager.Instance.ShakeScreen();

        if (StatsManager.Instance.currentHealth <= 0 && !isDead)
        {
            isDead = true; 
            audioManager.PlaySFX(audioManager.playerDeath);
            gameObject.SetActive(false);
            gameManager.GameOver();
            Debug.Log("Player Died");
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = Mathf.Clamp((float)StatsManager.Instance.currentHealth / StatsManager.Instance.maxHealth, 0f, 1f);
    }

    private IEnumerator PassiveHeal()
    {
        while (true)
        {
            yield return new WaitForSeconds(healInterval);

            if (!isDead && StatsManager.Instance.currentHealth < StatsManager.Instance.maxHealth)
            {
                StatsManager.Instance.currentHealth = Mathf.Clamp(StatsManager.Instance.currentHealth + StatsManager.Instance.passiveHealAmount, 0, StatsManager.Instance.maxHealth);
                UpdateHealthBar();
                Debug.Log("Passive healing applied");
            }
        }
    }
}
