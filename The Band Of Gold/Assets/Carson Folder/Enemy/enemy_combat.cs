using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_combat : MonoBehaviour
{
    public int damage = 1;
    public float KnockbackForce;
    public float stunTime;
    [Header("Damage on Touch")]
    public bool canTouchDamage = false; // New flag to control touch damage
    [Header("Laser Attack")]
    public Transform laserOrigin; // Origin point of the laser (where the laser starts)
    public GameObject laserPrefab; // The laser prefab to instantiate
    public float laserDuration = 0.5f; // How long the laser lasts
    public float laserDamage = 0.05f; // How much damage the laser deals
    [Header("Melee Attack")]
    public Transform attackPoint;
    public float weaponRange;
    public bool useSwordAttack = false;
    [Header("Projectile Attack")]
    public GameObject projectilePrefab; // Assign in Inspector
    public Transform projectileSpawnPoint; // Assign in Inspector
    public float projectileSpeed = 5f; // Adjust speed as needed

    public LayerMask playerLayer;

    AudioManager audioManager;
    private enemy_movement enemyMovement;
    private bool isAttacking = false; // Prevent multiple attacks at once

    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        enemyMovement = GetComponent<enemy_movement>(); // Get enemy's movement script
    }

    public void EnemyAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            if (useSwordAttack)
            {
                SwordAttack();
                isAttacking = false; // Reset flag only for sword attacks here
            }
            // Touch damage is handled in OnTriggerEnter2D and OnCollisionEnter2D
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
        // Touch damage logic is now in OnTriggerEnter2D and OnCollisionEnter2D
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canTouchDamage && !isAttacking && (playerLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            ApplyTouchDamage(collision);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canTouchDamage && !isAttacking && (playerLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            ApplyTouchDamage(collision.collider);
        }
    }

    private void ApplyTouchDamage(Collider2D otherCollider)
    {
        isAttacking = true;
        audioManager.PlaySFX(audioManager.playerHit);
        otherCollider.GetComponent<PlayerHealth>().ChangeHealth(-damage);
        otherCollider.GetComponent<PlayerController>().Knockback(transform, KnockbackForce, stunTime);
        Debug.Log("Touch Attack - Damage Applied");
        StartCoroutine(ResetAttackFlag());
    }

    private IEnumerator ResetAttackFlag()
    {
        yield return new WaitForSeconds(0.2f); // Adjust delay as needed
        isAttacking = false;
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

    public void LaserAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            // Create a ray from the enemy to the player
            GameObject player = GameObject.FindGameObjectWithTag("Player"); // Find the player
            if (player == null)
            {
                isAttacking = false;
                return;
            }

            // Show laser in the direction of the player
            GameObject laser = Instantiate(laserPrefab, laserOrigin.position, Quaternion.identity);
            Vector2 direction = (player.transform.position - laserOrigin.position).normalized;
            laser.transform.right = direction; // Make the laser face the player

            // Set laser range and start growing
            EnemyLaser enemyLaser = laser.GetComponent<EnemyLaser>();
            if (enemyLaser != null)
            {
                enemyLaser.UpdateLaserRange(Vector2.Distance(laserOrigin.position, player.transform.position), player.transform);
            }

            // Set laser duration and destroy it after some time
            Destroy(laser, laserDuration);

            // Optionally, play a sound or visual effect here
            Debug.Log("Laser Attack");

            // Make the enemy stay still while casting the laser (handled in movement script)

            // Reset attack flag after the laser duration
            Invoke(nameof(DelayedResetAttackFlag), laserDuration);
        }
    }

    private void DelayedResetAttackFlag()
    {
        isAttacking = false;
        if (enemyMovement != null)
        {
            enemyMovement.ChangeState(EnemyState.Chasing);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (useSwordAttack && attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
        }
        if (laserOrigin != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(laserOrigin.position, 0.1f); // Visualize laser origin
        }
        if (projectileSpawnPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(projectileSpawnPoint.position, 0.1f); // Visualize projectile spawn
        }
    }
}