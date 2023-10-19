using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastSnail : Enemy
{
    public FastSnailState_Move moveState { get; private set; }
    public FastSnailState_Dead deadState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        moveState = new FastSnailState_Move(this, stateMachine, "Move", this);
        deadState = new FastSnailState_Dead(this, stateMachine, "Dead", this);
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
