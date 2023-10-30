using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barnacle : Enemy
{
    [Header("State info")]
    public float hideTime;
    public float attackTime;

    public BarnacleState_ShowUp showState { get; private set; }
    public BarnacleState_Hide hideState { get; private set; }
    public BarnacleState_Attack attackState { get; private set; }
    public BarnacleState_Dead deadState { get; private set; }

    BoxCollider2D cd;

    protected override void Awake()
    {
        base.Awake();

        cd = GetComponent<BoxCollider2D>();

        showState = new BarnacleState_ShowUp(this, stateMachine, "Idle", this);
        hideState = new BarnacleState_Hide(this, stateMachine, "Idle", this);
        attackState = new BarnacleState_Attack(this, stateMachine, "Attack", this);
        deadState = new BarnacleState_Dead(this, stateMachine, "Dead", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(hideState);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();

        if (isHidden() && isDead == true)
            Destroy(this.gameObject);
    }

    public override void Die()
    {
        isDead = true;
        stateMachine.ChangeState(deadState);
    }

    public override bool isGroundDetected() => Physics2D.Raycast(transform.position, Vector2.down, (cd.size.y / 2) - 0.02f, whatIsGround);

    public bool isHidden() => Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + (cd.size.y / 2)), Vector2.up, 0.1f, whatIsGround);

}
