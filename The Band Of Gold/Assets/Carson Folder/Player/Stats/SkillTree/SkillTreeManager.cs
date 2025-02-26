using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Unity.VisualScripting;

public class SkillTreeManager : MonoBehaviour
{
    public SkillSlot[] skillSlots;
    public TMP_Text pointsText;
    public int availablePoints;

    private void OnEnable()
    {
       SkillSlot.OnAblityPointSpent += HandleAblityPointsSpent; 
       SkillSlot.OnSkillMaxed += HandleSkillMaxed;
    }
    private void OnDisable()
    {
       SkillSlot.OnAblityPointSpent -= HandleAblityPointsSpent; 
       SkillSlot.OnSkillMaxed -= HandleSkillMaxed;
    }

    private void Start()
    {
        foreach ( SkillSlot slot in skillSlots)   
        {
            slot.skillButton.onClick.AddListener(slot.TryUpgradeSkill);
        }
        UpdateAblityPoints(0);
    }

    private void HandleAblityPointsSpent(SkillSlot skillSlot)
    {
        if(availablePoints > 0)
        {
            UpdateAblityPoints(-1);
        }
    }
    private void HandleSkillMaxed(SkillSlot skillSlot)
    {
        foreach (SkillSlot slot in skillSlots)
        {
            if(!slot.isUnlocked && slot.CanUnlockSkill())
            {
                slot.Unlock();
            }
            
        }
    }
    public void UpdateAblityPoints(int amount){
        availablePoints+= amount;
        pointsText.text = "Points: " + availablePoints;
    }
}
