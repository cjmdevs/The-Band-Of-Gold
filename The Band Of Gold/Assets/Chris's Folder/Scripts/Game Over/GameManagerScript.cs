using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeGameManager()
    {
        if (instance == null)
        {
            GameObject gameManagerObject = new GameObject("GameManager");
            instance = gameManagerObject.AddComponent<GameManagerScript>();
        }
    }
    public GameData gameData;
    public GameObject gameOverScreen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (gameOverScreen == null)
        {
            gameOverScreen = GameObject.Find("GameOver Screen");
            if (gameOverScreen == null)
            {
                Debug.LogWarning("GameOverScreen not found in the scene! Attempting to instantiate a default GameOverScreen.");
            
                // Load a default prefab (ensure you have a prefab assigned in the inspector)
                GameObject defaultGameOverScreenPrefab = Resources.Load<GameObject>("DefaultGameOverScreen");
                if (defaultGameOverScreenPrefab != null)
                {
                    gameOverScreen = Instantiate(defaultGameOverScreenPrefab);
                    gameOverScreen.name = "GameOver Screen"; // Rename for consistency
                }
                else
                {
                    Debug.LogError("Default GameOverScreen prefab not found in Resources!");
                }
            }
        }
        Cursor.visible = true; // this needs to be true
        // Cursor.lockState = CursorLockMode.Locked; // we do not need this
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOverScreen != null && gameOverScreen.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            // Cursor.visible = false;
            // Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void GameOverScreen()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
        else
        {
            Debug.LogError("GameOverScreen is not assigned in the Inspector!");
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restart");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Main Menu");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
    
}
