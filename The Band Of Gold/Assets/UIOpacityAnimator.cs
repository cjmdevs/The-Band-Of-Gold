using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIOpacityAnimator : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 5f;

    void Start()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
        
        // Have the menu buttons disabled on start, opacity at 0
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Start Fade-In Contribution
        StartCoroutine(FadeInUI());
    }

    IEnumerator FadeInUI()
    {
        yield return new WaitForSeconds(0.75f);

        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha;
        float targetAlpha = 1f;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null;
        }
        
        // 
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
