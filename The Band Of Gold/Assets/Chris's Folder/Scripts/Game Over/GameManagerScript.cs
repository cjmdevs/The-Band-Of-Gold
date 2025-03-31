using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverScreen;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true; // this needs to be true
        // Cursor.lockState = CursorLockMode.Locked; // we do not need this

    }

    // Update is called once per frame
    void Update()
    {
       if (gameOverScreen.activeInHierarchy)
       {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
       }
       else
       {
        // this is breaking the combat
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked; 
       }
    }

    public void GameOverScreen()
    {
        gameOverScreen.SetActive(true); 
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
