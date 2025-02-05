using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;

    private WeaponInfo weaponInfo;
    private Vector3 startPosition;

    public float knockbackForce = 50f;
    public float knockbackTime = 0.15f;
    public float stunTime = 0.3f;

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
            enemyHealth?.TakeDamage(weaponInfo.weaponDamage);
            Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
            knockback?.EnemyKnockback(transform, knockbackForce, knockbackTime ,stunTime);
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
