using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTriggerCollider : MonoBehaviour
{
    public GameObject uiShop;
    public GameObject shop;
    private void Start()
    {
        if (uiShop == null)
        uiShop.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D shop)
    {
        if (shop.gameObject.activeSelf)
        {
            uiShop.SetActive(true);
        }
    }
}
