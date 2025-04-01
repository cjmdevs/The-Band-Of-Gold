using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDrops : MonoBehaviour
{
    [SerializeField] private GameObject goldCoin;
    [SerializeField] private CoinManager coinManager; // Reference to the CoinManager
    [SerializeField] private int maxGold = 5; // Maximum number of gold coins to spawn

    public void DropItems() {

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