using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{
    [Header("Follow camera info")]
    [SerializeField] Transform followCamera;
    [SerializeField] float cameraYPos;

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
    Material originalMat;
    bool isImmortal;

    [Header("Freeze timer info")]
    [SerializeField] float freezeTimeOnDropCatch;
    [SerializeField] float freezeTimeOnEnemyCollision;
    [SerializeField] float freezeTimeAfterRevive;
    [NonSerialized] public bool canMove;
    float freezeTimer;
    float canMoveTimer;

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

        followCamera.position = new Vector3(followCamera.position.x, cameraYPos);

        defaultParent = transform.parent;
        gM = GameManager.instance;
        stateMachine.Initialize(idleState);

        if (isOnStage1)
            SetFirstPlayerStage(0);
        else if (isOnStage2)
            SetSecondPlayerStage(0);
        else if (isOnStage3)
            SetThirdPlayerStage(0);

        canMove = true;
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
        KillPlayerOnFalling();

        followCamera.position = new Vector3(followCamera.position.x, cameraYPos);

        stateMachine.currentState.Update();

        if (isOnStage3 && canShoot && Input.GetKeyDown(KeyCode.Z) && canMove)
        {
            GameObject fireBall = Instantiate(fireBallPref,
                new Vector3(transform.position.x + (0.35f * facingDir), transform.position.y + 0.3f), Quaternion.identity);

            SetBallDirection(fireBall);

            canShoot = false;
            canShootTimer = shootingSpeed;
        }
    }

    void SetFirstPlayerStage(float _freezeTime)
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

        SetImmortalToPlayer(_freezeTime);
    }

    void SetSecondPlayerStage(float _freezeTime)
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

        SetImmortalToPlayer(_freezeTime);
    }

    void SetThirdPlayerStage(float _freezeTime)
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

        SetImmortalToPlayer(_freezeTime);
    }

    void SetImmortalToPlayer(float _freezeTime)
    {
        InvokeRepeating(nameof(WhiteBlinkFX), 0, 0.2f);
        freezeTimer = _freezeTime;
        canMoveTimer = _freezeTime * 0.8f;
        isImmortal = true;
        canMove = false;
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
            freezeTimer -= Time.deltaTime;
            canMoveTimer -= Time.deltaTime;

            if (canMoveTimer < 0)
                canMove = true;

            if (freezeTimer < 0)
            {
                CancelInvoke(nameof(WhiteBlinkFX));
                isImmortal = false;
                sr.material = originalMat;
            }
        }
    }

    void KillPlayerOnFalling()
    {
        if (transform.position.y < -4.3f && isDead == false)
        {
            PlayerDie();

            if (gM.gameOver == false)
                Invoke(nameof(RevivePlayer), 5f);
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

    void PlayerDie()
    {
        gM.DecreasePlayerLifeAmount();
        capsuleCollider.isTrigger = true;
        isDead = true;
    }

    #region Collision

    void OnCollisionEnter2D(Collision2D _collision)
    {
        EnemyCollision(_collision);
        ExtraLifeCollision(_collision);
        StageUpDropCollision(_collision);
        SpringboardCollision(_collision);
        MovablePlatformCollision(_collision);
        FireObstacleCollision(_collision);
    }

    void OnCollisionExit2D(Collision2D _collision)
    {
        if (_collision.gameObject.GetComponent<Platform>() != null)
        {
            if (transform.parent == defaultParent) { return; }

            Platform platform = _collision.gameObject.GetComponent<Platform>();
            transform.parent = defaultParent;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;

            if (isOnStage1)
                currentMoveSpeed = normalMoveSpeed;
            else if (isOnStage2 || isOnStage3)
                currentMoveSpeed = extraMoveSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D _collision)
    {
        CoinTriggerCollision(_collision);
    }

    void EnemyCollision(Collision2D _collision)
    {
        if (_collision.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy currentEnemy = _collision.gameObject.GetComponent<Enemy>();

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

                    SetSecondPlayerStage(freezeTimeOnEnemyCollision);

                    if (transform.position.x < currentEnemy.transform.position.x && currentEnemy.facingDir == -1 ||
                        transform.position.x > currentEnemy.transform.position.x && currentEnemy.facingDir == 1)
                        currentEnemy.Flip();
                }
                else if (isOnStage2)
                {
                    PushPlayerBackFromEnemy(currentEnemy);

                    SetFirstPlayerStage(freezeTimeOnEnemyCollision);

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

    void ExtraLifeCollision(Collision2D _collision)
    {
        if (_collision.gameObject.GetComponent<ExtraLifeDrop>() != null)
        {
            gM.IncreasePlayerLifeAmount();
            Destroy(_collision.gameObject);
        }
    }

    void StageUpDropCollision(Collision2D _collision)
    {
        if (_collision.gameObject.GetComponent<DropEntity>() != null)
        {
            if (gM.playerStage1)
                SetSecondPlayerStage(freezeTimeOnDropCatch);
            else if (gM.playerStage2)
                SetThirdPlayerStage(freezeTimeOnDropCatch);
            else if (gM.playerStage3)
                gM.IncreaseSocre(1000);

            Destroy(_collision.gameObject);
        }
    }

    void SpringboardCollision(Collision2D _collision)
    {
        if (_collision.gameObject.GetComponent<Springboard>() != null)
        {
            Springboard springboard = _collision.gameObject.GetComponent<Springboard>();

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

    void MovablePlatformCollision(Collision2D _collision)
    {
        if (_collision.gameObject.GetComponent<Platform>() != null)
        {
            Platform platform = _collision.gameObject.GetComponent<Platform>();

            if (platform.transform.position.y < transform.position.y - (capsuleCollider.size.y / 2) + 0.1f)
            {
                transform.parent = platform.transform;
                rb.interpolation = RigidbodyInterpolation2D.Extrapolate;
                currentMoveSpeed *= 0.75f;
            }
        }
    }

    void FireObstacleCollision(Collision2D _collision)
    {
        if (_collision.gameObject.GetComponentInParent<FireObstacle>() != null)
        {
            FireObstacle fireObstacle = _collision.gameObject.GetComponentInParent<FireObstacle>();

            if (isDead) { return; }

            if (transform.position.x < fireObstacle.transform.position.x)
                SetVelocity(-7, 11);
            else
                SetVelocity(7, 11);

            if (isImmortal) { return; }

            if (isOnStage3)
                SetSecondPlayerStage(freezeTimeOnEnemyCollision);
            else if (isOnStage2)
                SetFirstPlayerStage(freezeTimeOnEnemyCollision);
            else if (isOnStage1)
            {
                PlayerDie();

                if (gM.gameOver == false)
                    Invoke(nameof(RevivePlayer), 5f);
            }
        }
    }

    void CoinTriggerCollision(Collider2D _collision)
    {
        if (_collision.gameObject.GetComponent<Coin>() != null)
        {
            Coin coin = _collision.gameObject.GetComponent<Coin>();

            gM.IncreaseSocre(coin.scoreValue);
            Destroy(_collision.gameObject);
        }
    }

    bool leftLegIsAboveEnemy() => Physics2D.Raycast(groundCheck.position, Vector2.down, 0.4f, whatIsEnemy);

    bool rightLegIsAboveEnemy() => Physics2D.Raycast(secondGroundCheck.position, Vector2.down, 0.4f, whatIsEnemy);

    void PushPlayerBackFromEnemy(Enemy _currentEnemy)
    {
        if (transform.position.x > _currentEnemy.transform.position.x)
            SetVelocity(4, 10);

        if (transform.position.x < _currentEnemy.transform.position.x)
            SetVelocity(-4, 10);
    }

    void RevivePlayer()
    {
        FindPositionToRevive();
        capsuleCollider.isTrigger = false;
        isDead = false;
        SetFirstPlayerStage(freezeTimeAfterRevive);
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

        if (!GroundBelow() || SomethingIsAround() || GroundBelow().transform.gameObject.GetComponent<Platform>() != null)
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
