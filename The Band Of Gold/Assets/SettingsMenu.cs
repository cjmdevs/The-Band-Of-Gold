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

    [Header("Settings' Buttons")]
    [SerializeField] private Button AudioButton;
    [SerializeField] private Button GraphicsButton;

    [Header("Settings Parents")]
    [SerializeField] private GameObject AudioSettings;
    [SerializeField] private GameObject GraphicsSettings;

    [Header("Back Buttons")]
    [SerializeField] private Button AudioBackButton;
    [SerializeField] private Button GraphicsBackButton;

    // Start is called before the first frame update
    void Start()
    {
        // Disable the Audio and Graphic Parents
        AudioSettings.SetActive(false);
        GraphicsSettings.SetActive(false);
        
        // Add listener to the back button, Audio, and Graphics
        BackButton.onClick.AddListener(OnBackButtonClicked);
        AudioButton.onClick.AddListener(OnAudioButtonClicked);
        GraphicsButton.onClick.AddListener(OnGraphicsButtonClicked);
        AudioBackButton.onClick.AddListener(OnAudioBackButtonClicked);
        GraphicsBackButton.onClick.AddListener(OnGraphicsBackButtonClicked);
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
        mainMenu.ActivateMenu(); // Activate main menu
    }

    private void OnAudioButtonClicked()
    {

    }

    private void OnGraphicsButtonClicked()
    {

    }

    private void OnAudioBackButtonClicked()
    {

    }

    private void OnGraphicsBackButtonClicked()
    {

    }
}
