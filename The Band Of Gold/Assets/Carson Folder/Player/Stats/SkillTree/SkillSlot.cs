using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSlot : MonoBehaviour
{
    public SkillSO skillSO;
    public int currentLevel;
    public bool isUnlocked;
    public Button skillButton;
    public Image skillIcon;
    public TMP_Text skillLevelText;

    private void OnValidate()
    {
        if (skillSO != null && skillLevelText != null)
        {
            
            UpdateUI();
        }
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