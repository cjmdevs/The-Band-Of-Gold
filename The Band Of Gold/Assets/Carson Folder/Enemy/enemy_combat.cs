using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_combat : MonoBehaviour
{
    public int damage = 1;
    public float KnockbackForce;
    public float stunTime;

    // Optional: For sword-like attacks
    public Transform attackPoint;
    public float weaponRange;
    public bool useSwordAttack = false; // Flag to switch between attack types

    public LayerMask playerLayer;

    AudioManager audioManager;

    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void EnemyAttack()
    {
        if (useSwordAttack)
        {
            SwordAttack();
        }
        else
        {
            TouchAttack();
        }
    }

    private void SwordAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);

        if (hits.Length > 0)
        {
            audioManager.PlaySFX(audioManager.playerHit);
            hits[0].GetComponent<PlayerHealth>().ChangeHealth(-damage);
            hits[0].GetComponent<PlayerController>().Knockback(transform, KnockbackForce, stunTime);
            Debug.Log("Sword Attack");
        }
    }

    private void TouchAttack()
    {
        // Damage is applied in OnTriggerEnter2D
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!useSwordAttack && (playerLayer.value & (1 << collision.gameObject.layer)) != 0) //check if the collided object is on the player layer.
        {
            audioManager.PlaySFX(audioManager.playerHit);
            collision.GetComponent<PlayerHealth>().ChangeHealth(-damage);
            collision.GetComponent<PlayerController>().Knockback(transform, KnockbackForce, stunTime);
            Debug.Log("Touch Attack");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (useSwordAttack && attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
        }
    }
}