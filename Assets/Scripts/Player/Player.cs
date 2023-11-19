using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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
    [SerializeField] Transform playerBody;
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
    [SerializeField] float immortalityTime;
    [SerializeField] Material changedMat;
    Material originalMat;
    bool isImmortal;
    float immortalityTimer;

    [Header("Fireball info")]
    [SerializeField] GameObject fireBallPref;
    [SerializeField] float shootingSpeed;
    float canShootTimer;
    bool canShoot;
    [Space]

    public Transform defaultParent;
    public AudioManager aM;
    public GameManager gM;

    public bool isLevelLoading;
    public bool isPushed;

    bool isStartingLevel;

    CapsuleCollider2D capsuleCollider;
    SpriteRenderer sr;

    #region Player states

    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerState_Idle idleState { get; private set; }
    public PlayerState_Move moveState { get; private set; }
    public PlayerState_Jump jumpState { get; private set; }
    public PlayerState_Air airState { get; private set; }
    public PlayerState_Finish finishState { get; private set; }

    public PlayerState_Dead deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        isStartingLevel = true;

        capsuleCollider = GetComponent<CapsuleCollider2D>();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerState_Idle(this, stateMachine, "Idle");
        moveState = new PlayerState_Move(this, stateMachine, "Move");
        jumpState = new PlayerState_Jump(this, stateMachine, "Jump");
        airState = new PlayerState_Air(this, stateMachine, "Jump");
        finishState = new PlayerState_Finish(this, stateMachine, "Jump");

        deadState = new PlayerState_Dead(this, stateMachine, "Dead");
    }

    protected override void Start()
    {
        base.Start();

        followCamera.position = new Vector3(followCamera.position.x, cameraYPos);

        defaultParent = transform.parent;
        gM = GameManager.instance;
        aM = AudioManager.instance;
        stateMachine.Initialize(idleState);

        if (PlayerPrefs.GetInt("playerStage") == 1)
            SetFirstPlayerStage();
        else if (PlayerPrefs.GetInt("playerStage") == 2)
            SetSecondPlayerStage();
        else if (PlayerPrefs.GetInt("playerStage") == 3)
            SetThirdPlayerStage();

        isStartingLevel = false;
        isLevelLoading = false;
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
        KillPlayerAfterRoundTimeExpire();

        followCamera.position = new Vector3(followCamera.position.x, cameraYPos);

        stateMachine.currentState.Update();

        PlayerInputs();
    }

    void SetFirstPlayerStage()
    {
        #region setup animation
        idleState = new PlayerState_Idle(this, stateMachine, "Idle");
        moveState = new PlayerState_Move(this, stateMachine, "Move");
        jumpState = new PlayerState_Jump(this, stateMachine, "Jump");
        airState = new PlayerState_Air(this, stateMachine, "Jump");
        finishState = new PlayerState_Finish(this, stateMachine, "Jump");

        deadState = new PlayerState_Dead(this, stateMachine, "Dead");
        #endregion

        isOnStage1 = true;
        isOnStage2 = false;
        isOnStage3 = false;

        if (anim.GetBool("Idle2") || anim.GetBool("Idle3"))
        {
            anim.SetBool("Idle", true);
            anim.SetBool("Idle2", false);
            anim.SetBool("Idle3", false);
        }

        currentMoveSpeed = normalMoveSpeed;
        currentJumpForce = normalJumpForce;
        sr = GetComponentInChildren<SpriteRenderer>();

        originalMat = sr.material;

        capsuleCollider.offset = firstStageColliderOffset;
        capsuleCollider.size = firstStageColliderSize;

        gM.UpdatePlayerCurrentStage(isOnStage1, isOnStage2, isOnStage3);

        playerBody.transform.localPosition = Vector3.zero;
        playerBody.transform.localScale = Vector3.one;

        SetImmortalityPlayer();
    }

    void SetSecondPlayerStage()
    {
        #region setup animation
        idleState = new PlayerState_Idle(this, stateMachine, "Idle2");
        moveState = new PlayerState_Move(this, stateMachine, "Move2");
        jumpState = new PlayerState_Jump(this, stateMachine, "Jump2");
        airState = new PlayerState_Air(this, stateMachine, "Jump2");
        finishState = new PlayerState_Finish(this, stateMachine, "Jump2");

        deadState = new PlayerState_Dead(this, stateMachine, "Dead2");
        #endregion

        isOnStage1 = false;
        isOnStage2 = true;
        isOnStage3 = false;

        if (anim.GetBool("Idle") || anim.GetBool("Idle3"))
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Idle2", true);
            anim.SetBool("Idle3", false);
        }

        currentMoveSpeed = extraMoveSpeed;
        currentJumpForce = extraJumpForce;
        sr = GetComponentInChildren<SpriteRenderer>();

        originalMat = sr.material;

        capsuleCollider.offset = secondStageColliderOffset;
        capsuleCollider.size = secondStageColliderSize;

        gM.UpdatePlayerCurrentStage(isOnStage1, isOnStage2, isOnStage3);

        playerBody.transform.localPosition = new Vector3(0, 0.12f, 0);
        playerBody.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

        SetImmortalityPlayer();
    }

    void SetThirdPlayerStage()
    {
        #region setup animation
        idleState = new PlayerState_Idle(this, stateMachine, "Idle3");
        moveState = new PlayerState_Move(this, stateMachine, "Move3");
        jumpState = new PlayerState_Jump(this, stateMachine, "Jump3");
        airState = new PlayerState_Air(this, stateMachine, "Jump3");
        finishState = new PlayerState_Finish(this, stateMachine, "Jump3");

        deadState = new PlayerState_Dead(this, stateMachine, "Dead3");
        #endregion

        isOnStage1 = false;
        isOnStage2 = false;
        isOnStage3 = true;

        if (anim.GetBool("Idle") || anim.GetBool("Idle2"))
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Idle2", false);
            anim.SetBool("Idle3", true);
        }

        currentMoveSpeed = extraMoveSpeed;
        currentJumpForce = extraJumpForce;
        sr = GetComponentInChildren<SpriteRenderer>();

        originalMat = sr.material;

        capsuleCollider.offset = thirdStageColliderOffset;
        capsuleCollider.size = thirdStageColliderSize;

        gM.UpdatePlayerCurrentStage(isOnStage1, isOnStage2, isOnStage3);

        playerBody.transform.localPosition = new Vector3(0, 0.12f, 0);
        playerBody.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

        SetImmortalityPlayer();
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
            immortalityTimer -= Time.deltaTime;

            if (immortalityTimer < 0)
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

    void KillPlayerAfterRoundTimeExpire()
    {
        if (gM.roundTimeTimer <= 0 && !gM.gameOver)
        {
            aM.PlaySFX(8);

            stateMachine.ChangeState(deadState);
            capsuleCollider.isTrigger = true;
            isDead = true;

            gM.GameOver();
        }
    }

    void PlayerInputs()
    {
        if (isOnStage3 && canShoot && Input.GetKeyDown(KeyCode.Z))
        {
            aM.PlaySFX(1);

            GameObject fireBall = Instantiate(fireBallPref,
                new Vector3(transform.position.x + (0.35f * facingDir), transform.position.y + 0.3f), Quaternion.identity);

            SetFireballDirection(fireBall);

            canShoot = false;
            canShootTimer = shootingSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0 &&
            UIManager.instance.goToMenuInfoPanel.activeInHierarchy == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0;
            UIManager.instance.MenuPanelActiveUI(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0 &&
            UIManager.instance.goToMenuInfoPanel.activeInHierarchy == false)
        {
            Time.timeScale = 1;
            UIManager.instance.MenuPanelActiveUI(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void SetImmortalityPlayer()
    {
        if (isStartingLevel) { return; }

        InvokeRepeating(nameof(WhiteBlinkFX), 0, 0.2f);
        isImmortal = true;

        if (isDead)
            immortalityTimer = immortalityTime * 2;
        else
            immortalityTimer = immortalityTime;
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
        aM.PlaySFX(8);

        gM.DecreasePlayerLifeAmount();
        capsuleCollider.isTrigger = true;
        isDead = true;
    }

    void RevivePlayer()
    {
        FindPositionToRevive();
        capsuleCollider.isTrigger = false;
        SetFirstPlayerStage();
        isDead = false;
    }

    void SetFireballDirection(GameObject fireBall)
    {
        if (facingDir == 1)
            fireBall.GetComponent<Fireball>().facingDir = 1;
        else if (facingDir == -1)
            fireBall.GetComponent<Fireball>().facingDir = -1;
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
            transform.position = new Vector3(startingPos.x, -6.2f, startingPos.z);
            transform.position = new Vector3(transform.position.x, transform.position.y + 10f);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (capsuleCollider.size.y / 2) + 0.2f);
    }

    void PushPlayerBackFromEnemy(Enemy _currentEnemy)
    {
        isPushed = true;

        if (transform.position.x > _currentEnemy.transform.position.x)
            SetVelocity(8, 12);

        if (transform.position.x < _currentEnemy.transform.position.x)
            SetVelocity(-8, 12);

        Invoke(nameof(CancelPushedInputBlock), 0.4f);
    }

    void CancelPushedInputBlock() => isPushed = false;

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
        FlagTriggerCollision(_collision);
        BossFireballCollision(_collision);
    }

    void EnemyCollision(Collision2D _collision)
    {
        if (_collision.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy currentEnemy = _collision.gameObject.GetComponent<Enemy>();

            if (currentEnemy.isDead == true) { return; }

            if (leftLegIsAboveEnemy() || rightLegIsAboveEnemy())
            {
                if (_collision.gameObject.GetComponent<Boss>() != null)
                {
                    Boss boss = _collision.gameObject.GetComponent<Boss>();

                    aM.PlaySFX(5);

                    int randomYVelo = UnityEngine.Random.Range(14, 20);

                    SetVelocity(0, randomYVelo);

                    boss.bossHP--;

                    if (boss.bossHP <= 0)
                    {
                        gM.IncreaseSocre(boss.scoreValue);
                        boss.Die();
                    }

                    return;
                }
                else
                {
                    gM.IncreaseSocre(currentEnemy.scoreValue);
                    currentEnemy.Die();

                    aM.PlaySFX(5);
                    SetVelocity(0, 14);
                }
            }
            else
            {
                if (isImmortal)
                {
                    currentEnemy.Flip();
                    PushPlayerBackFromEnemy(currentEnemy);
                    return;
                }

                if (isOnStage3)
                {
                    PushPlayerBackFromEnemy(currentEnemy);
                    SetSecondPlayerStage();

                    aM.PlaySFX(10);

                    if (transform.position.x < currentEnemy.transform.position.x && currentEnemy.facingDir == -1 ||
                        transform.position.x > currentEnemy.transform.position.x && currentEnemy.facingDir == 1)
                        currentEnemy.Flip();
                }
                else if (isOnStage2)
                {
                    PushPlayerBackFromEnemy(currentEnemy);
                    SetFirstPlayerStage();

                    aM.PlaySFX(10);

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
            aM.PlaySFX(3);

            gM.IncreasePlayerLifeAmount();
            Destroy(_collision.gameObject);
        }
    }

    void StageUpDropCollision(Collision2D _collision)
    {
        if (_collision.gameObject.GetComponent<StageUpDrop>() != null)
        {
            aM.PlaySFX(3);

            if (gM.playerStage1 && !isImmortal)
                SetSecondPlayerStage();
            else if (gM.playerStage2 && !isImmortal)
                SetThirdPlayerStage();
            else if (gM.playerStage1 && isImmortal)
                Invoke(nameof(SetSecondPlayerStage), immortalityTimer + 0.2f);
            else if (gM.playerStage2 && isImmortal)
                Invoke(nameof(SetThirdPlayerStage), immortalityTimer + 0.2f);
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
                aM.PlaySFX(9);

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
            aM.PlaySFX(0);

            FireObstacle fireObstacle = _collision.gameObject.GetComponentInParent<FireObstacle>();

            if (isDead) { return; }

            if (transform.position.x < fireObstacle.transform.position.x)
                SetVelocity(-7, 11);
            else
                SetVelocity(7, 11);

            if (isImmortal) { return; }

            if (isOnStage3)
            {
                aM.PlaySFX(10);
                SetSecondPlayerStage();
            }
            else if (isOnStage2)
            {
                aM.PlaySFX(10);
                SetFirstPlayerStage();
            }
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
            aM.PlaySFX(6);

            Coin coin = _collision.gameObject.GetComponent<Coin>();

            gM.IncreaseSocre(coin.scoreValue);
            Destroy(_collision.gameObject);
        }
    }

    void FlagTriggerCollision(Collider2D _collision)
    {
        if ((_collision.gameObject.GetComponent<LevelEndFlag>() != null) && !isLevelLoading)
        {
            LevelEndFlag flag = _collision.gameObject.GetComponent<LevelEndFlag>();

            aM.PlaySFX(2);

            if (gM.score > PlayerPrefs.GetInt("bestScore"))
                PlayerPrefs.SetInt("bestScore", gM.score);

            PlayerPrefs.SetInt("roundStartingScore", gM.score);
            PlayerPrefs.SetInt("roundStartingLifeAmount", gM.playerLifeAmount);

            if (isOnStage1)
                PlayerPrefs.SetInt("playerStage", 1);
            else if (isOnStage2)
                PlayerPrefs.SetInt("playerStage", 2);
            else if (isOnStage3)
                PlayerPrefs.SetInt("playerStage", 3);

            flag.LoadNextLevelAfter(5f);

            stateMachine.ChangeState(finishState);
            isLevelLoading = true;
            gM.stopRoundTimer = true;
        }
    }

    void BossFireballCollision(Collider2D _collision)
    {
        if (_collision.GetComponent<BossFireball>() != null)
        {
            if (isImmortal || isDead) { return; }

            if (isOnStage3)
            {
                SetSecondPlayerStage();
                aM.PlaySFX(10);
            }
            else if (isOnStage2)
            {
                SetFirstPlayerStage();
                aM.PlaySFX(10);
            }
            else if (isOnStage1)
            {
                PlayerDie();

                if (gM.gameOver == false)
                    Invoke(nameof(RevivePlayer), 5f);
            }
        }
    }

    bool leftLegIsAboveEnemy() => Physics2D.Raycast(groundCheck.position, Vector2.down, 0.4f, whatIsEnemy);

    bool rightLegIsAboveEnemy() => Physics2D.Raycast(secondGroundCheck.position, Vector2.down, 0.4f, whatIsEnemy);

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
