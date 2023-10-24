using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Frog : Enemy
{
    [Header("Jump info")]
    public float forwardJumpForce;
    public float upwardJumpForce;
    [SerializeField] Transform canJumpCheck;
    [SerializeField] float canJumpCheckDistance;

    public FrogState_Idle idleState { get; private set; }
    public FrogState_Jump jumpState { get; private set; }
    public FrogState_Air airState { get; private set; }
    public FrogState_Dead deadState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new FrogState_Idle(this, stateMachine, "Idle", this);
        jumpState = new FrogState_Jump(this, stateMachine, "Jump", this);
        airState = new FrogState_Air(this, stateMachine, "Jump", this);
        deadState = new FrogState_Dead(this, stateMachine, "Dead", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool canJump() => Physics2D.Raycast(canJumpCheck.position, Vector2.down, canJumpCheckDistance, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(canJumpCheck.position, new Vector2(canJumpCheck.position.x, canJumpCheck.position.y - canJumpCheckDistance));
    }
}
