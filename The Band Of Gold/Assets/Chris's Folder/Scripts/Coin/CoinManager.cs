using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinManager : MonoBehaviour, IDataPersistence
{
    public int coinCount;
    public TMPro.TMP_Text coinText;
    public levelupManager levelManager;
    internal static readonly object instance;

    private void OnEnable()
    {
        // Subscribe to the coin collection event
        GameEvents.OnCoinCollected += AddCoins;
    }

    private void OnDisable()
    {
        // Unsubscribe when the object is disabled
        GameEvents.OnCoinCollected -= AddCoins;
    }

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
        //SaveData(GameManagerScript.instance.gameData);
    }

    public void LoadData(GameData data)
    {
        coinCount = data.coinCount;
        levelManager.UpdateCoinText(); // Update coin text after loading data
    }

    public void SaveData(GameData data)
    {
        data.coinCount = coinCount;
    }
}