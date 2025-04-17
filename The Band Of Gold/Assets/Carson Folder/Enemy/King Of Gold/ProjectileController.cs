using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public GameObject impactParticle;
    
    private Vector2 direction;
    private float speed;
    private float damage;
    private float lifetime;
    private float timer;
    private bool isRingProjectile = false;
    private float initialDelay = 0f;
    
    public void Initialize(Vector2 dir, float projectileSpeed, float projectileDamage, float projectileLifetime)
    {
        direction = dir.normalized;
        speed = projectileSpeed;
        damage = projectileDamage;
        lifetime = projectileLifetime;
        timer = 0f;
        
        // Rotate to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    
    // Optional method for ring patterns
    public void InitializeRingProjectile(Vector2 dir, float projectileSpeed, float projectileDamage, float projectileLifetime)
    {
        Initialize(dir, projectileSpeed, projectileDamage, projectileLifetime);
        isRingProjectile = true;
    }
    
    private void Update()
    {
        timer += Time.deltaTime;
        
        // Move in direction
        transform.Translate(direction * speed * Time.deltaTime);
        
        // Despawn if lifetime is over
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if collision is on the Wall layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // Spawn impact particle and destroy projectile
            SpawnImpactParticle();
            Destroy(gameObject);
        }
        
        // Check for player to apply damage (keeping this separate from wall check)
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.ChangeHealth(-damage);
            
            // If you want to destroy the projectile when hitting a player too
            SpawnImpactParticle();
            Destroy(gameObject);
        }
    }
    
    private void SpawnImpactParticle()
    {
        if (impactParticle != null)
        {
            GameObject particle = Instantiate(impactParticle, transform.position, Quaternion.identity);
            
            // Auto-destroy the particle after a time
            Destroy(particle, 1.0f);
        }
    }
}