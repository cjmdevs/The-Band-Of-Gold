using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI retryText;
    private bool isGameOver = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //Disables panel if active
        gameOverPanel.SetActive(false);
        retryText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Trigger game over manually and check with bool so it isn't called multiple times
        if (Input.GetKeyDown(KeyCode.G) && !isGameOver)
        {
            isGameOver = true;
            
            StartCoroutine(GameOverSequence());
        }
        
        //If game is over
        if (isGameOver)
        {
            //If R is hit, restart the current scene
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            
            //If Q is hit, quit the game
            if (Input.GetKeyDown(KeyCode.Q))
            {
                print("Application Quit");
                Application.Quit();
            }

            // if ESC is hit, return to Main Menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Main Menu");
            }
        }
        
        
    }

    //controls game over canvas and there's a brief delay between main Game Over text and option to restart/quit text
    private IEnumerator GameOverSequence()
    {
        gameOverPanel.SetActive(true);
        
        yield return new WaitForSeconds(5.0f);

        retryText.gameObject.SetActive(true);
    }
}