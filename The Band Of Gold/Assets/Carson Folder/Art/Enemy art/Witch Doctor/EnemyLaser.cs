using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField] private float laserGrowTime = 2f;
    [SerializeField] private float laserDamage = 0.05f; // Define the laserDamage variable

    private bool isGrowing = true;
    private float laserRange;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider2D;
    private Transform playerTransform;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Indestructible>() && !other.isTrigger)
        {
            isGrowing = false;
        }
        else if (other.gameObject.CompareTag("Player") && !other.isTrigger)
        {
            other.GetComponent<PlayerHealth>().ChangeHealth(-laserDamage);
            isGrowing = false;
        }
    }

    public void UpdateLaserRange(float laserRange, Transform playerTransform)
    {
        this.laserRange = laserRange;
        this.playerTransform = playerTransform;
        StartCoroutine(IncreaseLaserLengthRoutine());
    }

    private IEnumerator IncreaseLaserLengthRoutine()
    {
        float timePassed = 0f;

        while (timePassed < laserGrowTime && isGrowing)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / laserGrowTime;

            // sprite
            spriteRenderer.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), 1f);

            // collider
            capsuleCollider2D.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), capsuleCollider2D.size.y);
            capsuleCollider2D.offset = new Vector2((Mathf.Lerp(1f, laserRange, linearT)) / 2, capsuleCollider2D.offset.y);

            yield return null;
        }

        // Check if the SpriteFade component exists before starting the fade routine
        SpriteFade spriteFade = GetComponent<SpriteFade>();
        if (spriteFade != null)
        {
            StartCoroutine(spriteFade.SlowFadeRoutine());
        }

        // Remove the problematic line:
        // enemy_combat enemyCombat = GetComponentInParent<enemy_combat>();
        // if (enemyCombat != null)
        // {
        //     enemyCombat.OnLaserAttackComplete();
        // }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.right = direction;
        }
    }
}