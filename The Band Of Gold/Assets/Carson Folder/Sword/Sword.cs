using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Sword : MonoBehaviour
{ // accessing components
    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController; 
    private ActiveWeapon activeWeapon;

    private void Awake() { // gets called once
        playerController = GetComponentInParent<PlayerController>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        myAnimator = GetComponent<Animator>(); // get animator
        playerControls = new PlayerControls(); // get player controls
    }

    private void OnEnable() { // when object gets activated
        playerControls.Enable();

    }

    void Start() {
        playerControls.Combat.Attack.started += _ => Attack(); // on attack run Attack
    }

    private void Update() {
        MouseFollowWithOffset();
    }
    private void Attack() {  // attack function
        myAnimator.SetTrigger("Attack");
    }
    private void MouseFollowWithOffset() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

        float angle = Mathf.Atan2(mousePos.y , mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x) {
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
        } else {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
