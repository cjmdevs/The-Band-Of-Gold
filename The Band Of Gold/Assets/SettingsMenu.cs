using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private MainMenu mainMenu; // Reference to the main menu

    [Header("Menu Buttons")]
    [SerializeField] private Button OptionButton;
    [SerializeField] private Button BackButton; // Reference to your back button

    [Header("Pause abdeca")]
    [SerializeField] private GameObject PauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        // Add listener to the back button
        BackButton.onClick.AddListener(OnBackButtonClicked);
    }

    // Method to activate the settings menu
    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
        // Additional logic if needed
    }

    // Method to deactivate the settings menu
    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    // Method called when the back button is clicked
    private void OnBackButtonClicked()
    {
        DeactivateMenu(); // Deactivate settings menu
        PauseMenu.SetActive(true); // Activate pause menu
        mainMenu.ActivateMenu(); // Activate main menu
    }
}
