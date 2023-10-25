using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnacleState_ShowUp : EnemyState
{
    Barnacle barnacleEnemy;

    public BarnacleState_ShowUp(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Barnacle _barnacleEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.barnacleEnemy = _barnacleEnemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        enemy.SetVelocity(rb.velocity.x, barnacleEnemy.moveSpeed);
    }

    public override void Update()
    {
        base.Update();

        if (!barnacleEnemy.isGroundDetected())
            stateMachine.ChangeState(barnacleEnemy.attackState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
