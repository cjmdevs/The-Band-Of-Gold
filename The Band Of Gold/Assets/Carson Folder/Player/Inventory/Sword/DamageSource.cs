using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class DamageSource : MonoBehaviour
{   

    private float damageAmount;

    private void Start()
    {
        MonoBehaviour currentActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
        damageAmount = (currentActiveWeapon as IWeapon).GetWeaponInfo().weaponDamage + StatsManager.Instance.damage;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        Knockback knockback = other.gameObject.GetComponent<Knockback>();
        enemyHealth?.TakeDamage(damageAmount);
        knockback?.EnemyKnockback(transform, StatsManager.Instance.knockbackForce, StatsManager.Instance.knockbackTime ,StatsManager.Instance.stunTime);

    }
}
