using UnityEngine;
using System.Collections;

public class MinoController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float maxMoveSpeed = 6f; // Cap for speed increase
    public float speedIncreaseRate = 0.1f; // Rate of speed increase
    private float currentMoveSpeed; // Current move speed that increases gradually
    public Transform player;
    private Animator animator;
    private Vector2 movement;
    public GameObject enemyPrefab;
    public Transform summonPoint;
    public float attackRange = 5f;
    public float attackCooldown = 2f;
    private float nextAttackTime = 0f;
    public int regularAttackDamage = 10;
    public int quickAttackDamage = 5;
    public int heavyAttackDamage = 20;
    public float heavyAttackKnockback = 5f;
    public float heavyAttackStun = 1f;
    public LayerMask playerLayer;
    public Transform attackPoint;
    public float followRange = 10f;
    public float playerDetectRange = 3f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public GameObject rageParticles;
    public float rageThreshold = 0.5f;
    public Vector3 particleOffset = new Vector3(1f, 0f, 0f);

    private int facingDirection = -1;
    private Rigidbody2D rb;
    private bool isAttacking = false;
    private bool isDashing = false;
    private bool isInRageMode = false;
    private EnemyHealth enemyHealth;
    private GameObject currentRageParticles;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentMoveSpeed = moveSpeed;
        enemyHealth = GetComponent<EnemyHealth>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }

    private void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= followRange && !isAttacking)
            {
                // ✅ Gradually increase movement speed until it reaches maxMoveSpeed
                currentMoveSpeed = Mathf.Min(currentMoveSpeed + (speedIncreaseRate * Time.deltaTime), maxMoveSpeed);

                Vector2 direction = (player.position - transform.position).normalized;
                movement = direction * currentMoveSpeed;

                if (movement.magnitude > 0.1f)
                {
                    animator.SetBool("IsMoving", true);
                    // ✅ Use currentMoveSpeed directly for the animation value
                    animator.SetFloat("MovementSpeed", currentMoveSpeed);
                }
                else
                {
                    animator.SetBool("IsMoving", false);
                    animator.SetFloat("MovementSpeed", moveSpeed);
                }

                // ✅ Flip the boss to face the player
                if ((player.position.x > transform.position.x && facingDirection == -1) ||
                    (player.position.x < transform.position.x && facingDirection == 1))
                {
                    Flip();
                }

                // ✅ Attack if within detect range
                if (distanceToPlayer <= playerDetectRange && Time.time >= nextAttackTime)
                {
                    ChooseAndPerformAttack();
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
            else
            {
                // ✅ Stop moving if out of range
                animator.SetBool("IsMoving", false);
                animator.SetFloat("MovementSpeed", moveSpeed);
                rb.velocity = Vector2.zero;
                ResetMoveSpeed(); // Reset speed when idle
            }

            // ✅ Check health to activate rage mode
            CheckHealth();
        }
    }

    private void FixedUpdate()
    {
        if (!isAttacking && !isDashing)
        {
            rb.velocity = movement;
        }
        else if (isDashing)
        {
            rb.velocity = new Vector2(facingDirection * dashSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private bool IsPlayerInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        return hits.Length > 0;
    }

    private void ChooseAndPerformAttack()
    {
        if (IsPlayerInRange())
        {
            int regularAttackChance = 60;
            int quickAttackChance = 25;
            int summonAttackChance = 5;
            int heavyAttackChance = 10;

            int totalChance = regularAttackChance + quickAttackChance + summonAttackChance + heavyAttackChance;
            int randomValue = Random.Range(0, totalChance);

            if (randomValue < regularAttackChance)
            {
                PerformAttack(1);
            }
            else if (randomValue < regularAttackChance + quickAttackChance)
            {
                PerformAttack(2);
            }
            else if (randomValue < regularAttackChance + quickAttackChance + summonAttackChance)
            {
                PerformAttack(3);
            }
            else
            {
                PerformAttack(4);
            }
        }
    }

    public void PerformAttack(int attackNumber)
    {
        isAttacking = true;
        animator.SetInteger("AttackTrigger", attackNumber);
        ResetMoveSpeed(); // ✅ Reset speed after attacking

        if (attackNumber == 2)
        {
            StartCoroutine(DashForward());
        }
    }

    private IEnumerator DashForward()
    {
        isDashing = true;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }

    public void MinoAttack1Function()
    {
        DealDamage(regularAttackDamage, heavyAttackKnockback, heavyAttackStun);
    }

    public void MinoAttack2Function()
    {
        DealDamage(quickAttackDamage, heavyAttackKnockback, heavyAttackStun);
    }

    public void MinoAttack3Function()
    {
        if (enemyPrefab != null && summonPoint != null)
        {
            Instantiate(enemyPrefab, summonPoint.position, Quaternion.identity);
        }
    }

    public void MinoAttack4Function()
    {
        DealDamage(heavyAttackDamage, heavyAttackKnockback, heavyAttackStun);
    }

    private void DealDamage(int damage, float knockback = 0f, float stun = 0f)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
                PlayerController playerController = hit.GetComponent<PlayerController>();
                if (playerHealth != null)
                {
                    playerHealth.ChangeHealth(-damage);
                }
                if (playerController != null && knockback > 0)
                {
                    playerController.Knockback(transform, knockback, stun);
                }
            }
        }
    }

    public void ResetAttackTrigger()
    {
        animator.SetInteger("AttackTrigger", 0);
        isAttacking = false;
        ResetMoveSpeed();
    }

    private void ResetMoveSpeed()
    {
        currentMoveSpeed = moveSpeed;
        animator.SetFloat("MovementSpeed", moveSpeed); // ✅ Keep it consistent
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void CheckHealth()
    {
        float currentHealth = enemyHealth.currentHealth;
        float maxHealth = enemyHealth.startingHealth;
        if (!isInRageMode && currentHealth <= maxHealth * rageThreshold)
        {
            ActivateRageMode();
        }
    }

    private void ActivateRageMode()
    {
        isInRageMode = true;
        moveSpeed *= 1.5f;
        maxMoveSpeed *= 1.5f;
        speedIncreaseRate *= 1.2f;

        if (rageParticles != null)
        {
            Vector3 particlePosition = transform.position + particleOffset;
            currentRageParticles = Instantiate(rageParticles, particlePosition, Quaternion.identity, transform);
        }
    }
}
