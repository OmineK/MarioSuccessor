using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bat : Enemy
{
    [Header("Fly info")]
    public float flyUpAndDownSpeed;
    public float minFlyRangeDistance;
    public float maxFlyRangeDistance;
    [Space]
    [SerializeField] Transform flyingWallCheck;
    [SerializeField] float flyingWallDistance;

    public BatState_Move MoveState { get; private set; }
    public BatState_Dead deadState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        MoveState = new BatState_Move(this, stateMachine, "Move", this);
        deadState = new BatState_Dead(this, stateMachine, "Dead", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(MoveState);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();
    }

    public RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 10, whatIsGround);

    public override bool isWallDetected() => Physics2D.Raycast(flyingWallCheck.position, Vector2.down, flyingWallDistance, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - maxFlyRangeDistance));
        Gizmos.DrawLine(flyingWallCheck.position, new Vector2(flyingWallCheck.position.x, flyingWallCheck.position.y - flyingWallDistance));
    }
}
