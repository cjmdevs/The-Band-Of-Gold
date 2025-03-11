using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HideToolbar : MonoBehaviour
{
    public GameObject ActiveInventory;
    public GameObject GameOverScreen;
    public Button RetryButton;
    public Button MainMenuButton;
    public Button QuitButton;

    void Start()
    {
        RetryButton.onClick.AddListener(OnRetry);
        MainMenuButton.onClick.AddListener(OnMainMenu);
        QuitButton.onClick.AddListener(OnQuit);
    }

    void Update()
    {
        if (GameOverScreen.activeSelf)
        {
            ActiveInventory.SetActive(false);
        }
    }

    void OnRetry()
    {
        UnHideActiveInventory();
    }

    void OnMainMenu()
    {
        HideActiveInventory();
    }

    void OnQuit()
    {
        HideActiveInventory();
    }

    void HideActiveInventory()
    {
        ActiveInventory.SetActive(false);
    }

    void UnHideActiveInventory()
    {
        ActiveInventory.SetActive(true);
    }
}
