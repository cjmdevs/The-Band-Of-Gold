using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public int coinCount;
    public TMPro.TMP_Text coinText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = ":  " + coinCount.ToString();

        if (coinCount > 0)
        {
            coinText.text = coinText.text + ""; 
        }
    }
}
