using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamWarningIndicator : MonoBehaviour
{
    public float pulseDuration = 0.5f;
    public Color startColor = new Color(1, 0, 0, 0.2f);
    public Color endColor = new Color(1, 0, 0, 0.5f);
    
    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        
        spriteRenderer.color = startColor;
    }
    
    private void Start()
    {
        StartCoroutine(PulseEffect());
    }
    
    private IEnumerator PulseEffect()
    {
        while (true)
        {
            // Pulse to end color
            float elapsed = 0f;
            while (elapsed < pulseDuration / 2f)
            {
                spriteRenderer.color = Color.Lerp(startColor, endColor, elapsed / (pulseDuration / 2f));
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            // Pulse back to start color
            elapsed = 0f;
            while (elapsed < pulseDuration / 2f)
            {
                spriteRenderer.color = Color.Lerp(endColor, startColor, elapsed / (pulseDuration / 2f));
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}
