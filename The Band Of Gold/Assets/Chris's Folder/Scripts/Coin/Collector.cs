using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
       var coin = collision.GetComponent<Coin>(); 
       if (coin != null)
       {
        coin.Collect();
       }
    }
}
