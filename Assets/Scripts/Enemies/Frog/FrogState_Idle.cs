using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogState_Idle : EnemyState
{
    Frog frogEnemy;
    float idleStateTimer;

    public FrogState_Idle(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Frog _frogEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.frogEnemy = _frogEnemy;
    }

    public override void Enter()
    {
        base.Enter();

        idleStateTimer = frogEnemy.moveSpeed;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        idleStateTimer -= Time.deltaTime;

        if (enemy.isDead)
            stateMachine.ChangeState(frogEnemy.deadState);

        if (idleStateTimer < 0)
            stateMachine.ChangeState(frogEnemy.jumpState);

        if (enemy.isWallDetected() || !frogEnemy.canJump())
            enemy.Flip();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
