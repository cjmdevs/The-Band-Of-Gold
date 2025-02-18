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
   public Button OptionButton;
   public Button BackButton;
   public Button Back2ndButton;

   void Start()
   {
    NewGameButton.onClick.AddListener(OnNewGame);
    ContinueButton.onClick.AddListener(OnContinue);
    LoadGameButton.onClick.AddListener(OnLoad);
    OptionButton.onClick.AddListener(OnOptions);
    RemoveButton.onClick.AddListener(OnRemove);
    BackButton.onClick.AddListener(OnBack);
    Back2ndButton.onClick.AddListener(On2ndBack);
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

   void OnOptions()
   {
      HideTitle();
   }

   void OnRemove()
   {
    HideTitle();
   }

   void OnBack()
   {
      UnHideTitle();
   }

   void On2ndBack()
   {
      UnHideTitle();
   }

   void HideTitle()
   {
    Title.SetActive(false);
   }

   void UnHideTitle()
   {
      Title.SetActive(true);
   }
}
