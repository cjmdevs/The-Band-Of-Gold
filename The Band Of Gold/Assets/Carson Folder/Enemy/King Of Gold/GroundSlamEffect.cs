using System.Collections;
using UnityEngine;

public class GroundSlamEffect : MonoBehaviour
{
    [Header("Visual Settings")]
    public float radius = 5f;
    public float expandDuration = 0.8f;
    public float lingerDuration = 0.3f;
    public Color startColor = new Color(1f, 0.5f, 0f, 1f);
    public Color endColor = new Color(1f, 0.5f, 0f, 0f);
    
    [Header("Rendering Settings")]
    public string sortingLayerName = "Effects";
    public int sortingOrder = 10;
    
    [Header("Damage Settings")]
    public float damage = 2f;
    public float playerStunDuration = 1.5f;
    public LayerMask playerLayer;
    
    private ParticleSystem waveParticles;
    private bool damageApplied = false;
    private float currentRadius = 0f;
    
    private void Awake()
    {
        // Create the particle system
        CreateWaveParticleSystem();
    }
    
    private void Start()
    {
        // Play the effect
        waveParticles.Play();
        
        // Start tracking the expanding wave for damage
        StartCoroutine(TrackExpansion());
        
        // Self-destroy after effect completes
        Destroy(gameObject, expandDuration + lingerDuration + 0.2f);
    }
    
    private void CreateWaveParticleSystem()
    {
        // Create particle system
        waveParticles = gameObject.AddComponent<ParticleSystem>();
        
        // Main module
        var main = waveParticles.main;
        main.duration = expandDuration;
        main.loop = false;
        main.startLifetime = expandDuration;
        main.startSpeed = radius / expandDuration;
        main.startSize = 0.5f;
        main.startColor = startColor;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = 100;
        
        // Emission module
        var emission = waveParticles.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] { 
            new ParticleSystem.Burst(0f, 100)
        });
        
        // Shape module
        var shape = waveParticles.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.1f; // Start from almost center
        shape.radiusThickness = 0f; // Emit from edge only
        shape.arc = 360f;
        
        // Color over lifetime
        var colorOverLifetime = waveParticles.colorOverLifetime;
        colorOverLifetime.enabled = true;
        
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { 
                new GradientColorKey(startColor, 0.0f), 
                new GradientColorKey(endColor, 1.0f) 
            },
            new GradientAlphaKey[] { 
                new GradientAlphaKey(1.0f, 0.0f), 
                new GradientAlphaKey(0.0f, 1.0f) 
            }
        );
        
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
        
        // Size over lifetime
        var sizeOverLifetime = waveParticles.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve sizeCurve = new AnimationCurve();
        sizeCurve.AddKey(0.0f, 0.3f);
        sizeCurve.AddKey(0.2f, 1.0f);
        sizeCurve.AddKey(1.0f, 0.2f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1.0f, sizeCurve);
        
        // Configure renderer for 2D high-priority rendering
        var renderer = waveParticles.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
        
        // Set 2D sorting properties to ensure high-level rendering
        renderer.sortingLayerName = sortingLayerName;
        renderer.sortingOrder = sortingOrder;
        
        // Make sure particles render on top by setting material properties
        renderer.material.renderQueue = 3100; // Higher than default transparent queue (3000)
        
        // Make sure the particles are facing the camera in 2D
        renderer.alignment = ParticleSystemRenderSpace.View;
    }
    
    private IEnumerator TrackExpansion()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < expandDuration)
        {
            // Calculate current radius based on elapsed time
            currentRadius = (elapsedTime / expandDuration) * radius;
            
            // Check for player in the expanding wave
            CheckForPlayer();
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    
    private void CheckForPlayer()
    {
        // Get all player colliders within the current radius
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, currentRadius, playerLayer);
        
        foreach (Collider2D hitPlayer in hitPlayers)
        {
            // Get the player's position
            Vector2 playerPos = hitPlayer.transform.position;
            
            // Calculate distance to the wave's edge
            float distanceToWaveEdge = Mathf.Abs(Vector2.Distance(transform.position, playerPos) - currentRadius);
            
            // If player is close to the wave edge (within a small tolerance), apply damage
            if (distanceToWaveEdge < 0.5f) // Adjust tolerance as needed
            {
                PlayerHealth playerHealth = hitPlayer.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.ChangeHealth(-damage);
                }
                
                // Apply stun if the player controller exists
                PlayerController playerController = hitPlayer.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    // Uncomment when you implement StunPlayer in your PlayerController
                    // playerController.StunPlayer(playerStunDuration);
                }
            }
        }
    }
    
    // Helper to visualize the effect radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}