using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnacleState_Hide : EnemyState
{
    Barnacle barnacleEnemy;

    float hideStateTimer;

    public BarnacleState_Hide(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Barnacle _barnacleEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.barnacleEnemy = _barnacleEnemy;
    }

    public override void Enter()
    {
        base.Enter();

        hideStateTimer = barnacleEnemy.hideTime;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!barnacleEnemy.isHidden())
            enemy.SetVelocity(rb.velocity.x, -barnacleEnemy.moveSpeed);
        else
            enemy.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (barnacleEnemy.isHidden())
            hideStateTimer -= Time.deltaTime;

        if (hideStateTimer < 0)
            stateMachine.ChangeState(barnacleEnemy.showState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
