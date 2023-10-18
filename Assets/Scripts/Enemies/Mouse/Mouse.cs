using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : Enemy
{
    public MouseState_Move moveState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        moveState = new MouseState_Move(this, stateMachine, "Move", this);
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
