using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement info")]
    public float playerMoveSpeed;

    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    #region States

    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerState_Idle idleState { get; private set; }
    public PlayerState_Move moveState { get; private set; }

    #endregion

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        stateMachine = new PlayerStateMachine();
        idleState = new PlayerState_Idle(this, stateMachine, "Idle");
        moveState = new PlayerState_Move(this, stateMachine, "Move");
    }

    void Start()
    {
        stateMachine.Initialize(idleState);
    }

    void FixedUpdate()
    {
        stateMachine.currentState.FixedUpdate();
    }

    void Update()
    {
        stateMachine.currentState.Update();
    }

    public void SetZeroVelocity() => rb.velocity = Vector3.zero;

    public void SetVelocity(float _xVelocity, float _yVelocity) => rb.velocity = new Vector3(_xVelocity, _yVelocity);
}
