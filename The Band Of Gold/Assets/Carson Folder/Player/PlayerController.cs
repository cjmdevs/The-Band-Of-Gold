using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>, IDataPersistence
{
    public bool FacingLeft {get {return facingLeft;} }

    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;
    [SerializeField] private Transform weaponCollider;
    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private float startingMoveSpeed;
    private bool isKnockedBack;
    private bool facingLeft = false;
    private bool isDashing = false;
    
    private string enabledDash = "";

    public CoinManager cm;

    AudioManager audioManager;

    protected override void Awake() {

        base.Awake();
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start() {
        playerControls.Combat.Dash.performed += _ => Dash();

        startingMoveSpeed = StatsManager.Instance.speed;
    }
    private void OnEnable() {
        playerControls.Enable();
    }

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }
    
    private void Update() {
        PlayerInput();
    }

    public Transform GetWeaponCollider(){
        return weaponCollider;
    }

    private void FixedUpdate() {
        AdjustPlayerFacingDirection();
        Move();
    }

    private void PlayerInput() {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move() {
        if (isKnockedBack == false) {
            rb.MovePosition(rb.position + movement * (StatsManager.Instance.speed * Time.fixedDeltaTime));
        }
        
    }

    private void AdjustPlayerFacingDirection() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x) {
            mySpriteRender.flipX = true;
            facingLeft = true;
        } else {
            mySpriteRender.flipX = false;
            facingLeft = false;
        }
    }
    private void Dash() {
        if (!isDashing && Stamina.Instance.CurrentStamina > 0) 
        {
            Stamina.Instance.UseStamina();
            audioManager.PlaySFX(audioManager.playerDash);
            isDashing = true;
            StatsManager.Instance.speed *= dashSpeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            cm.coinCount++;
        }
    }

    private IEnumerator EndDashRoutine() {
        float dashTime = .2f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);
        StatsManager.Instance.speed = startingMoveSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }

    public void SaveData(GameData data)
    {
        throw new System.NotImplementedException();
    }

    public void Knockback(Transform enemy, float force, float stunTime)
    {
        isKnockedBack = true;
        Vector2 direction = (transform.position - enemy.position).normalized;
        rb.velocity = direction * force;
        StartCoroutine(KnockbackRoutine(stunTime));
    }

    IEnumerator KnockbackRoutine(float stunTime) {
        yield return new WaitForSeconds(stunTime);
        rb.velocity = Vector2.zero;
        isKnockedBack = false;
    }

    void EnableDash(string dashName)
    {
        enabledDash = dashName;
    }

    public void CharactersDash()
    {
        PlayerPrefs.SetString("EnabledPlayerDash", enabledDash);
    }
}

