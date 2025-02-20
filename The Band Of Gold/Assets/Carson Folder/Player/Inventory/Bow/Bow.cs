using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow: MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;

    readonly int FIRE_HASH = Animator.StringToHash("Fire");

    private Animator myAnimator;

    AudioManager audioManager;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void Attack()
    {
        myAnimator.SetTrigger(FIRE_HASH);
        audioManager.PlaySFX(audioManager.Bow);
        GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        newArrow.GetComponent<Projectile>().UpdateWeaponInfo(weaponInfo);
    }


    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }


}
