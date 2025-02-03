using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;
    [SerializeField] private SettingsMenu settingsMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button NewGameButton;
    [SerializeField] private Button ContinueGameButton;
    [SerializeField] private Button LoadGameButton;
    [SerializeField] private Button OptionButton;

    private void Start()
    {
        DisableButtonsDependingOnData();
    }

    private void DisableButtonsDependingOnData()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            ContinueGameButton.interactable = false;
            LoadGameButton.interactable = false;
        }
    }

    public void OnNewGameClicked()
    {
        saveSlotsMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }

    public void OnLoadGameClicked() // Changed to OnLoadGameClicked
    {
        saveSlotsMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }

    public void OnContinueGameClicked()
    {
        DisableMenuButtons();
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadSceneAsync("Test Scene");
    }

    public void OnOptionButtonClicked()
    {
        this.DeactivateMenu();
        settingsMenu.ActivateMenu();
    }

    private void DisableMenuButtons()
    {
        NewGameButton.interactable = false;
        ContinueGameButton.interactable = false;
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
        DisableButtonsDependingOnData(); // Removed extra period
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
