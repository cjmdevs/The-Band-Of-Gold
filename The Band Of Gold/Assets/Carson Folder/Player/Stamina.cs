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
    private int maxStamina;
    const string STAMINA_CONTAINER_TEXT = "Stamina Container";

    protected override void Awake()
    {
        base.Awake();

        maxStamina = startingStamina;
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
        if (CurrentStamina < maxStamina)
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
        for (int i = 0; i < maxStamina; i++)
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
        if (CurrentStamina < maxStamina)
        {
            StopAllCoroutines();
            StartCoroutine(RefreshStaminaRoutine());
        }
    }

    public void SetMaxStamina(int newMaxStamina)
    {
        maxStamina = newMaxStamina;
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, maxStamina);
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
        for (int i = 0; i < maxStamina; i++)
        {
            GameObject staminaOrb = new GameObject("StaminaOrb", typeof(RectTransform), typeof(Image));
            staminaOrb.transform.SetParent(staminaContainer);
            staminaOrb.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50); // Adjust size as needed
            staminaOrb.GetComponent<Image>().sprite = (i < CurrentStamina) ? fullStaminaImage : emptyStaminaImage;
        }
    }
}
