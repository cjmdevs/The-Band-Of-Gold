using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataConntent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI percentageCompleteText;
    [SerializeField] private TextMeshProUGUI deathCountText;
    
    [Header("Remove Data Button")]
    [SerializeField] private Button removeButton;

    private Button saveSlotbutton;

    private void Awake()
    {
        saveSlotbutton = this.GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        // if there's no data for this profileId
        if (data == null)
        {
            noDataConntent.SetActive(true);
            hasDataContent.SetActive(false);
            removeButton.gameObject.SetActive(false);
        }
        // there is data for this profileId
        else
        {
            noDataConntent.SetActive(false);
            hasDataContent.SetActive(true);
            removeButton.gameObject.SetActive(true);

            percentageCompleteText.text = data.GetPercentageComplete() + "% COMPLETE";
            deathCountText.text = "DEATH COUNT: " + data.deathCount;
        }
    }

    public string GetProfileId()
    {
        return this.profileId;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotbutton.interactable = interactable;
        removeButton.interactable = interactable;
    }
}
