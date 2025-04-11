using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kingManager : MonoBehaviour
{
    [Header("Boss Stats")]
    public float moveSpeed = 2f;
    public float detectionRange = 15f;
    public float phaseChangeThreshold = 0.5f; // Boss enters phase 2 at 50% health

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

    [Header("Ground Slam Attack")]
    public float slamRadius = 5f;
    public float slamDamage = 2f;
    public float playerStunDuration = 1.5f;
    public GameObject slamEffectPrefab;

    [Header("Staff Melee Attack")]
    public float staffAttackRange = 3f;
    public float staffDamage = 1f;
    public GameObject staffAttackEffectPrefab;

    [Header("Ring of Balls Attack")]
    public GameObject ballProjectilePrefab;
    public int ballsInRing = 8;
    public float ballSpeed = 5f;
    public float ballDamage = 1f;
    public float ballLifetime = 5f;

    [Header("Beam Attack")]
    public GameObject beamPrefab;
    public float beamDuration = 2f;
    public float beamDamage = 0.5f; // Damage per tick
    public float beamDamageTickRate = 0.2f;
    public float beamChargeTime = 1f;
    public float beamWidth = 2f;
    
    [Header("References")]
    public GameObject beamWarningIndicatorPrefab;
    private GameObject currentBeamWarning = null;
    private GameObject currentBeam = null;
    private Vector2 currentBeamDirection = Vector2.right;

    // Add these to your existing Header sections
    [Header("Attack Effects")]
    public GameObject trackingBallChargePrefab;
    public GameObject staffChargeEffectPrefab;
    public GameObject ringChargeEffectPrefab;
    public GameObject phaseChangeEffectPrefab;


    private enum BossState { Idle, Moving, Attacking, Stunned }
    private enum BossPhase { Phase1, Phase2 }
    private enum AttackType { TrackingBall, GroundSlam, StaffMelee, RingOfBalls, BeamAttack }

    private Animator animator;
    private Rigidbody2D rb;
    private EnemyHealth health;
    private Transform player;
    private BossState currentState;
    private BossPhase currentPhase;
    private float attackCooldownTimer;
    private AttackType nextAttack;
    private int facingDirection = 1;
    private float startingHealth;
    private bool isBeamActive = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<EnemyHealth>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startingHealth = health.startingHealth;
        currentState = BossState.Idle;
        currentPhase = BossPhase.Phase1;
        attackCooldownTimer = attackCooldown;
    }

    private void Update()
    {
        if (currentState == BossState.Stunned)
            return;

        // Check for phase change
        if (currentPhase == BossPhase.Phase1 && health.currentHealth / startingHealth <= phaseChangeThreshold)
        {
            TransitionToPhase2();
        }

        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        // Find player in detection range
        if (player != null && Vector2.Distance(transform.position, player.position) <= detectionRange)
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
                // Move toward player when not attacking
                MoveTowardPlayer();
            }
        }
        else
        {
            // No player in range, return to idle
            SetState(BossState.Idle);
        }
    }

    private void MoveTowardPlayer()
    {
        if (player == null || currentState == BossState.Attacking || isBeamActive)
            return;

        SetState(BossState.Moving);
        
        // Don't move too close to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > staffAttackRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
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
        // Select a random attack with weighting based on phase
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        List<AttackType> availableAttacks = new List<AttackType>();

        // Phase 1 attacks
        if (distanceToPlayer <= staffAttackRange)
        {
            availableAttacks.Add(AttackType.StaffMelee);
        }
        availableAttacks.Add(AttackType.TrackingBall);
        availableAttacks.Add(AttackType.GroundSlam);

        // Phase 2 adds more attacks
        if (currentPhase == BossPhase.Phase2)
        {
            availableAttacks.Add(AttackType.RingOfBalls);
            availableAttacks.Add(AttackType.BeamAttack);
            
            // Add TrackingBall and GroundSlam again to increase their probability in phase 2
            if (distanceToPlayer > staffAttackRange)
            {
                availableAttacks.Add(AttackType.TrackingBall);
                availableAttacks.Add(AttackType.GroundSlam);
            }
        }

        // Select a random attack from available attacks
        nextAttack = availableAttacks[Random.Range(0, availableAttacks.Count)];
    }

    private IEnumerator PerformAttack()
    {
        SetState(BossState.Attacking);
        
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
        
        // Return to idle/moving state
        if (player != null && Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            SetState(BossState.Moving);
        }
        else
        {
            SetState(BossState.Idle);
        }
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
            // Apply damage and stun to player
            PlayerHealth playerHealth = hitPlayer.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ChangeHealth(slamDamage);
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

    private void TransitionToPhase2()
    {
        currentPhase = BossPhase.Phase2;
        
        // Visual indication of phase change
        animator.SetTrigger("PhaseChange");
        
        // Adjust attack cooldown for phase 2
        attackCooldown *= 0.8f; // Reduce cooldown by 20%
        
        // Increase movement speed
        moveSpeed *= 1.2f; // Increase speed by 20%
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
        animator.SetBool("isStunned", currentState == BossState.Stunned);
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
    public void AnimEvent_StartTrackingBallAttack()
    {
        // Visual or sound effects for charging up the attack
        // Example: instantiate a charging effect
        if (trackingBallChargePrefab != null)
        {
            GameObject chargeEffect = Instantiate(trackingBallChargePrefab, attackPoint.position, Quaternion.identity);
            chargeEffect.transform.parent = attackPoint;
            Destroy(chargeEffect, 0.8f); // Destroy after animation completes
        }
    }

    // Call this from animation event when boss should spawn a tracking ball
    public void AnimEvent_FireTrackingBall()
    {
        if (player != null)
        {
            GameObject trackingBall = Instantiate(trackingBallPrefab, attackPoint.position, Quaternion.identity);
            TrackingBallController ballController = trackingBall.GetComponent<TrackingBallController>();
            if (ballController != null)
            {
                ballController.Initialize(player, trackingBallSpeed, trackingBallLifetime);
            }
        }
    }

    // GROUND SLAM ATTACK EVENTS
    // Call this from animation event when boss starts the ground slam
    public void AnimEvent_StartGroundSlam()
    {
        // Boss jumps or prepares for slam
        // Add effects or screen shake anticipation
        rb.velocity = new Vector2(0, 5f); // Optional: small jump before slam
    }

    // Call this from animation event when boss hits the ground
    public void AnimEvent_GroundSlamImpact()
    {
        // Create slam effect
        if (slamEffectPrefab != null)
        {
            GameObject slamEffect = Instantiate(slamEffectPrefab, transform.position, Quaternion.identity);
            slamEffect.transform.localScale = new Vector3(slamRadius * 2, slamRadius * 2, 1);
            Destroy(slamEffect, 2f);
        }
        
        // Camera shake effect - implement your own camera shake method or use Cinemachine
        CameraShake();
        
        // Check for player in slam radius and damage them
        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, slamRadius, playerLayer);
        if (hitPlayer != null)
        {
            PlayerHealth playerHealth = hitPlayer.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(slamDamage);
            }
            
            // Stun player
            PlayerController playerController = hitPlayer.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.StunPlayer(playerStunDuration);
            }
        }
    }

    // STAFF MELEE ATTACK EVENTS
    // Call this from animation event during windup
    public void AnimEvent_StaffWindup()
    {
        // Visual effect for staff charging
        if (staffChargeEffectPrefab != null)
        {
            GameObject chargeEffect = Instantiate(staffChargeEffectPrefab, attackPoint.position, Quaternion.identity);
            chargeEffect.transform.parent = attackPoint;
            Destroy(chargeEffect, 0.5f);
        }
    }

    // Call this from animation event at the moment of impact
    public void AnimEvent_StaffStrike()
    {
        // Apply damage in attack arc
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, staffAttackRange, playerLayer);
        if (hitPlayer != null)
        {
            PlayerHealth playerHealth = hitPlayer.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(staffDamage);
            }
            
            // Apply knockback
            Knockback playerKnockback = hitPlayer.GetComponent<Knockback>();
            if (playerKnockback != null)
            {
                Vector2 knockbackDirection = (hitPlayer.transform.position - transform.position).normalized;
                playerKnockback.GetKnockedBack(knockbackDirection, 10f);
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

    // RING OF BALLS ATTACK EVENTS
    // Call this from animation at the start of ring attack
    public void AnimEvent_StartRingOfBalls()
    {
        // Visual buildup effect
        if (ringChargeEffectPrefab != null)
        {
            GameObject chargeEffect = Instantiate(ringChargeEffectPrefab, transform.position, Quaternion.identity);
            chargeEffect.transform.parent = transform;
            Destroy(chargeEffect, 1.0f);
        }
    }

    // Call this from animation when the boss should release the ring of balls
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

    // BEAM ATTACK EVENTS
    // Call this from animation when starting beam charge
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

    // Call this from animation when the beam should fire
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

    // Call this from animation when beam should end (could be called from coroutine instead)
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

    // PHASE CHANGE EVENTS
    // Call this from phase change animation at the key moment
    public void AnimEvent_PhaseChangeEffect()
    {
        // Spawn phase change effect
        if (phaseChangeEffectPrefab != null)
        {
            GameObject effect = Instantiate(phaseChangeEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 3f);
        }
        
        // Add screen shake
        CameraShake();
        
        // Adjust stats for phase 2
        attackCooldown *= 0.8f;
        moveSpeed *= 1.2f;
    }

}