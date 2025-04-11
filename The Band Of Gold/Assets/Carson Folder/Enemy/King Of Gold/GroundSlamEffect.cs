using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlamEffect : MonoBehaviour
{
    public float expandDuration = 0.5f;
    public float stayDuration = 0.3f;
    public float fadeDuration = 0.3f;
    
    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        
        // Start at zero scale
        transform.localScale = Vector3.zero;
    }
    
    private void Start()
    {
        StartCoroutine(AnimateEffect());
    }
    
    private IEnumerator AnimateEffect()
    {
        // Expand effect
        float elapsed = 0f;
        Vector3 targetScale = transform.localScale;
        while (elapsed < expandDuration)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsed / expandDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
        
        // Stay at full size
        yield return new WaitForSeconds(stayDuration);
        
        // Fade out
        Color startColor = spriteRenderer.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            spriteRenderer.color = Color.Lerp(startColor, endColor, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        Destroy(gameObject);
    }
}
