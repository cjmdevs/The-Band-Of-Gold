using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;

public class PauseMenu : MonoBehaviour
{    
    [Header("Settings Button")]
    [SerializeField] private Button BackButton;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public static bool isPaused;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializePauseState()
    {
        isPaused = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        BackButton.onClick.AddListener(OnBackButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key pressed");
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                {
                    PauseGame();
                }
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();   
        Debug.Log("Game is exiting");
    }

    public void SettingsMenu()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
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
        settingsMenu.SetActive(false); // Deactivate settings menu
        pauseMenu.SetActive(true); // Activate pause menu
        isPaused = true;
    }
}
