using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatState_Dead : EnemyState
{
    Bat batEnemy;

    public BatState_Dead(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Bat _batEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.batEnemy = _batEnemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetVelocity(0, 1);
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
