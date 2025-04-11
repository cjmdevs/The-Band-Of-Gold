using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamController : MonoBehaviour
{
    private Transform source;
    private Vector2 direction;
    private float width;
    private float damage;
    private float damageTickRate;
    private float duration;
    private LayerMask playerLayer;
    
    private float damageTimer;
    private float beamLength = 50f; // Long enough to cover the screen
    private LineRenderer lineRenderer;
    
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.2f;
            lineRenderer.endWidth = 0.2f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }
    }
    
    public void Initialize(Transform beamSource, Vector2 beamDirection, float beamWidth, float beamDuration, float beamDamage, float tickRate, LayerMask targetLayer)
    {
        source = beamSource;
        direction = beamDirection;
        width = beamWidth;
        duration = beamDuration;
        damage = beamDamage;
        damageTickRate = tickRate;
        playerLayer = targetLayer;
        
        // Set beam width
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        
        // Set up line renderer
        lineRenderer.SetPosition(0, source.position);
        lineRenderer.SetPosition(1, (Vector2)source.position + direction * beamLength);
        
        // Add a light for beam glow effect (optional)
        GameObject lightObj = new GameObject("BeamLight");
        lightObj.transform.parent = transform;
        lightObj.transform.position = source.position + (Vector3)(direction * 2f);
        Light light = lightObj.AddComponent<Light>();
        light.color = new Color(1f, 0.2f, 0.2f); // Red-ish
        light.intensity = 2f;
        light.range = 5f;
    }
    
    private void Update()
    {
        damageTimer += Time.deltaTime;
        
        // Keep beam positioned at source
        if (source != null)
        {
            lineRenderer.SetPosition(0, source.position);
            lineRenderer.SetPosition(1, (Vector2)source.position + direction * beamLength);
        }
        
        // Check for player in beam and apply damage at tick rate
        if (damageTimer >= damageTickRate)
        {
            ApplyDamageToPlayersInBeam();
            damageTimer = 0f;
        }
    }
    
    private void ApplyDamageToPlayersInBeam()
    {
        RaycastHit2D hit = Physics2D.Raycast(source.position, direction, beamLength, playerLayer);
        if (hit.collider != null)
        {
            PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ChangeHealth(-damage);
            }
        }
    }
}
