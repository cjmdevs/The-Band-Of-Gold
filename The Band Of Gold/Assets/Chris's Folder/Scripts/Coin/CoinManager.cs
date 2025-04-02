using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public int coinCount;
    public TMPro.TMP_Text coinText;
    public levelupManager levelManager;
    internal static readonly object instance;

    void Update()
    {
        coinText.text = "Silver:  " + coinCount.ToString();
    }

    public void AddCoins(int amount)
    {
        coinCount += amount;
        levelManager.UpdateCoinText(); // Update coin text after adding coins
    }

    public void SpendCoins(int amount)
    {
        coinCount -= amount;
        levelManager.UpdateCoinText(); // Update coin text after spending coins
    }
}