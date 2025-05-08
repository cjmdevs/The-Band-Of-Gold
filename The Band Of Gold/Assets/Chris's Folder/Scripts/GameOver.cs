using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public Animator playerAnimation;
    public GameObject gameOverScreen;

    private void Start()
    {
        playerAnimation = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        gameOverScreen.SetActive(false);
    }

    public void EndGame()
    {
        playerAnimation.SetBool("Hurt", true);
        Debug.Log("Game Over");
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }
}
