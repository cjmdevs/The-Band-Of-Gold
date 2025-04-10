using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goldCoin, healthGlobe, staminaGlobe;
    [SerializeField] private int maxGold = 5; // Maximum number of gold coins to spawn

    public void DropItems() {
        int randomNum = Random.Range(1, 5);

        if (randomNum == 1) {
            Instantiate(healthGlobe, transform.position, Quaternion.identity); 
        } 

        if (randomNum == 2) {
            Instantiate(staminaGlobe, transform.position, Quaternion.identity); 
        }
        // spawn a gold coin 
        if (randomNum == 3) {
            int randomAmountOfGold = Random.Range(1, maxGold);
            
            for (int i = 0; i < randomAmountOfGold; i++)
            {
                Instantiate(goldCoin, transform.position, Quaternion.identity);
                // No CoinManager reference needed anymore
            }
        }
    }
}