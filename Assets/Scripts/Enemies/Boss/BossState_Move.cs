using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState_Move : EnemyState
{
    Boss bossEnemy;

    public BossState_Move(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Boss _bossEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.bossEnemy = _bossEnemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
