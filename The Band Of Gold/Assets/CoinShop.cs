using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  

public class CoinShop : MonoBehaviour
{
    public void MerchantUI()
    {
        SceneManager.LoadScene("MerchantUI");
        Debug.Log("MerchantUI");
    } 
}
