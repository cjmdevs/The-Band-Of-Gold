using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;
    private MonoBehaviour currentActiveWeapon;

    public delegate void OnMaxHealthChanged();
    public static event OnMaxHealthChanged MaxHealthChanged;

    [Header("Combat Stats")]
    public float weaponRange;
    public float knockbackForce;
    public float knockbackTime;
    public float stunTime;
    public int damage;
    public float attackCooldown;

    [Header("Movement Stats")]
    public float speed;
    public float staminaRegen;
    public float stamina;

    [Header("Health Stats")]
    public float maxHealth;
    public float currentHealth;
    public float passiveHealAmount;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth; // Fully heal the player
    }
}
