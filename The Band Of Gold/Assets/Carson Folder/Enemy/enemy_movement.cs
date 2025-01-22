using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class enemy_movement : MonoBehaviour
{   
    [Header("Enemy Stats")]
    public float speed; // speed of the enemy
    public float playerDetectRange = 5; // range at which the enemy can detect the player dont need capsule collidor I believe
    public float attackRange = 2; // range at which the enemy can attack the player
    public float attackCooldown = 2; // time before the enemy can attack again

    public Transform detectionPoint; 
    public LayerMask playerLayer; 


    private EnemyState enemyState;
    private Transform player;
    private int facingDirection = -1;
    private Animator anim;
    private float attackCooldownTimer;

    private Rigidbody2D rb;

    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyState != EnemyState.Knockback){

        

            CheckForPlayer();

            if(attackCooldownTimer > 0){
                attackCooldownTimer -= Time.deltaTime;
            }

            if(enemyState == EnemyState.Chasing){
                Chase();
            }
            else if(enemyState == EnemyState.Attacking){
                rb.velocity = Vector2.zero;
            }
        
        }
    }

    void Chase(){

        if(player.position.x > transform.position.x && facingDirection == -1 || player.position.x < transform.position.x && facingDirection == 1){
            Flip();
        }
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }


    private void CheckForPlayer()
    {
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerLayer);


        if(hits.Length > 0)
        {

            player = hits[0].transform;

            if (Vector2.Distance(transform.position, player.transform.position) <= attackRange && attackCooldownTimer <= 0)
            {
   
                attackCooldownTimer = attackCooldown;
                ChangeState(EnemyState.Attacking);

            }
            else if(Vector2.Distance(transform.position, player.position) > attackRange && enemyState != EnemyState.Attacking)
            {

                ChangeState(EnemyState.Chasing);
            }
        }
        else
        {
            ChangeState(EnemyState.Idle);
            rb.velocity = Vector2.zero;


        }
        
            
         
    }
        

    public void ChangeState(EnemyState newState){
    //exit current animation
    if(enemyState == EnemyState.Idle){
        anim.SetBool("isIdle", false);
        Debug.Log("Exiting Idle State");
    }
    else if (enemyState == EnemyState.Chasing){
        anim.SetBool("isChasing", false);
        Debug.Log("Exiting Chasing State");
    }
    else if (enemyState == EnemyState.Attacking){
        anim.SetBool("isAttacking", false);
        Debug.Log("Exiting Attacking State");
    }

    enemyState = newState;
    Debug.Log("Changing State to: " + newState);

    //update new animations
    if(enemyState == EnemyState.Idle){
        anim.SetBool("isIdle", true);
        Debug.Log("Entering Idle State");
    }
    else if (enemyState == EnemyState.Chasing){
        anim.SetBool("isChasing", true);
        Debug.Log("Entering Chasing State");
    }
    else if (enemyState == EnemyState.Attacking){
        anim.SetBool("isAttacking", true);
        //Debug.Log("Attacking Player");
        //Debug.Log("Animator isAttacking: " + anim.GetBool("isAttacking"));
    }

}

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionPoint.position, playerDetectRange);

    }
}


public enum EnemyState{
    Idle,
    Chasing,
    Attacking,
    Knockback
}
