using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;
    private MonoBehaviour currentActiveWeapon;

  

    [Header("Combat Stats")]
    public float weaponRange;
    public float knockbackForce;
    public float knockbackTime;
    public float stunTime;
    public float damage;
    public float attackCooldown;

    [Header("Movement Stats")]
    public float speed;
    public float staminaRegen;
    public int maxStamina;

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
    public void UpdateHealAmount(float amount)
    {
        passiveHealAmount += amount;
    }
    public void UpdateSpeed(float amount)
    {
        speed += amount;
    }
    public void UpdateDamage(float amount)
    {
        damage += amount;
    }
    public void UpdateKnockback(float amount)
    {
        knockbackForce += amount;
    }
    public void UpdateAttackSpeed(float amount)
    {
        attackCooldown += amount;
    }
    public void UpdateRange(float amount)
    {
        weaponRange += amount;
    }

    public void UpdateStamina(int amount)
    {
        Stamina.Instance.IncreaseMaxStamina();
    }
    public void UpdateStaminaRegen(float amount)
    {
        staminaRegen -= amount;
    }
}
