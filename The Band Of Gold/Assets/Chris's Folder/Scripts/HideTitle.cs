using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
   public GameObject Title;
   public Button NewGameButton;
   public Button ContinueButton;
   public Button LoadGameButton;
   public Button RemoveButton;

   void Start()
   {
    NewGameButton.onClick.AddListener(OnNewGame);
    ContinueButton.onClick.AddListener(OnContinue);
    LoadGameButton.onClick.AddListener(OnLoad);
    RemoveButton.onClick.AddListener(OnRemove);
   }

   void OnNewGame()
   {
    HideTitle();
   }

   void OnContinue()
   {
    HideTitle();
   }

   void OnLoad()
   {
    HideTitle();
   }

   void OnRemove()
   {
    HideTitle();
   }

   void HideTitle()
   {
    Title.SetActive(false);
   }
}
