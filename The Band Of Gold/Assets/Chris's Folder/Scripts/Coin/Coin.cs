using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectible
{
    public void Collet()
    {
        Debug.Log("You Collected A Coin!");
    }
}
