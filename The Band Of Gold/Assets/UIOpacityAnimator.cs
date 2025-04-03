using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIOpacityAnimator : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 3f;
    public GameObject title;
    public float titleDropDuration = 2f;
    public Vector2 titleStartPos;
    public Vector2 titleEndPos;
    public RectTransform titleEndTransform;
    public Button[] buttons;

    private bool isSkipping = false;
    private RectTransform titleRectTransform;

    void Start()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        // **Ensure UI starts fully transparent & disabled**
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = true;

        titleRectTransform = title?.GetComponent<RectTransform>();

        if (titleRectTransform != null)
        {
            if (titleEndTransform != null)
            {
                titleEndPos = titleEndTransform.anchoredPosition;
            }

            titleRectTransform.anchoredPosition = titleStartPos;
        }

        // **Ensure buttons are disabled at start**
        foreach (Button button in buttons)
        {
            if (button != null)
            {
                button.interactable = false; // Disable interactions
                button.onClick.AddListener(SkipAnimations); // Allow skipping via button click
            }
        }

        StartCoroutine(FadeInUI());
        if (title != null)
            StartCoroutine(DropTitle());
    }

    void Update()
    {
        // **Allow skipping via Event System buttons (Submit, Cancel, etc.)**
        if (Input.anyKeyDown)
        {
            GameObject selectedObject = EventSystem.current?.currentSelectedGameObject;
            
            if (selectedObject != null && selectedObject.GetComponent<Button>() != null)
            {
                SkipAnimations();
            }
        }
    }

    IEnumerator FadeInUI()
    {
        yield return new WaitForSeconds(0.5f);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration && !isSkipping)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            yield return null;
        }

        // **Ensure final state**
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // **Enable buttons after fade-in completes**
        foreach (Button button in buttons)
        {
            if (button != null)
            {
                button.interactable = true;
            }
        }
    }

    IEnumerator DropTitle()
    {
        float elapsedTime = 0f;

        if (titleRectTransform != null) // UI element
        {
            Vector2 startAnchoredPos = titleStartPos;
            Vector2 endAnchoredPos = titleEndPos;

            while (elapsedTime < titleDropDuration && !isSkipping)
            {
                elapsedTime += Time.deltaTime;
                titleRectTransform.anchoredPosition = Vector2.Lerp(startAnchoredPos, endAnchoredPos, elapsedTime / titleDropDuration);
                yield return null;
            }

            titleRectTransform.anchoredPosition = endAnchoredPos;
        }
    }

    void SkipAnimations()
    {
        if (isSkipping) return;
        isSkipping = true;

        // Instantly finish UI fade
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // Instantly set title to final position
        if (titleRectTransform != null)
        {
            titleRectTransform.anchoredPosition = titleEndPos;
        }

        // **Immediately enable buttons**
        foreach (Button button in buttons)
        {
            if (button != null)
            {
                button.interactable = true;
            }
        }
    }
}
