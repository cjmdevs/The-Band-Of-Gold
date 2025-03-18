using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;

    [Header("Loaded Character/Dash")]
    [SerializeField] private string loadedCharacter;
    [SerializeField] private string loadedDash;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerController>()) {
            SceneManager.LoadScene(sceneToLoad);
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
        }
        LoadCharacter();
        LoadDash();
    }
    
    public void LoadCharacter()
    {
        loadedCharacter = PlayerPrefs.GetString("CharacterSelected");
    }

    public void LoadDash()
    {
        loadedDash = PlayerPrefs.GetString("EnabledPlayerDash");
    }
}
