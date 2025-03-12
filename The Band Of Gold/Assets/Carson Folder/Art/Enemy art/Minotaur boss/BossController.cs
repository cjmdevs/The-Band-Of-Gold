using UnityEngine;

public class BossController : MonoBehaviour
{
    public float moveSpeed = 3f;
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
    public LayerMask playerLayer; // Player layer for range check
    public Transform attackPoint; // Point from which to check attack range

    void Start()
    {
        Debug.Log("BossController Start");
        animator = GetComponent<Animator>();
        Debug.Log("Animator component assigned: " + (animator != null));
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            Debug.Log("Player found by tag: " + (player != null));
        }
        Debug.Log("Player variable assigned: " + (player != null));
        Debug.Log("Attack point assigned: " + (attackPoint != null));
        Debug.Log("Player layer assigned: " + playerLayer.value);
    }

    void Update()
    {
        Debug.Log("BossController Update");
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            movement = direction * moveSpeed;
            if (movement.magnitude > 0.1f)
            {
                animator.SetBool("IsMoving", true);
                animator.SetFloat("MovementSpeed", movement.magnitude);
                Debug.Log("Boss is moving");
            }
            else
            {
                animator.SetBool("IsMoving", false);
                Debug.Log("Boss is idle");
            }

            if (Time.time >= nextAttackTime)
            {
                Debug.Log("Time for attack check");
                if (IsPlayerInRange())
                {
                    Debug.Log("Player in range");
                    ChooseAndPerformAttack();
                    nextAttackTime = Time.time + attackCooldown;
                    Debug.Log("Next attack time set to: " + nextAttackTime);
                }
                else
                {
                    Debug.Log("Player out of range");
                }
            }
        }
        else
        {
            Debug.Log("Player variable is null");
        }
    }

    void FixedUpdate()
    {
        Debug.Log("BossController FixedUpdate");
        GetComponent<Rigidbody2D>().velocity = movement;
    }

    bool IsPlayerInRange()
    {
        Debug.Log("Checking if player is in range");
        if (attackPoint == null) {
            Debug.LogError("Attack point is null, cannot check range.");
            return false;
        }
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        bool inRange = hits.Length > 0;
        Debug.Log("Player in range: " + inRange);
        return inRange;
    }

    void ChooseAndPerformAttack()
    {
        Debug.Log("Choosing and performing attack");
        int regularAttackChance = 60;
        int quickAttackChance = 25;
        int summonAttackChance = 10;
        int heavyAttackChance = 5;

        int totalChance = regularAttackChance + quickAttackChance + summonAttackChance + heavyAttackChance;
        int randomValue = Random.Range(0, totalChance);

        if (randomValue < regularAttackChance)
        {
            Debug.Log("Performing regular attack");
            PerformAttack(1);
        }
        else if (randomValue < regularAttackChance + quickAttackChance)
        {
            Debug.Log("Performing quick attack");
            PerformAttack(2);
        }
        else if (randomValue < regularAttackChance + quickAttackChance + summonAttackChance)
        {
            Debug.Log("Performing summon attack");
            PerformAttack(3);
        }
        else
        {
            Debug.Log("Performing heavy attack");
            PerformAttack(4);
        }
    }

    public void PerformAttack(int attackNumber)
    {
        Debug.Log("PerformAttack called with attackNumber: " + attackNumber);
        animator.SetInteger("AttackTrigger", attackNumber);
        Debug.Log("AttackTrigger set to: " + attackNumber);
    }

    public void MinoAttack1Function()
    {
        Debug.Log("Regular Attack! (MinoAttack1Function)");
        DealDamage(regularAttackDamage);
    }

    public void MinoAttack2Function()
    {
        Debug.Log("Quick Attack! (MinoAttack2Function)");
        DealDamage(quickAttackDamage);
    }

    public void MinoAttack3Function()
    {
        Debug.Log("Special Summon Attack! (MinoAttack3Function)");
        if (enemyPrefab != null && summonPoint != null)
        {
            Instantiate(enemyPrefab, summonPoint.position, Quaternion.identity);
            Debug.Log("Enemy summoned at: " + summonPoint.position);
        }
        else
        {
            Debug.Log("Enemy prefab or summon point is null");
        }
        //Add projectile attack logic here if you want a projectile on summon.
    }

    public void MinoAttack4Function()
    {
        Debug.Log("Heavy Attack! (MinoAttack4Function)");
        DealDamage(heavyAttackDamage, heavyAttackKnockback, heavyAttackStun);
    }

    public void ResetAttackTrigger()
    {
        Debug.Log("ResetAttackTrigger called");
        animator.SetInteger("AttackTrigger", 0);
        Debug.Log("AttackTrigger reset to 0");
    }

    private void DealDamage(int damage, float knockback = 0f, float stun = 0f)
    {
        Debug.Log("Dealing damage: " + damage + ", knockback: " + knockback + ", stun: " + stun);
        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerHealth != null)
            {
                playerHealth.ChangeHealth(-damage);
                Debug.Log("Player health changed by: " + -damage);
            }
            else
            {
                Debug.Log("PlayerHealth component not found");
            }
            if (playerController != null && knockback > 0f)
            {
                playerController.Knockback(transform, knockback, stun);
                Debug.Log("Player knocked back and stunned");
            }
            else
            {
                Debug.Log("PlayerController component not found or knockback is 0");
            }
        }
        else
        {
            Debug.Log("Player variable is null, cannot deal damage");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}