using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public BossState_Move moveState { get; private set; }
    public BossState_Shoot shootState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        moveState = new BossState_Move(this, stateMachine, "Move", this);
        shootState = new BossState_Shoot(this, stateMachine, "Shoot", this);
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
}
