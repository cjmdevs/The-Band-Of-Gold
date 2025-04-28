using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_movement : MonoBehaviour
{
    [Header("Enemy Stats")]
    public bool isRangedEnemy = false;
    public bool isBoss = false; // Flag to identify boss enemy
    public float speed = 3f;
    public float playerDetectRange = 5f;
    public float attackRange = 2f; // Melee attack range
    public float attackCooldown = 2f;
    public float separationRadius = 2f;
    public float separationForce = 1f;
    public float rangedAttackRange = 4f;

    public Transform detectionPoint;
    public LayerMask playerLayer;

    public EnemyState enemyState; // Changed to public
    private Transform player;
    private int facingDirection = -1;
    private Animator anim;
    private float attackCooldownTimer;
    private Rigidbody2D rb;
    private enemy_combat enemyCombat;
    private bool playerInAttackRange; // Track if player is currently in attack range

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemyCombat = GetComponent<enemy_combat>(); // Get enemy's combat script
        
        // Set initial orientation for boss (flipped 180° from normal enemies)
        if (isBoss)
        {
            // Start with boss rotated 180 degrees by default
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        
        ChangeState(EnemyState.Idle);
    }

    void Update()
    {
        if (enemyState != EnemyState.Knockback)
        {
            CheckForPlayer();

            if (attackCooldownTimer > 0)
            {
                attackCooldownTimer -= Time.deltaTime;
            }

            if (enemyState == EnemyState.Chasing)
            {
                Chase();
            }
            else if (enemyState == EnemyState.Attacking)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    void Chase()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Check if we need to flip the enemy based on player position
        if (player.position.x > transform.position.x && facingDirection == -1 ||
            player.position.x < transform.position.x && facingDirection == 1)
        {
            Flip();
        }

        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 separation = CalculateSeparation();

        if (isRangedEnemy && distanceToPlayer <= rangedAttackRange)
        {
            // If ranged enemy, stop moving and prepare to shoot
            rb.velocity = Vector2.zero;

            if (attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldown;
                enemyCombat.LaserAttack(); // Shoot laser
                ChangeState(EnemyState.Attacking);
            }
        }
        else
        {
            // Melee enemies keep chasing or ranged enemies move closer if out of range
            rb.velocity = (direction + separation) * speed;
        }
    }

    Vector2 CalculateSeparation()
    {
        Vector2 separation = Vector2.zero;
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, separationRadius, LayerMask.GetMask("Enemy"));

        foreach (Collider2D enemy in nearbyEnemies)
        {
            if (enemy.gameObject != gameObject)
            {
                Vector2 away = transform.position - enemy.transform.position;
                separation += away.normalized;
            }
        }

        return separation * separationForce;
    }

    void Flip()
    {
        facingDirection *= -1;
        
        if (isBoss)
        {
            // For boss, use rotation to flip (180 degree rotations)
            transform.rotation = Quaternion.Euler(0, facingDirection == 1 ? 0 : 180, 0);
        }
        else
        {
            // For regular enemies, use scale as before
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    private void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerLayer);

        if (hits.Length > 0)
        {
            player = hits[0].transform;
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            if (isRangedEnemy)
            {
                // Ranged Enemy Logic
                playerInAttackRange = distanceToPlayer <= rangedAttackRange;

                if (playerInAttackRange && attackCooldownTimer <= 0 && enemyState != EnemyState.Attacking)
                {
                    attackCooldownTimer = attackCooldown;
                    enemyCombat.LaserAttack();
                    ChangeState(EnemyState.Attacking);
                }
                else if (!playerInAttackRange && enemyState != EnemyState.Attacking)
                {
                    ChangeState(EnemyState.Chasing);
                }
            }
            else
            {
                // Melee Enemy Logic
                playerInAttackRange = distanceToPlayer <= attackRange;

                if (playerInAttackRange && attackCooldownTimer <= 0 && enemyState != EnemyState.Attacking)
                {
                    attackCooldownTimer = attackCooldown;
                    ChangeState(EnemyState.Attacking);
                    enemyCombat.EnemyAttack(); // Perform melee attack
                }
                else if (!playerInAttackRange && enemyState != EnemyState.Attacking)
                {
                    ChangeState(EnemyState.Chasing);
                }
            }
        }
        else
        {
            ChangeState(EnemyState.Idle);
            rb.velocity = Vector2.zero;
            player = null; // Clear the player reference when out of detection range
            playerInAttackRange = false;
        }
    }

    public void ChangeState(EnemyState newState)
    {
        if (enemyState == EnemyState.Idle) anim.SetBool("isIdle", false);
        else if (enemyState == EnemyState.Chasing) anim.SetBool("isChasing", false);
        else if (enemyState == EnemyState.Attacking) anim.SetBool("isAttacking", false);

        enemyState = newState;

        if (enemyState == EnemyState.Idle) anim.SetBool("isIdle", true);
        else if (enemyState == EnemyState.Chasing) anim.SetBool("isChasing", true);
        else if (enemyState == EnemyState.Attacking) anim.SetBool("isAttacking", true);
    }

    private void OnDrawGizmosSelected()
    {
        if (detectionPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(detectionPoint.position, playerDetectRange);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
        if (isRangedEnemy)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, rangedAttackRange);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
    Knockback
}