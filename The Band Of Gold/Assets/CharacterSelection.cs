using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [Header("Character Class Screen")]
    [SerializeField] private GameObject ClassSelectionParent;
    [SerializeField] private GameObject BackPanel;
    [SerializeField] private Button SwordsmanButton;
    [SerializeField] private Button MageButton;
    [SerializeField] private Button ArcherButton;

    [Header("Confirmation Popup")]
    [SerializeField] private GameObject ConfirmationParent;
    [SerializeField] private GameObject ClassSelectedParent;
    [SerializeField] private GameObject SwordsmanText;
    [SerializeField] private GameObject MageText;
    [SerializeField] private GameObject ArcherText;
    [SerializeField] private Button ConfirmButton;
    [SerializeField] private Button DeclineButton;

    [Header("Notice Popup")]
    [SerializeField] private GameObject NoticeParent;
    [SerializeField] private Button ProceedButton;
    [SerializeField] private Button ReturnButton;

    private string selectedClass = "";

    void Start()
    {
        // Initialize UI states
        ConfirmationParent.SetActive(false);
        NoticeParent.SetActive(false);
        ClassSelectionParent.SetActive(true);
        BackPanel.SetActive(false);

        // Disable all class texts initially
        SwordsmanText.SetActive(false);
        MageText.SetActive(false);
        ArcherText.SetActive(false);

        // Assign button listeners
        SwordsmanButton.onClick.AddListener(() => SelectClass("Swordsman"));
        MageButton.onClick.AddListener(() => SelectClass("Mage"));
        ArcherButton.onClick.AddListener(() => SelectClass("Archer"));

        ConfirmButton.onClick.AddListener(ConfirmSelection);
        DeclineButton.onClick.AddListener(() => ConfirmationParent.SetActive(false));
        DeclineButton.onClick.AddListener(() => BackPanel.SetActive(false));

        ProceedButton.onClick.AddListener(ProceedToGame);
        ReturnButton.onClick.AddListener(() => NoticeParent.SetActive(false));
        ReturnButton.onClick.AddListener(() => BackPanel.SetActive(false));

        // Assign OnClick
        SwordsmanButton.onClick.AddListener(OnSwordsmanButtonClicked);
        MageButton.onClick.AddListener(OnMageButtonClicked);
        ArcherButton.onClick.AddListener(OnArcherButtonClicked);
    }

    void OnSwordsmanButtonClicked()
    {
        Debug.Log("Swordsman Selected");
        SaveCharacter();
    }

    void OnMageButtonClicked()
    {
        Debug.Log("Mage Selected");
        SaveCharacter();
    }

    void OnArcherButtonClicked()
    {
        Debug.Log("Archer Selected");
        SaveCharacter();
    }

    void SelectClass(string className)
    {
        selectedClass = className;

        // Disable all texts first
        SwordsmanText.SetActive(false);
        MageText.SetActive(false);
        ArcherText.SetActive(false);

        // Enable the correct text based on selection
        switch (className)
        {
            case "Swordsman":
                SwordsmanText.SetActive(true);
                break;
            case "Mage":
                MageText.SetActive(true);
                break;
            case "Archer":
                ArcherText.SetActive(true);
                break;
        }

        // Show confirmation popup
        ConfirmationParent.SetActive(true);
        BackPanel.SetActive(true);
    }

    void ConfirmSelection()
    {
        ConfirmationParent.SetActive(false);
        NoticeParent.SetActive(true);
    }

    void ProceedToGame()
    {
        Debug.Log($"Character '{selectedClass}' selected. Proceeding to game...");
        // Load the game scene or proceed with game logic
        SceneManager.LoadSceneAsync("Test Scene");
        SaveCharacter();
    }

    public void SaveCharacter()
    {
        PlayerPrefs.SetString("CharacterSelected", selectedClass);
    }
}
