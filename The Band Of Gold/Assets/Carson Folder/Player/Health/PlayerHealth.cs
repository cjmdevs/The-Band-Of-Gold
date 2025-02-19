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
    // Start is called before the first frame update
    
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        StatsManager.Instance.maxHealth = StatsManager.Instance.currentHealth;   
        UpdateHealthBar(); // Update the health bar initially
    }

    // Update is called once per frame
    void Update()
    {
        // The health bar is updated only when health changes, so no need to do this in Update()
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

        // Clamp health between 0 and maxHealth
        StatsManager.Instance.currentHealth = Mathf.Clamp(StatsManager.Instance.currentHealth, 0, StatsManager.Instance.maxHealth);

        UpdateHealthBar(); // Update the health bar whenever health changes
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
        // Use float division to avoid truncation
        healthBar.fillAmount = Mathf.Clamp((float)StatsManager.Instance.currentHealth / StatsManager.Instance.maxHealth, 0f, 1f);
    }
}
