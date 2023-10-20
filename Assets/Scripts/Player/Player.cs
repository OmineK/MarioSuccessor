using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class Player : Entity
{
    [Header("Movement info")]
    public float moveSpeed;
    public float jumpForce;

    [SerializeField] LayerMask whatIsEnemy;

    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerState_Idle idleState { get; private set; }
    public PlayerState_Move moveState { get; private set; }
    public PlayerState_Jump jumpState { get; private set; }
    public PlayerState_Air airState { get; private set; }

    public PlayerState_Dead deadState { get; private set; }

    CapsuleCollider2D capsuleCollider;

    public bool isDead;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();
        idleState = new PlayerState_Idle(this, stateMachine, "Idle");
        moveState = new PlayerState_Move(this, stateMachine, "Move");
        jumpState = new PlayerState_Jump(this, stateMachine, "Jump");
        airState = new PlayerState_Air(this, stateMachine, "Jump");

        deadState = new PlayerState_Dead(this, stateMachine, "Dead");

        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        stateMachine.currentState.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }



    #region Collision

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy currentEnemy = collision.gameObject.GetComponent<Enemy>();

            if (isAboveEnemy())
            {
                GameManager.instance.IncreaseSocre(currentEnemy.scoreValue);
                currentEnemy.Die();

                SetVelocity(0, 10);
            }
            else
            {
                GameManager.instance.DecreasePlayerLifeAmount();
                Die();
            }
        }
    }

    bool isAboveEnemy() => Physics2D.Raycast(transform.position, Vector2.down, transform.localScale.y + 1, whatIsEnemy);

    void Die()
    {
        capsuleCollider.isTrigger = true;
        isDead = true;
    }

    #endregion
}
