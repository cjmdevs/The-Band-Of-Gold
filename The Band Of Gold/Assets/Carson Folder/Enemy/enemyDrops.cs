using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDrops : MonoBehaviour
{
    [SerializeField] private GameObject goldCoin;
    [SerializeField] private int maxGold = 5; // Maximum number of gold coins to spawn

    public void DropItems() {
        int randomAmountOfGold = Random.Range(1, maxGold);
        
        for (int i = 0; i < randomAmountOfGold; i++)
        {
            Instantiate(goldCoin, transform.position, Quaternion.identity);
            // No CoinManager reference needed anymore
        }
    }
}