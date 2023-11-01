using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{
    [Header("Movement info")]
    [SerializeField] float normalMoveSpeed;
    [SerializeField] float extraMoveSpeed;
    [SerializeField] float normalJumpForce;
    [SerializeField] float extraJumpForce;
    [NonSerialized] public float currentMoveSpeed;
    [NonSerialized] public float currentJumpForce;

    [Header("Player collision info")]
    [SerializeField] Transform secondGroundCheck;
    [SerializeField] float secondGroundCheckDistance;
    [SerializeField] LayerMask whatIsEnemy;
    [SerializeField] Vector2 revivePositionCheck;
    [Space]
    [NonSerialized] public bool isDead;

    [Header("Physic material info")]
    public PhysicsMaterial2D friction;
    public PhysicsMaterial2D frictionless;

    [Header("Player stage 1 info")]
    [SerializeField] GameObject firstStage;
    [SerializeField] Vector2 firstStageColliderOffset;
    [SerializeField] Vector2 firstStageColliderSize;
    [SerializeField] bool isOnStage1;
    [Space]

    [Header("Player stage 2 info")]
    [SerializeField] GameObject secondStage;
    [SerializeField] Vector2 secondStageColliderOffset;
    [SerializeField] Vector2 secondStageColliderSize;
    [SerializeField] bool isOnStage2;
    [Space]

    [Header("Player stage 3 info")]
    [SerializeField] GameObject thirdStage;
    [SerializeField] Vector2 thirdStageColliderOffset;
    [SerializeField] Vector2 thirdStageColliderSize;
    [SerializeField] bool isOnStage3;

    [Header("White blink FX && Player immortality")]
    [SerializeField] Material changedMat;
    [SerializeField] float immortalTime;
    Material originalMat;
    bool isImmortal;
    float immortalTimer;

    [Header("Fireball info")]
    [SerializeField] GameObject fireBallPref;
    [SerializeField] float shootingSpeed;
    float canShootTimer;
    bool canShoot;
    [Space]

    public Transform defaultParent;

    CapsuleCollider2D capsuleCollider;
    SpriteRenderer sr;
    GameManager gM;

    #region Player states

    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerState_Idle idleState { get; private set; }
    public PlayerState_Move moveState { get; private set; }
    public PlayerState_Jump jumpState { get; private set; }
    public PlayerState_Air airState { get; private set; }

    public PlayerState_Dead deadState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        capsuleCollider = GetComponent<CapsuleCollider2D>();

        stateMachine = new PlayerStateMachine();
        idleState = new PlayerState_Idle(this, stateMachine, "Idle");
        moveState = new PlayerState_Move(this, stateMachine, "Move");
        jumpState = new PlayerState_Jump(this, stateMachine, "Jump");
        airState = new PlayerState_Air(this, stateMachine, "Jump");

        deadState = new PlayerState_Dead(this, stateMachine, "Dead");
    }

    protected override void Start()
    {
        base.Start();

        defaultParent = transform.parent;
        gM = GameManager.instance;
        stateMachine.Initialize(idleState);

        if (isOnStage1)
            SetFirstPlayerStage(0);
        else if (isOnStage2)
            SetSecondPlayerStage(0);
        else if (isOnStage3)
            SetThirdPlayerStage(0);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        stateMachine.currentState.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();
        CanShootTimer();
        ImmortalityTimer();

        stateMachine.currentState.Update();

        if (isOnStage3 && canShoot && Input.GetKeyDown(KeyCode.Z))
        {
            GameObject fireBall = Instantiate(fireBallPref,
                new Vector3(transform.position.x + (0.35f * facingDir), transform.position.y + 0.3f), Quaternion.identity);

            SetBallDirection(fireBall);

            canShoot = false;
            canShootTimer = shootingSpeed;
        }
    }

    void SetFirstPlayerStage(float _immortalTime)
    {
        isOnStage1 = true;
        isOnStage2 = false;
        isOnStage3 = false;

        secondStage.SetActive(false);
        thirdStage.SetActive(false);

        firstStage.SetActive(true);

        currentMoveSpeed = normalMoveSpeed;
        currentJumpForce = normalJumpForce;
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();

        originalMat = sr.material;

        capsuleCollider.offset = firstStageColliderOffset;
        capsuleCollider.size = firstStageColliderSize;

        gM.UpdatePlayerCurrentStage(isOnStage1, isOnStage2, isOnStage3);

        InvokeRepeating(nameof(WhiteBlinkFX), 0, 0.2f);
        immortalTimer = _immortalTime;
        isImmortal = true;
    }

    void SetSecondPlayerStage(float _immortalTime)
    {
        isOnStage1 = false;
        isOnStage2 = true;
        isOnStage3 = false;

        firstStage.SetActive(false);
        thirdStage.SetActive(false);

        secondStage.SetActive(true);

        currentMoveSpeed = extraMoveSpeed;
        currentJumpForce = extraJumpForce;
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();

        originalMat = sr.material;

        capsuleCollider.offset = secondStageColliderOffset;
        capsuleCollider.size = secondStageColliderSize;

        gM.UpdatePlayerCurrentStage(isOnStage1, isOnStage2, isOnStage3);

        InvokeRepeating(nameof(WhiteBlinkFX), 0, 0.2f);
        immortalTimer = _immortalTime;
        isImmortal = true;
    }

    void SetThirdPlayerStage(float _immortalTime)
    {
        isOnStage1 = false;
        isOnStage2 = false;
        isOnStage3 = true;

        firstStage.SetActive(false);
        secondStage.SetActive(false);

        thirdStage.SetActive(true);

        currentMoveSpeed = extraMoveSpeed;
        currentJumpForce = extraJumpForce;
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();

        originalMat = sr.material;

        capsuleCollider.offset = thirdStageColliderOffset;
        capsuleCollider.size = thirdStageColliderSize;

        gM.UpdatePlayerCurrentStage(isOnStage1, isOnStage2, isOnStage3);

        InvokeRepeating(nameof(WhiteBlinkFX), 0, 0.2f);
        immortalTimer = _immortalTime;
        isImmortal = true;
    }

    void CanShootTimer()
    {
        if (!canShoot)
        {
            canShootTimer -= Time.deltaTime;

            if (canShootTimer < 0)
                canShoot = true;
        }
    }

    void ImmortalityTimer()
    {
        if (isImmortal)
        {
            immortalTimer -= Time.deltaTime;

            if (immortalTimer < 0)
            {
                CancelInvoke(nameof(WhiteBlinkFX));
                isImmortal = false;
                sr.material = originalMat;
            }
        }
    }

    void SetBallDirection(GameObject fireBall)
    {
        if (facingDir == 1)
            fireBall.GetComponent<Fireball>().facingDir = 1;
        else if (facingDir == -1)
            fireBall.GetComponent<Fireball>().facingDir = -1;
    }

    void WhiteBlinkFX()
    {
        if (sr.material == originalMat)
            sr.material = changedMat;
        else
            sr.material = originalMat;
    }

    #region Collision

    void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyCollision(collision);
        ExtraLifeCollision(collision);
        StageUpDropCollision(collision);
        SpringboardCollision(collision);
        MovablePlatformCollision(collision);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Platform>() != null)
        {
            if (transform.parent == defaultParent) { return; }

            Platform platform = collision.gameObject.GetComponent<Platform>();
            transform.parent = defaultParent;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;

            if (isOnStage1)
                currentMoveSpeed = normalMoveSpeed;
            else if (isOnStage2 || isOnStage3)
                currentMoveSpeed = extraMoveSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        CoinTriggerCollision(collision);
    }

    void EnemyCollision(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy currentEnemy = collision.gameObject.GetComponent<Enemy>();

            if (currentEnemy.isDead == true) { return; }

            if (leftLegIsAboveEnemy() || rightLegIsAboveEnemy())
            {
                gM.IncreaseSocre(currentEnemy.scoreValue);
                currentEnemy.Die();

                SetVelocity(0, 10);
            }
            else
            {
                if (isImmortal) 
                {
                    currentEnemy.Flip();
                    return; 
                }

                if (isOnStage3)
                {
                    PushPlayerBackFromEnemy(currentEnemy);

                    SetSecondPlayerStage(immortalTime);

                    if (transform.position.x < currentEnemy.transform.position.x && currentEnemy.facingDir == -1 ||
                        transform.position.x > currentEnemy.transform.position.x && currentEnemy.facingDir == 1)
                        currentEnemy.Flip();
                }
                else if (isOnStage2)
                {
                    PushPlayerBackFromEnemy(currentEnemy);

                    SetFirstPlayerStage(immortalTime);

                    if (transform.position.x < currentEnemy.transform.position.x && currentEnemy.facingDir == -1 ||
                        transform.position.x > currentEnemy.transform.position.x && currentEnemy.facingDir == 1)
                        currentEnemy.Flip();
                }
                else if (isOnStage1)
                {
                    PlayerDie();

                    if (gM.gameOver == false)
                        Invoke(nameof(RevivePlayer), 5f);
                }
            }
        }
    }

    void ExtraLifeCollision(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<ExtraLifeDrop>() != null)
        {
            gM.IncreasePlayerLifeAmount();
            Destroy(collision.gameObject);
        }
    }

    void StageUpDropCollision(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<DropEntity>() != null)
        {
            if (gM.playerStage1)
                SetSecondPlayerStage(immortalTime);
            else if (gM.playerStage2)
                SetThirdPlayerStage(immortalTime);
            else if (gM.playerStage3)
                gM.IncreaseSocre(1000);

            Destroy(collision.gameObject);
        }
    }

    void SpringboardCollision(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Springboard>() != null)
        {
            Springboard springboard = collision.gameObject.GetComponent<Springboard>();

            float lowestPointOfPlayerPos = transform.position.y - (capsuleCollider.size.y / 2);

            if (lowestPointOfPlayerPos + 0.1f > springboard.transform.position.y - 0.2f)
            {
                springboard.anim.SetTrigger("Jump");

                if (isOnStage1)
                    SetVelocity(rb.velocity.x, springboard.playerStage1PushForce);
                else if (isOnStage2 || isOnStage3)
                    SetVelocity(rb.velocity.x, springboard.playerStage2and3PushForce);
            }
        }
    }

    void MovablePlatformCollision(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Platform>() != null)
        {
            Platform platform = collision.gameObject.GetComponent<Platform>();

            if (platform.transform.position.y < transform.position.y - (capsuleCollider.size.y / 2) + 0.1f)
            {
                transform.parent = platform.transform;
                rb.interpolation = RigidbodyInterpolation2D.Extrapolate;
                currentMoveSpeed *= 0.75f;
            }
        }
    }

    void CoinTriggerCollision(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Coin>() != null)
        {
            Coin coin = collision.gameObject.GetComponent<Coin>();

            gM.IncreaseSocre(coin.scoreValue);
            Destroy(collision.gameObject);
        }
    }

    bool leftLegIsAboveEnemy() => Physics2D.Raycast(groundCheck.position, Vector2.down, 0.4f, whatIsEnemy);

    bool rightLegIsAboveEnemy() => Physics2D.Raycast(secondGroundCheck.position, Vector2.down, 0.4f, whatIsEnemy);

    void PushPlayerBackFromEnemy(Enemy currentEnemy)
    {
        if (transform.position.x > currentEnemy.transform.position.x)
            SetVelocity(2, 8);

        if (transform.position.x < currentEnemy.transform.position.x)
            SetVelocity(-2, 8);
    }

    void PlayerDie()
    {
        gM.DecreasePlayerLifeAmount();
        capsuleCollider.isTrigger = true;
        isDead = true;
    }

    void RevivePlayer()
    {
        FindPositionToRevive();
        capsuleCollider.isTrigger = false;
        isDead = false;
        SetFirstPlayerStage(immortalTime * 2);
    }

    void FindPositionToRevive()
    {
        int lookingForNewPosAttempts = 10;
        Vector3 startingPos = transform.position;

        transform.position = new Vector3(transform.position.x - 10f, transform.position.y + 4.5f);

        while (SomethingIsAround() && lookingForNewPosAttempts > 0 ||
               !GroundBelow() && lookingForNewPosAttempts > 0)
        {
            float randomXoffset = UnityEngine.Random.Range(-4f, -1f);
            transform.position = new Vector3(transform.position.x + randomXoffset, transform.position.y);
            lookingForNewPosAttempts--;
        }

        if (!GroundBelow() || SomethingIsAround())
        {
            transform.position = startingPos;
            transform.position = new Vector3(transform.position.x - 2f, transform.position.y + 4.5f);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (capsuleCollider.size.y / 2) + 0.2f);
    }

    bool SomethingIsAround() => Physics2D.BoxCast(transform.position, revivePositionCheck, 0, Vector2.zero, 0, whatIsGround);

    RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 4, whatIsGround);

    public bool isSecondGroundDetected() => Physics2D.Raycast(secondGroundCheck.position, Vector2.down, secondGroundCheckDistance, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(secondGroundCheck.position, new Vector2(secondGroundCheck.position.x, secondGroundCheck.position.y - secondGroundCheckDistance));
        Gizmos.DrawWireCube(transform.position, revivePositionCheck);
    }

    #endregion
}
