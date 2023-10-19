using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : Enemy
{
    public MouseState_Move moveState { get; private set; }
    public MouseState_Dead deadState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        moveState = new MouseState_Move(this, stateMachine, "Move", this);
        deadState = new MouseState_Dead(this, stateMachine, "Dead", this);
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
