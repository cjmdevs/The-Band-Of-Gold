using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    public static CoinCounter instance;

    public TMP_Text coinText;
    public int currentSilver = 0;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        coinText.text = "Silver: " + currentSilver.ToString();
    }

    public void IncreaseSilver(int v)
    {
        currentSilver += v;
        coinText.text = "Silver: " + currentSilver.ToString();
    }
}
