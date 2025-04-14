using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kingManager : MonoBehaviour
{
    [Header("Boss Stats")]
    public float moveSpeed = 2f;
    public float detectionRange = 15f;

    [Header("Attack Properties")]
    public float attackCooldown = 3f;
    public float attackSelectionRange = 10f; // Range to select attacks from
    public Transform attackPoint;
    public LayerMask playerLayer;

    [Header("Tracking Ball Attack")]
    public GameObject trackingBallPrefab;
    public float trackingBallSpeed = 3f;
    public float trackingBallLifetime = 8f;
    public int trackingBallsCount = 3;
    public float trackingBallDelay = 0.5f;
    public float trackingBallAttackChance = 0.25f;

    [Header("Ground Slam Attack")]
    public float slamRadius = 5f;
    public float slamDamage = 2f;
    public float playerStunDuration = 1.5f;
    public GameObject slamEffectPrefab;
    public float groundSlamAttackChance = 0.25f;

    [Header("Staff Melee Attack")]
    public float staffAttackRange = 3f;
    public float staffDamage = 1f;
    public GameObject staffAttackEffectPrefab;
    public float staffMeleeAttackChance = 0.2f;

    [Header("Ring of Balls Attack")]
    public GameObject ballProjectilePrefab;
    public int ballsInRing = 8;
    public float ballSpeed = 5f;
    public float ballDamage = 1f;
    public float ballLifetime = 5f;
    public float ringOfBallsAttackChance = 0.15f;

    [Header("Beam Attack")]
    public GameObject beamPrefab;
    public float beamDuration = 2f;
    public float beamDamage = 0.5f; // Damage per tick
    public float beamDamageTickRate = 0.2f;
    public float beamChargeTime = 1f;
    public float beamWidth = 2f;
    public float beamAttackChance = 0.15f;
    
    [Header("References")]
    public GameObject beamWarningIndicatorPrefab;
    private GameObject currentBeamWarning = null;
    private GameObject currentBeam = null;
    private Vector2 currentBeamDirection = Vector2.right;

    private enum BossState { Idle, Moving, Attacking }
    private enum AttackType { TrackingBall, GroundSlam, StaffMelee, RingOfBalls, BeamAttack }

    private Animator animator;
    private Rigidbody2D rb;
    private EnemyHealth health;
    private Transform player;
    private BossState currentState;
    private float attackCooldownTimer;
    private AttackType nextAttack;
    private int facingDirection = 1;
    private bool isBeamActive = false;
    private bool playerInRange = false;
    private bool isRangedAttack = false;
    private bool needsMeleeRange = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<EnemyHealth>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = BossState.Idle;
        attackCooldownTimer = attackCooldown;
    }

    private void Update()
    {
        if (player == null)
            return;
            
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        playerInRange = distanceToPlayer <= detectionRange;
        
        if (playerInRange)
        {
            FacePlayer();
            
            // Select and perform attack if cooldown is ready
            if (attackCooldownTimer <= 0 && currentState != BossState.Attacking)
            {
                SelectAttack();
                StartCoroutine(PerformAttack());
            }
            else if (currentState != BossState.Attacking)
            {
                // Only move if not attacking
                HandleMovement();
            }
        }
        else
        {
            // No player in range, return to idle
            SetState(BossState.Idle);
            rb.velocity = Vector2.zero;
        }
        
        // Update the cooldown timer
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }

    private void HandleMovement()
    {
        if (player == null || currentState == BossState.Attacking || isBeamActive)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // Always in Moving state when player is in range and not attacking
        SetState(BossState.Moving);
        
        // Stop at staff attack range if waiting for cooldown
        if (distanceToPlayer <= staffAttackRange && attackCooldownTimer > 0)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            // Move toward player
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
    }

    private void FacePlayer()
    {
        if (player == null) return;

        if ((player.position.x > transform.position.x && facingDirection == -1) ||
            (player.position.x < transform.position.x && facingDirection == 1))
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void SelectAttack()
    {
        // Weight-based attack selection
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // Reset flags
        isRangedAttack = false;
        needsMeleeRange = false;
        
        // Create a weighted list of attacks
        List<AttackType> attackOptions = new List<AttackType>();
        List<float> attackWeights = new List<float>();
        
        // Close range - prioritize melee if in range
        if (distanceToPlayer <= staffAttackRange)
        {
            attackOptions.Add(AttackType.StaffMelee);
            attackWeights.Add(staffMeleeAttackChance * 2); // Double weight when in range
        }
        else 
        {
            // Still add melee attack but with normal weight
            attackOptions.Add(AttackType.StaffMelee);
            attackWeights.Add(staffMeleeAttackChance);
        }
        
        // Add all other attacks with their respective weights
        attackOptions.Add(AttackType.TrackingBall);
        attackWeights.Add(trackingBallAttackChance);
        
        attackOptions.Add(AttackType.GroundSlam);
        attackWeights.Add(groundSlamAttackChance);
        
        attackOptions.Add(AttackType.RingOfBalls);
        attackWeights.Add(ringOfBallsAttackChance);
        
        attackOptions.Add(AttackType.BeamAttack);
        attackWeights.Add(beamAttackChance);
        
        // Select attack based on weights
        float totalWeight = 0f;
        foreach (float weight in attackWeights)
        {
            totalWeight += weight;
        }
        
        float randomValue = Random.Range(0f, totalWeight);
        float weightSum = 0f;
        
        for (int i = 0; i < attackOptions.Count; i++)
        {
            weightSum += attackWeights[i];
            if (randomValue <= weightSum)
            {
                nextAttack = attackOptions[i];
                break;
            }
        }
        
        // Set attack type flags
        switch (nextAttack)
        {
            case AttackType.StaffMelee:
                isRangedAttack = false;
                needsMeleeRange = true;
                break;
            case AttackType.GroundSlam:
                isRangedAttack = false;
                needsMeleeRange = true;
                break;
            case AttackType.TrackingBall:
            case AttackType.RingOfBalls:
            case AttackType.BeamAttack:
                isRangedAttack = true;
                needsMeleeRange = false;
                break;
        }
    }

    private IEnumerator PerformAttack()
    {
        SetState(BossState.Attacking);
        
        // For melee attacks, move toward player until in range
        if (needsMeleeRange)
        {
            yield return StartCoroutine(MoveToMeleeRange());
        }
        
        // Stop moving while attacking
        rb.velocity = Vector2.zero;
        
        // Set attack animation trigger
        string animTrigger = "Attack" + nextAttack.ToString();
        animator.SetTrigger(animTrigger);
        
        // Perform the selected attack
        switch (nextAttack)
        {
            case AttackType.TrackingBall:
                yield return StartCoroutine(TrackingBallAttack());
                break;
            case AttackType.GroundSlam:
                yield return StartCoroutine(GroundSlamAttack());
                break;
            case AttackType.StaffMelee:
                yield return StartCoroutine(StaffMeleeAttack());
                break;
            case AttackType.RingOfBalls:
                yield return StartCoroutine(RingOfBallsAttack());
                break;
            case AttackType.BeamAttack:
                yield return StartCoroutine(BeamAttack());
                break;
        }
        
        // Reset attack cooldown
        attackCooldownTimer = attackCooldown;
        
        // Return to moving state if player still in range
        if (playerInRange)
        {
            SetState(BossState.Moving);
        }
        else
        {
            SetState(BossState.Idle);
        }
    }

    private IEnumerator MoveToMeleeRange()
    {
        // Only move if player isn't already in range
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        while (distanceToPlayer > staffAttackRange && player != null)
        {
            // Move toward player
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
            
            // Face the player
            FacePlayer();
            
            // Update distance
            distanceToPlayer = Vector2.Distance(transform.position, player.position);
            
            yield return null;
        }
        
        // Stop moving once in range
        rb.velocity = Vector2.zero;
    }

    private IEnumerator TrackingBallAttack()
    {
        // Animation buildup
        yield return new WaitForSeconds(0.8f);
        
        // Spawn tracking balls
        for (int i = 0; i < trackingBallsCount; i++)
        {
            if (player != null)
            {
                GameObject trackingBall = Instantiate(trackingBallPrefab, attackPoint.position, Quaternion.identity);
                TrackingBallScript ballController = trackingBall.GetComponent<TrackingBallScript>();
                if (ballController != null)
                {
                    ballController.Initialize(player, trackingBallSpeed, trackingBallLifetime);
                }
            }
            
            yield return new WaitForSeconds(trackingBallDelay);
        }
        
        // Animation recovery
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator GroundSlamAttack()
    {
        // Animation buildup
        yield return new WaitForSeconds(1.0f);
        
        // Create slam effect
        if (slamEffectPrefab != null)
        {
            GameObject slamEffect = Instantiate(slamEffectPrefab, transform.position, Quaternion.identity);
            slamEffect.transform.localScale = new Vector3(slamRadius * 2, slamRadius * 2, 1);
            Destroy(slamEffect, 2f);
        }
        
        // Check for player in slam radius
        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, slamRadius, playerLayer);
        if (hitPlayer != null)
        {
            // Apply damage to player
            PlayerHealth playerHealth = hitPlayer.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ChangeHealth(-slamDamage);
            }
            
            // Stun player - you'll need to implement this in your player controller
            PlayerController playerController = hitPlayer.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // Implement stun functionality in your player controller
                //playerController.StunPlayer(playerStunDuration);
            }
        }
        
        // Animation recovery
        yield return new WaitForSeconds(1.0f);
    }

    private IEnumerator StaffMeleeAttack()
    {
        // Animation buildup
        yield return new WaitForSeconds(0.5f);
        
        // Apply damage in attack arc
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, staffAttackRange, playerLayer);
        if (hitPlayer != null)
        {
            PlayerHealth playerHealth = hitPlayer.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ChangeHealth(-staffDamage);
            }
            
            // Apply knockback if you have a knockback component
            Knockback knockback = hitPlayer.GetComponent<Knockback>();
            if (knockback != null)
            {
                Vector2 knockbackDirection = (hitPlayer.transform.position - transform.position).normalized;
                //PlayerController.knockback(knockbackDirection, 10f);
            }
        }
        
        // Staff attack effect
        if (staffAttackEffectPrefab != null)
        {
            GameObject effect = Instantiate(staffAttackEffectPrefab, attackPoint.position, Quaternion.identity);
            // Adjust rotation to face the direction of attack
            effect.transform.right = facingDirection == 1 ? Vector2.right : Vector2.left;
            Destroy(effect, 0.5f);
        }
        
        // Animation recovery
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator RingOfBallsAttack()
    {
        // Animation buildup
        yield return new WaitForSeconds(1.0f);
        
        // Create ring of balls
        float angleStep = 360f / ballsInRing;
        for (int i = 0; i < ballsInRing; i++)
        {
            float angle = i * angleStep;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            
            GameObject ball = Instantiate(ballProjectilePrefab, transform.position, Quaternion.identity);
            ProjectileController projectile = ball.GetComponent<ProjectileController>();
            if (projectile != null)
            {
                projectile.Initialize(direction, ballSpeed, ballDamage, ballLifetime);
            }
        }
        
        // Animation recovery
        yield return new WaitForSeconds(0.8f);
    }

    private IEnumerator BeamAttack()
    {
        isBeamActive = true;
        
        // Face player
        FacePlayer();
        
        // Animation buildup and warning indicator
        GameObject warningIndicator = null;
        if (beamWarningIndicatorPrefab != null && player != null)
        {
            // Calculate direction to player
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            
            // Create warning indicator
            warningIndicator = Instantiate(beamWarningIndicatorPrefab, transform.position, Quaternion.identity);
            warningIndicator.transform.up = directionToPlayer;
            
            // Scale the indicator to match beam length
            float indicatorLength = detectionRange * 2;
            warningIndicator.transform.localScale = new Vector3(beamWidth, indicatorLength, 1);
        }
        
        // Charge time
        yield return new WaitForSeconds(beamChargeTime);
        
        // Remove warning indicator
        if (warningIndicator != null)
        {
            Destroy(warningIndicator);
        }
        
        // Create beam
        GameObject beam = null;
        if (beamPrefab != null && player != null)
        {
            // Get direction to player at time of firing
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            
            // Create the beam
            beam = Instantiate(beamPrefab, attackPoint.position, Quaternion.identity);
            beam.transform.up = directionToPlayer;
            
            // Set beam properties
            BeamController beamController = beam.GetComponent<BeamController>();
            if (beamController != null)
            {
                beamController.Initialize(transform, directionToPlayer, beamWidth, beamDuration, beamDamage, beamDamageTickRate, playerLayer);
            }
        }
        
        // Active beam duration
        yield return new WaitForSeconds(beamDuration);
        
        // Clean up beam if it's still there
        if (beam != null)
        {
            Destroy(beam);
        }
        
        // Animation recovery
        yield return new WaitForSeconds(0.8f);
        
        isBeamActive = false;
    }

    private void SetState(BossState newState)
    {
        if (currentState == newState)
            return;
            
        currentState = newState;
        
        // Update animator parameters
        animator.SetBool("isIdle", currentState == BossState.Idle);
        animator.SetBool("isMoving", currentState == BossState.Moving);
        animator.SetBool("isAttacking", currentState == BossState.Attacking);
    }

    // Called from animation events to deal damage
    public void DealMeleeDamage()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, staffAttackRange, playerLayer);
        if (hitPlayer != null)
        {
            PlayerHealth playerHealth = hitPlayer.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ChangeHealth(-staffDamage);
            }
        }
    }

    // This is useful for testing and visualizing attack ranges
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
            
        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Draw slam attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, slamRadius);
        
        // Draw staff attack range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, staffAttackRange);
    }

    // Animation event handlers
    public void AnimEvent_FireTrackingBall()
    {
        if (player != null)
        {
            GameObject trackingBall = Instantiate(trackingBallPrefab, attackPoint.position, Quaternion.identity);
            TrackingBallScript ballController = trackingBall.GetComponent<TrackingBallScript>();
            if (ballController != null)
            {
                ballController.Initialize(player, trackingBallSpeed, trackingBallLifetime);
            }
        }
    }

    public void AnimEvent_GroundSlamImpact()
    {
        // Create slam effect
        if (slamEffectPrefab != null)
        {
            GameObject slamEffect = Instantiate(slamEffectPrefab, transform.position, Quaternion.identity);
            slamEffect.transform.localScale = new Vector3(slamRadius * 2, slamRadius * 2, 1);
            Destroy(slamEffect, 2f);
        }
        
        // Check for player in slam radius and damage them
        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, slamRadius, playerLayer);
        if (hitPlayer != null)
        {
            PlayerHealth playerHealth = hitPlayer.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ChangeHealth(-slamDamage);
            }
            
            // Stun player
            PlayerController playerController = hitPlayer.GetComponent<PlayerController>();
            if (playerController != null)
            {
                //playerController.StunPlayer(playerStunDuration);
            }
        }
    }

    public void AnimEvent_StaffStrike()
    {
        // Apply damage in attack arc
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, staffAttackRange, playerLayer);
        if (hitPlayer != null)
        {
            PlayerHealth playerHealth = hitPlayer.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ChangeHealth(-staffDamage);
            }
            
            // Apply knockback
            Knockback playerKnockback = hitPlayer.GetComponent<Knockback>();
            if (playerKnockback != null)
            {
                Vector2 knockbackDirection = (hitPlayer.transform.position - transform.position).normalized;
                //playerKnockback.GetKnockedBack(knockbackDirection, 10f);
            }
        }
        
        // Staff attack effect
        if (staffAttackEffectPrefab != null)
        {
            GameObject effect = Instantiate(staffAttackEffectPrefab, attackPoint.position, Quaternion.identity);
            effect.transform.right = facingDirection == 1 ? Vector2.right : Vector2.left;
            Destroy(effect, 0.5f);
        }
    }

    public void AnimEvent_FireRingOfBalls()
    {
        // Create ring of balls
        float angleStep = 360f / ballsInRing;
        for (int i = 0; i < ballsInRing; i++)
        {
            float angle = i * angleStep;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            
            GameObject ball = Instantiate(ballProjectilePrefab, transform.position, Quaternion.identity);
            ProjectileController projectile = ball.GetComponent<ProjectileController>();
            if (projectile != null)
            {
                projectile.Initialize(direction, ballSpeed, ballDamage, ballLifetime);
            }
        }
    }

    public void AnimEvent_BeamChargeStart()
    {
        // Face player
        FacePlayer();
        
        // Create warning indicator
        if (beamWarningIndicatorPrefab != null && player != null)
        {
            // Calculate direction to player
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            
            // Store direction for firing
            currentBeamDirection = directionToPlayer;
            
            // Create warning indicator
            GameObject indicator = Instantiate(beamWarningIndicatorPrefab, transform.position, Quaternion.identity);
            currentBeamWarning = indicator;
            indicator.transform.up = directionToPlayer;
            
            // Scale the indicator to match beam length
            float indicatorLength = detectionRange * 2;
            indicator.transform.localScale = new Vector3(beamWidth, indicatorLength, 1);
        }
    }

    public void AnimEvent_BeamFire()
    {
        // Clean up warning indicator
        if (currentBeamWarning != null)
        {
            Destroy(currentBeamWarning);
            currentBeamWarning = null;
        }
        
        // Create beam
        if (beamPrefab != null)
        {
            // Create the beam
            GameObject beam = Instantiate(beamPrefab, attackPoint.position, Quaternion.identity);
            currentBeam = beam;
            beam.transform.up = currentBeamDirection;
            
            // Set beam properties
            BeamController beamController = beam.GetComponent<BeamController>();
            if (beamController != null)
            {
                beamController.Initialize(transform, currentBeamDirection, beamWidth, beamDuration, beamDamage, beamDamageTickRate, playerLayer);
            }
        }
        
        // Start beam duration timer
        StartCoroutine(BeamDurationCoroutine());
    }

    public void AnimEvent_BeamEnd()
    {
        // Clean up beam if it's still there
        if (currentBeam != null)
        {
            Destroy(currentBeam);
            currentBeam = null;
        }
    }

    private IEnumerator BeamDurationCoroutine()
    {
        yield return new WaitForSeconds(beamDuration);
        AnimEvent_BeamEnd();
    }
}