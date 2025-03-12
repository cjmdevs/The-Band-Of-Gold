using UnityEngine;

public class BossController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform player;
    private Animator animator;
    private Vector2 movement;
    public GameObject enemyPrefab;
    public Transform summonPoint;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            movement = direction * moveSpeed;
            if(movement.magnitude > 0.1f){
                animator.SetBool("IsMoving", true);
                animator.SetFloat("MovementSpeed", movement.magnitude);
            } else {
                animator.SetBool("IsMoving", false);
            }
        }
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = movement;
    }

    public void PerformAttack(int attackNumber)
    {
        animator.SetInteger("AttackTrigger", attackNumber);
    }

    // Called during the mino_attack1 animation (Regular Attack)
    public void MinoAttack1Function()
    {
        Debug.Log("Regular Attack!");
        // Your regular attack logic here (e.g., moderate damage, standard range).
    }

    // Called during the mino_attack2 animation (Quick Attack)
    public void MinoAttack2Function()
    {
        Debug.Log("Quick Attack!");
        // Your quick attack logic here (e.g., low damage, short range, faster animation).
    }

    // Called during the mino_attack3 animation (Special Summon Attack)
    public void MinoAttack3Function()
    {
        if (enemyPrefab != null && summonPoint != null)
        {
            Instantiate(enemyPrefab, summonPoint.position, Quaternion.identity);
            Debug.Log("Special Summon Attack!");
        }
    }

    // Called during the mino_attack4 animation (Heavy Attack)
    public void MinoAttack4Function()
    {
        Debug.Log("Heavy Attack!");
        // Your heavy attack logic here (e.g., high damage, wide range, slow animation).
    }

    //Called by animator at the end of the attack animations.
    public void ResetAttackTrigger(){
        animator.SetInteger("AttackTrigger", 0);
    }
}