using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_combat : MonoBehaviour
{
    public int damage = 1;
    public float KnockbackForce;
    public float stunTime;

    public Transform attackPoint;
    public float weaponRange;
    public bool useSwordAttack = false;

    public LayerMask playerLayer;

    public GameObject projectilePrefab; // Assign in Inspector
    public Transform projectileSpawnPoint; // Assign in Inspector
    public float projectileSpeed = 5f; // Adjust speed as needed

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
        if (!useSwordAttack && (playerLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            audioManager.PlaySFX(audioManager.playerHit);
            collision.GetComponent<PlayerHealth>().ChangeHealth(-damage);
            collision.GetComponent<PlayerController>().Knockback(transform, KnockbackForce, stunTime);
            Debug.Log("Touch Attack");
        }
    }

    // Special attack function to launch a projectile (Call this from Animation Event)
    public void LaunchProjectile()
{
    GameObject player = GameObject.FindGameObjectWithTag("Player"); // Find the player
    if (player == null) return;

    float attackRadius = 15f; // Set the max distance at which the enemy can shoot
    float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

    if (distanceToPlayer <= attackRadius) // Only shoot if player is within range
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 direction = (player.transform.position - projectileSpawnPoint.position).normalized; // Get direction to player
            rb.velocity = direction * projectileSpeed; // Move projectile toward player
        }

        Debug.Log("Projectile Launched!");
    }
    else
    {
        Debug.Log("Player not in range, no projectile launched.");
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
