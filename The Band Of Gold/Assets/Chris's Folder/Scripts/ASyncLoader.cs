using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ASyncLoader : MonoBehaviour
{
    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingBar;

    [Header("Disabled Screens")]
    [SerializeField] private GameObject CharacterSelectScreen;
    
    public void LoadLevelBtn(string levelToLoad)
    {
        CharacterSelectScreen.SetActive(false);
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelASync(levelToLoad));
    }

    IEnumerator LoadLevelASync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingBar.value = progressValue;
            yield return null;
        }
    }
}
