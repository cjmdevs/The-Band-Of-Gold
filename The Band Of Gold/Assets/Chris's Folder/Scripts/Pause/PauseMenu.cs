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

    public HideToolbar hideToolbar;

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

        if (hideToolbar != null && hideToolbar.UICanvas != null)
        {
            CanvasGroup canvasGroup = hideToolbar.UICanvas.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = hideToolbar.UICanvas.gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0.2f; // Set to semi-transparent

            foreach (GameObject gameObj in GameObject.FindObjectsOfType<GameObject>())
            {
                if (gameObj.name == "Inventory")
                {
                    Image image = gameObj.GetComponent<Image>();
                    image = gameObj.AddComponent<Image>();
                    image.color = new Color(80 / 255f, 80 / 255f, 80 / 255f);
                }

                if (gameObj.name == "Item")
                {
                    Image image = gameObj.GetComponent<Image>();
                    image = gameObj.AddComponent<Image>();
                    image.color = new Color(80 / 255f, 80 / 255f, 80 / 255f);
                }                
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        if (hideToolbar != null && hideToolbar.UICanvas != null)
        {
            CanvasGroup canvasGroup = hideToolbar.UICanvas.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f; // Set to fully opaque
            }
        }

        Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();

            foreach (GameObject gameObj in GameObject.FindObjectsOfType<GameObject>())
            {
                if (gameObj.name == "Inventory")
                {
                    Image image = gameObj.GetComponent<Image>();
                    image = gameObj.AddComponent<Image>();
                    image.color = new Color(1f, 1f, 1f);
                }
                {
                    // Store the original color
                   // if (!originalColors.ContainsKey(gameObj))
                    //{
                    //    originalColors[gameObj] = image.color;

                }
            }
        
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
