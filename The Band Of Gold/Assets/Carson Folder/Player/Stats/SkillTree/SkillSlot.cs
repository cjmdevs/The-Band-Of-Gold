using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;

public class SkillSlot : MonoBehaviour
{
    public List<SkillSlot> prerequisiteSkillSlots;
    public SkillSO skillSO;
    public int currentLevel;
    public bool isUnlocked;
    public Button skillButton;
    public Image skillIcon;
    public TMP_Text skillLevelText;

    public static event Action<SkillSlot> OnAblityPointSpent;
    public static event Action<SkillSlot> OnSkillMaxed;
    private void OnValidate()
    {
        if (skillSO != null && skillLevelText != null)
        {
            
            UpdateUI();
        }
    }

    public void TryUpgradeSkill()
    {
        if(isUnlocked && currentLevel < skillSO.maxLevel)
        {
            currentLevel++;
            OnAblityPointSpent?.Invoke(this);

            if(currentLevel >= skillSO.maxLevel)
            {
                OnSkillMaxed?.Invoke(this);
            }
            UpdateUI();

        }
    }

    public bool CanUnlockSkill()
    {

        foreach (SkillSlot slot in prerequisiteSkillSlots)
        {
            if(!slot.isUnlocked || slot.currentLevel < slot.skillSO.maxLevel)
            {
                return false;
            }
        }
        return true;

    }

    public void Unlock()
    {
        isUnlocked = true;
        UpdateUI();
    }

    private void UpdateUI()
    {
        skillIcon.sprite = skillSO.skillIcon;

        if(isUnlocked)
        {
            skillButton.interactable = true;
            skillLevelText.text = currentLevel.ToString() + "/" + skillSO.maxLevel.ToString();
            skillIcon.color = Color.white;
        }
        else
        {
            skillButton.interactable = false;
            skillLevelText.text = "Locked";
            skillIcon.color = Color.grey;
        }
    }
}