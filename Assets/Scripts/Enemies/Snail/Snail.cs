using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Snail : Enemy
{
    public SnailState_Move moveState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        moveState = new SnailState_Move(this, stateMachine, "Move", this);
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
