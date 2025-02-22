using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goldCoin, healthGlobe, staminaGlobe;
    [SerializeField] private CoinManager coinManager; // Reference to the CoinManager
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
                GameObject coin = Instantiate(goldCoin, transform.position, Quaternion.identity);
                Pickup pickupScript = coin.GetComponent<Pickup>();
                if (pickupScript != null) {
                    pickupScript.cm = coinManager; // Set the CoinManager reference
                }
            }
        }
    }
}