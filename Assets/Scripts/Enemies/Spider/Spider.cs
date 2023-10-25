using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy
{
    [Header("Follow movement info")]
    public float followSpeed;
    public float followTime;
    [Space]

    [Header("Player check info")]
    [SerializeField] Transform playerCheck;
    [SerializeField] float playerCheckDistanse;
    [SerializeField] LayerMask whatIsPlayer;

    public SpiderState_Move moveState { get; private set; }
    public SpiderState_Follow followState { get; private set; }
    public SpiderState_Dead deadState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        moveState = new SpiderState_Move(this, stateMachine, "Move", this);
        followState = new SpiderState_Follow(this, stateMachine, "Move", this);
        deadState = new SpiderState_Dead(this, stateMachine, "Dead", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(moveState);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool isPlayerDetected() => Physics2D.Raycast(playerCheck.position, Vector3.right * facingDir, playerCheckDistanse, whatIsPlayer);
    public bool isGroundBetween() => Physics2D.Raycast(playerCheck.position, Vector3.right * facingDir, playerCheckDistanse, whatIsGround);

    public RaycastHit2D playerHit() => Physics2D.Raycast(playerCheck.position, Vector3.right * facingDir, playerCheckDistanse, whatIsPlayer);
    public RaycastHit2D forwardGroundHit() => Physics2D.Raycast(playerCheck.position, Vector3.right * facingDir, playerCheckDistanse, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(playerCheck.position, new Vector2(playerCheck.position.x + playerCheckDistanse * facingDir, playerCheck.position.y));
    }
}
