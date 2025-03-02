using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour
{
    [Header("Single Moms")]
    [SerializeField] private GameObject singleMomsAd;
    [SerializeField] private Button singleMomsAdButton;
    [SerializeField] private GameObject singleMomsAdImage;

    [Header("Try Not To ...")]
    [SerializeField] private Button tryNotToAd;
    [SerializeField] private GameObject tryNotToAdImage;

    // Start is called before the first frame update
    void Start()
    {
        singleMomsAdImage.SetActive(false);
        tryNotToAdImage.SetActive(false);

        singleMomsAdButton.onClick.AddListener(OnSingleMomsAd);
        tryNotToAd.onClick.AddListener(OnTryNotToAd);
    }

    void OnSingleMomsAd()
    {
        singleMomsAdImage.SetActive(true);
    }

    void OnTryNotToAd()
    {
        tryNotToAdImage.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
