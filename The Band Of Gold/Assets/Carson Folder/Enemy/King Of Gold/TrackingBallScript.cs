// TrackingBallController.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBallScript : MonoBehaviour
{
    public float damage = 1f;
    public ParticleSystem impactEffect;
    
    private Transform target;
    private float speed;
    private float lifetime;
    private float timer;
    
    public void Initialize(Transform playerTarget, float ballSpeed, float ballLifetime)
    {
        target = playerTarget;
        speed = ballSpeed;
        lifetime = ballLifetime;
        timer = 0f;
    }
    
    private void Update()
    {
        timer += Time.deltaTime;
        
        // Despawn if lifetime is over
        if (timer >= lifetime)
        {
            StartCoroutine(DespawnEffect());
            return;
        }
        
        // Track the player if target exists
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);
            
            // Optional: Rotate to face movement direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
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
            
            StartCoroutine(DespawnEffect());
        }
    }
    
    private IEnumerator DespawnEffect()
    {
        // Disable collider
        GetComponent<Collider2D>().enabled = false;
        
        // Play impact effect
        if (impactEffect != null)
        {
            ParticleSystem effect = Instantiate(impactEffect, transform.position, Quaternion.identity);
            effect.Play();
        }
        
        // Scale down
        float despawnTime = 0.3f;
        float elapsedTime = 0f;
        Vector3 initialScale = transform.localScale;
        
        while (elapsedTime < despawnTime)
        {
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, elapsedTime / despawnTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        Destroy(gameObject);
    }
}
