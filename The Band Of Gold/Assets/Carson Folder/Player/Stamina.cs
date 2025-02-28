using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : Singleton<Stamina>
{
    public int CurrentStamina { get; private set; }

    [SerializeField] private Sprite fullStaminaImage, emptyStaminaImage;

    private Transform staminaContainer;
    public int startingStamina = 3;
    const string STAMINA_CONTAINER_TEXT = "Stamina Container";

    protected override void Awake()
    {
        base.Awake();

        StatsManager.Instance.maxStamina = startingStamina;
        CurrentStamina = startingStamina;
    }

    private void Start()
    {
        staminaContainer = GameObject.Find(STAMINA_CONTAINER_TEXT).transform;
        UpdateStaminaUI();
    }

    public void UseStamina()
    {
        if (CurrentStamina > 0)
        {
            CurrentStamina--;
            UpdateStaminaImage();
        }
    }

    public void RefreshStamina()
    {
        if (CurrentStamina < StatsManager.Instance.maxStamina)
        {
            CurrentStamina++;
        }
        UpdateStaminaImage();
    }

    private IEnumerator RefreshStaminaRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(StatsManager.Instance.staminaRegen);
            RefreshStamina();
        }
    }

    private void UpdateStaminaImage()
{
    int maxStamina = StatsManager.Instance.maxStamina;
    Debug.Log("Max Stamina: " + maxStamina); // Add this line
    int childCount = staminaContainer.childCount;


    for (int i = 0; i < maxStamina; i++)
    {
        if (i < childCount) // Check if the child exists
        {
            if (i < CurrentStamina)
            {
                staminaContainer.GetChild(i).GetComponent<Image>().sprite = fullStaminaImage;
            }
            else
            {
                staminaContainer.GetChild(i).GetComponent<Image>().sprite = emptyStaminaImage;
            }
        }
        else
        {
            // Handle the case where the child doesn't exist (optional)
            // You could log a warning or add a new orb here if needed.
            Debug.LogWarning("Stamina Image, child index out of range. Index: " + i);
            break; // Stop the loop to avoid further errors
        }
    }

    if (CurrentStamina < maxStamina)
    {
        StopAllCoroutines();
        StartCoroutine(RefreshStaminaRoutine());
    }
}


    public void SetMaxStamina(int newMaxStamina)
    {
        StatsManager.Instance.maxStamina = newMaxStamina;
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, StatsManager.Instance.maxStamina);
        UpdateStaminaUI();
    }

    // Call this method whenever CurrentStamina is changed.
    public void SetCurrentStamina(int newCurrentStamina)
    {
        CurrentStamina = Mathf.Clamp(newCurrentStamina, 0, StatsManager.Instance.maxStamina);
        UpdateStaminaUI();
    }

    private void UpdateStaminaUI()
    {
        // Clear existing stamina orbs
        foreach (Transform child in staminaContainer)
        {
            Destroy(child.gameObject);
        }

        // Create new stamina orbs
        for (int i = 0; i < StatsManager.Instance.maxStamina; i++)
        {
            GameObject staminaOrb = new GameObject("StaminaOrb", typeof(RectTransform), typeof(Image));
            staminaOrb.transform.SetParent(staminaContainer);
            RectTransform rectTransform = staminaOrb.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(50, 50); // Adjust size as needed
            rectTransform.localScale = Vector3.one; // Ensure scale is 1,1,1
            staminaOrb.GetComponent<Image>().sprite = (i < CurrentStamina) ? fullStaminaImage : emptyStaminaImage;
        }
    }
}