using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SkillManager : MonoBehaviour
{
    public GameObject skillCanvas; // Fully enable/disable this object
    private PlayerControls playerControls; // Input Action Asset
    private InputAction openSkillsAction; // Action for opening/closing skills
    private bool isCanvasActive = false; // Track canvas state

    private void Awake()
    {
        playerControls = new PlayerControls(); // Initialize Input Action Asset
        openSkillsAction = playerControls.Stats.OpenSkills; // Get the "OpenSkills" action from "Stats"
        openSkillsAction.Enable(); // Enable the action
    }

    private void Start()
    {
        skillCanvas.SetActive(false); // Ensure it's disabled at the start
    }

    private void Update()
    {
        if (openSkillsAction.WasPressedThisFrame())
        {
            ToggleSkillUI();
        }
    }

    private void ToggleSkillUI()
    {
        isCanvasActive = !isCanvasActive; // Toggle state
        skillCanvas.SetActive(isCanvasActive); // Enable/Disable canvas

        if (isCanvasActive)
        {
            Time.timeScale = 0f; // Pause game
            Debug.Log("Skill UI opened, time stopped.");
        }
        else
        {
            Time.timeScale = 1f; // Resume game
            Debug.Log("Skill UI closed, time resumed.");
        }
    }

    private void OnEnable()
    {
        openSkillsAction?.Enable();
        Debug.Log("openSkillsAction enabled");
        // Subscribe to the event
        SkillSlot.OnAblityPointSpent += HandleAblityPointSpent;
    }

    private void OnDisable()
    {
        openSkillsAction?.Disable();
        Debug.Log("openSkillsAction disabled");
        // Unsubscribe from the event
        SkillSlot.OnAblityPointSpent -= HandleAblityPointSpent;
    }

    private void HandleAblityPointSpent(SkillSlot slot)
    {
        string skillName = slot.skillSO.skillName;

        switch (skillName)
        {
            case "Health Boost":
                StatsManager.Instance.UpdateMaxHealth(1f);
                break;
            case "Health Regen":
                StatsManager.Instance.UpdateHealAmount(.15f);
                break;
            case "Speed":
                StatsManager.Instance.UpdateSpeed(.25f);
                break;
            case "Stamina":
                StatsManager.Instance.UpdateStamina(1);
                break;
            case "Stamina Regen":
                StatsManager.Instance.UpdateStaminaRegen(.5f);
                break;
            case "Damage ":
                StatsManager.Instance.UpdateDamage(.25F);
                break;
            case "Attack Speed":
                StatsManager.Instance.UpdateAttackSpeed(.15f);
                break;
            case "Knockback ":
                StatsManager.Instance.UpdateKnockback(.25f);
                break;
            case "Range":
                StatsManager.Instance.UpdateRange(.50f);
                break;
            default:
                Debug.Log("No skill found with the name: " + skillName);
                break;
        }
    }
}
