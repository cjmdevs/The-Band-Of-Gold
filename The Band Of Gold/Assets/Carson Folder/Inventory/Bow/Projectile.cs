using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;

    private WeaponInfo weaponInfo;
    private Vector3 startPosition;


    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update(){
        MoveProjectile();
        DetectFireDistance();
    }

    public void UpdateWeaponInfo(WeaponInfo weaponInfo){
        this.weaponInfo = weaponInfo;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        Indestructible indestructible = other.gameObject.GetComponent<Indestructible>();
        Knockback knockback = other.gameObject.GetComponent<Knockback>();
        // use if statement and fix it if stuff starts breaking upon shooting bow

        //if(!other.isTrigger && (enemyHealth || indestructable)) {
            Debug.Log("Take Damage");
            enemyHealth?.TakeDamage(weaponInfo.weaponDamage + StatsManager.Instance.damage);
            Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
            knockback?.EnemyKnockback(transform, StatsManager.Instance.knockbackForce, StatsManager.Instance.knockbackTime ,StatsManager.Instance.stunTime);
            Destroy(gameObject);
       // }
    }

    private void DetectFireDistance(){
        if(Vector3.Distance(transform.position, startPosition) > weaponInfo.weaponRange) {
            Destroy(gameObject);
        }
    }
    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }
}
