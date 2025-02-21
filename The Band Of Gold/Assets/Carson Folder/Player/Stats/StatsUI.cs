using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatsUI : MonoBehaviour
{
    private WeaponInfo weaponInfo;
    public GameObject[] statsSlots;
    public CanvasGroup statsCanvas;
    private PlayerControls playerControls; // Your Input Action Asset
    private InputAction openStatsAction; // The specific action for opening/closing stats
    private int damageAmount;

    private void Awake()
    {
        playerControls = new PlayerControls(); // Initialize your Input Action Asset
        openStatsAction = playerControls.Stats.OpenUI; // Get the "OpenUI" action from the "Stats" action map
        openStatsAction.Enable(); // Enable the action

    }

    private void Start()
    {
        MonoBehaviour currentActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
        if (currentActiveWeapon is IWeapon weapon) // Safer cast
        {
            damageAmount = weapon.GetWeaponInfo().weaponDamage;
        }
        else
        {
            Debug.LogWarning("Current active weapon is not an IWeapon!");
        }
        UpdateAllStats();
    }

    private void Update()
    {
        if (openStatsAction.WasPressedThisFrame())
        {
            // Toggle the canvas alpha
            statsCanvas.alpha = statsCanvas.alpha == 1 ? 0 : 1;

            // Stop or resume time based on the canvas visibility
            if (statsCanvas.alpha == 1)
            {
                UpdateAllStats();
                Time.timeScale = 0f; // Stop time 
            }
            else
            {
                Time.timeScale = 1f; // Resume time
            }
        }
    }

    private void OnEnable()
    {
        openStatsAction?.Enable();
    }

    private void OnDisable()
    {
        openStatsAction?.Disable();
    }


    public void UpdateDamage()
    {
        statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Damage: " + (StatsManager.Instance.damage + damageAmount);
    }

    public void UpdateSpeed()
    {
        statsSlots[1].GetComponentInChildren<TMP_Text>().text = "Speed: " + StatsManager.Instance.speed;
    }

    public void UpdateAllStats()
    {
        UpdateDamage();
        UpdateSpeed();
    }
}