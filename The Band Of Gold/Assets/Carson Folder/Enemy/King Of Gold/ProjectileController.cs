using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public ParticleSystem impactEffect;
    
    private Vector2 direction;
    private float speed;
    private float damage;
    private float lifetime;
    private float timer;
    
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
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ChangeHealth(-damage);
            }
            
            if (impactEffect != null)
            {
                ParticleSystem effect = Instantiate(impactEffect, transform.position, Quaternion.identity);
                effect.Play();
            }
            
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground") || collision.CompareTag("Wall"))
        {
            // Play impact effect on environment
            if (impactEffect != null)
            {
                ParticleSystem effect = Instantiate(impactEffect, transform.position, Quaternion.identity);
                effect.Play();
            }
            
            Destroy(gameObject);
        }
    }
}
