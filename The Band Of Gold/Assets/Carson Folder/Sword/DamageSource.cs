using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{   
    public float knockbackForce = 50f;
    public float knockbackTime = 0.15f;
    public float stunTime = 0.3f;

    [SerializeField] private int damageAmount = 1;
    private void OnTriggerEnter2D(Collider2D other) {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        Knockback knockback = other.gameObject.GetComponent<Knockback>();
        enemyHealth?.TakeDamage(damageAmount);
        knockback?.EnemyKnockback(transform, knockbackForce, knockbackTime ,stunTime);

    }
}
