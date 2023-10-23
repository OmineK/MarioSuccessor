using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : Entity
{
    [Header("Movement info")]
    public float moveSpeed;
    public float jumpForce;

    [Header("Player collision info")]
    [SerializeField] LayerMask whatIsEnemy;
    [SerializeField] Vector2 revivePositionCheck;
    [Space]
    public bool isDead;

    CapsuleCollider2D capsuleCollider;

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
                PlayerDie();

                if (GameManager.instance.gameOver == false)
                    Invoke(nameof(RevivePlayer), 5f);
            }
        }
    }

    bool isAboveEnemy() => Physics2D.Raycast(transform.position, Vector2.down, transform.localScale.y + 1, whatIsEnemy);

    void PlayerDie()
    {
        GameManager.instance.DecreasePlayerLifeAmount();
        capsuleCollider.isTrigger = true;
        isDead = true;
    }

    void RevivePlayer()
    {
        FindPositionToRevive();
        capsuleCollider.isTrigger = false;
        isDead = false;
    }

    void FindPositionToRevive()
    {
        transform.position = new Vector3(transform.position.x - 10f, transform.position.y + 4.5f);

        while (SomethingIsAround() || !GroundBelow())
        {
            float randomXoffset = Random.Range(-4f, -1f);
            transform.position = new Vector3(transform.position.x + randomXoffset, transform.position.y);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (capsuleCollider.size.y / 2) + 0.2f);
    }

    bool SomethingIsAround() => Physics2D.BoxCast(transform.position, revivePositionCheck, 0, Vector2.zero, 0, whatIsGround);

    RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 4, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(transform.position, revivePositionCheck);
    }

    #endregion
}
