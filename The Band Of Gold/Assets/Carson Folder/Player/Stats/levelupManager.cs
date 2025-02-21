using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class levelupManager : MonoBehaviour
{
    public int currentLevel = 1;
    public int coinsNeededForNextLevel = 10;
    public TMPro.TMP_Text levelText;
    public TMPro.TMP_Text coinsNeededText; // Text for coins needed
    public CoinManager coinManager;
    public Button levelUpButton; // Button for manual level up

    void Start()
    {
        UpdateLevelText();
        UpdateCoinText(); // Initialize coin text
        levelUpButton.onClick.AddListener(AttemptManualLevelUp); // Add listener to the button
    }

    public void CheckForLevelUp()
    {
        while (coinManager.coinCount >= coinsNeededForNextLevel)
        {
            LevelUp();
        }
    }

    public void AttemptManualLevelUp()
    {
        if (coinManager.coinCount >= coinsNeededForNextLevel)
        {
            LevelUp();
        }
        else
        {
            // Optionally display a message that not enough coins are available
            Debug.Log("Not enough coins to level up.");
        }
    }

    void LevelUp()
    {
        coinManager.SpendCoins(coinsNeededForNextLevel);
        currentLevel++;
        coinsNeededForNextLevel = CalculateNextLevelCost(currentLevel);
        UpdateLevelText();
        UpdateCoinText(); // Update coins needed text

        // ***********************************************************************************
        // Implement level up actions here
        // ***********************************************************************************
        Debug.Log("Leveled up to level " + currentLevel + "!");
    }

    int CalculateNextLevelCost(int level)
    {
        return Mathf.RoundToInt(10 * Mathf.Pow(1.5f, level - 1));
    }

    void UpdateLevelText()
    {
        levelText.text = "Level: " + currentLevel;
    }

    public void UpdateCoinText() // Made public so CoinManager can call it
    {
        coinsNeededText.text = "Coins needed: " + coinsNeededForNextLevel;
    }
}

