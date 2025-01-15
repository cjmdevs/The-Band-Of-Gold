using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public Image healthBar;
    private bool isDead;
    public GameManagerScript gameManager;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;   
        UpdateHealthBar(); // Update the health bar initially
    }

    // Update is called once per frame
    void Update()
    {
        // The health bar is updated only when health changes, so no need to do this in Update()
    }

    public void ChangeHealth(int amount)
    {
        health += amount;

        // Clamp health between 0 and maxHealth
        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateHealthBar(); // Update the health bar whenever health changes
        Debug.Log(health);

        if (health <= 0 && !isDead)
        {
            isDead = true;
            gameManager.gameOver();
            Debug.Log("Player Died");
        }
    }

    private void UpdateHealthBar()
    {
        // Use float division to avoid truncation
        healthBar.fillAmount = Mathf.Clamp((float)health / maxHealth, 0f, 1f);
    }
}
