using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
    [Header("Character Class Screen")]
    [SerializeField] public GameObject ClassSelection;
    [SerializeField] public Button SwordsmanButton;
    [SerializeField] public Button MageButton;
    [SerializeField] public Button ArcherButton;

    [Header("Confirmation Popup")]
    [SerializeField] public GameObject ConfirmationPopup;
    [SerializeField] public GameObject ClassSelected;
    [SerializeField] public Button ConfirmButton;
    [SerializeField] public Button DeclineButton;

    [Header("Notice PopUp")]
    [SerializeField] public GameObject NoticePopup;
    [SerializeField] public Button PositiveButton;
    [SerializeField] public Button Back;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
