using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HideToolbar : MonoBehaviour
{
    [Header("Player Inventory")]
    public GameObject ActiveInventory;

    [Header("Game Over Screen")]
    [SerializeField] public GameObject GameOverScreen;
    [SerializeField] public Button RetryButton;
    [SerializeField] public Button MainMenuButton;
    [SerializeField] public Button QuitButton;

    [Header("Inventory & Stamina")]
    [SerializeField] public GameObject UICanvas;

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
