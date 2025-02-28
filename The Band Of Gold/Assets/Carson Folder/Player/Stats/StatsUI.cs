using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatsUI : MonoBehaviour
{
private WeaponInfo weaponInfo;
    public GameObject[] statsSlots;
    public GameObject statsCanvas; // Fully enable/disable this object
    private PlayerControls playerControls; // Input Action Asset
    private InputAction openStatsAction; // Action for opening/closing stats
    private float damageAmount;
    private float weaponRange;
    private float weaponCooldown;
    private bool isCanvasActive = false; // Track canvas state

    private void Awake()
    {
        playerControls = new PlayerControls(); // Initialize Input Action Asset
        openStatsAction = playerControls.Stats.OpenUI; // Get the "OpenUI" action from "Stats"
        openStatsAction.Enable(); // Enable the action
    }

    private void Start()
    {
        statsCanvas.SetActive(false); // Ensure it's disabled at the start
    }

    private void Update()
    {
        if (openStatsAction.WasPressedThisFrame())
        {
            ToggleStatsUI();
        }
    }

    private void ToggleStatsUI()
    {
        isCanvasActive = !isCanvasActive; // Toggle state
        statsCanvas.SetActive(isCanvasActive); // Enable/Disable canvas

        if (isCanvasActive)
        {
            UpdateAllStats(); // Update stats when enabling
            Time.timeScale = 0f; // Pause game
            Debug.Log("Stats UI opened, time stopped.");
        }
        else
        {
            Time.timeScale = 1f; // Resume game
            Debug.Log("Stats UI closed, time resumed.");
        }
    }

    public void UpdateAllStats()
    {
        MonoBehaviour currentActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
        if (currentActiveWeapon is IWeapon weapon)
        {
            damageAmount = weapon.GetWeaponInfo().weaponDamage;
            weaponRange = weapon.GetWeaponInfo().weaponRange;
            weaponCooldown = weapon.GetWeaponInfo().weaponCooldown;
        }
        else
        {
            Debug.LogWarning("Current active weapon is not an IWeapon!");
        }

        UpdateDamage();
        UpdateWeaponRange();
        UpdateKnockbackForce();
        UpdateKnockbackTime();
        UpdateStunTime();
        UpdateAttackCooldown();
        UpdateSpeed();
        UpdateStaminaRegen();
        UpdateMaxHealth();
        UpdateCurrentHealth();
    }
    private void OnEnable()
    {
        openStatsAction?.Enable();
        Debug.Log("openStatsAction enabled");
    }

    private void OnDisable()
    {
        openStatsAction?.Disable();
        Debug.Log("openStatsAction disabled");
    }

    public void UpdateDamage()
    {
        statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Damage: " + (StatsManager.Instance.damage + damageAmount);
    }

    public void UpdateWeaponRange()
    {
        statsSlots[1].GetComponentInChildren<TMP_Text>().text = "Weapon Range: " + (StatsManager.Instance.weaponRange + weaponRange);
    }

    public void UpdateKnockbackForce()
    {
        statsSlots[2].GetComponentInChildren<TMP_Text>().text = "Knockback Force: " + StatsManager.Instance.knockbackForce;
    }

    public void UpdateKnockbackTime()
    {
        statsSlots[3].GetComponentInChildren<TMP_Text>().text = "Knockback Time: " + StatsManager.Instance.knockbackTime;
    }

    public void UpdateStunTime()
    {
        statsSlots[4].GetComponentInChildren<TMP_Text>().text = "Stun Time: " + StatsManager.Instance.stunTime;
    }

    public void UpdateAttackCooldown()
    {
        statsSlots[5].GetComponentInChildren<TMP_Text>().text = "Cooldown: " + (weaponCooldown- StatsManager.Instance.attackCooldown);
    }

    public void UpdateSpeed()
    {
        statsSlots[6].GetComponentInChildren<TMP_Text>().text = "Speed: " + StatsManager.Instance.speed;
    }

    public void UpdateStaminaRegen()
    {
        statsSlots[7].GetComponentInChildren<TMP_Text>().text = "Stamina Regen: " + StatsManager.Instance.staminaRegen;
    }

    public void UpdateMaxHealth()
    {
        statsSlots[8].GetComponentInChildren<TMP_Text>().text = "Passive Heal: " + StatsManager.Instance.passiveHealAmount;
    }
    public void UpdateCurrentHealth()
    {
        statsSlots[9].GetComponentInChildren<TMP_Text>().text = "Max Health: " + StatsManager.Instance.maxHealth;
    }


    
}
