using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_combat : MonoBehaviour
{
    
    public int damage = 1; // damage enemy will do
    public float KnockbackForce; // amount of knockback enemy does
    public float stunTime; // amount of time player is stunned for
    public Transform attackPoint;
    public float weaponRange;
    public LayerMask playerLayer;



    public void EnemyAttack(){
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);

        if(hits.Length > 0)
        {
            hits[0].GetComponent<PlayerHealth>().ChangeHealth(-damage);
            hits[0].GetComponent<PlayerController>().Knockback(transform, KnockbackForce, stunTime);
            Debug.Log("Attack");
        }
    }
}
