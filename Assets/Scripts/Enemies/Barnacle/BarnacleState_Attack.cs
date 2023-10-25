using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnacleState_Attack : EnemyState
{
    Barnacle barnacleEnemy;

    float attackStateTimer;

    public BarnacleState_Attack(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Barnacle _barnacleEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.barnacleEnemy = _barnacleEnemy;
    }

    public override void Enter()
    {
        base.Enter();

        attackStateTimer = barnacleEnemy.attackTime;
        barnacleEnemy.SetZeroVelocity();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        attackStateTimer -= Time.deltaTime;

        if (attackStateTimer < 0)
            stateMachine.ChangeState(barnacleEnemy.hideState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
