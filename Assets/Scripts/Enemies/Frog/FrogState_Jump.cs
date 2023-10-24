using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogState_Jump : EnemyState
{
    Frog frogEnemy;

    public FrogState_Jump(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Frog _frogEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.frogEnemy = _frogEnemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetVelocity(frogEnemy.forwardJumpForce * enemy.facingDir, frogEnemy.upwardJumpForce);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.isDead)
            stateMachine.ChangeState(frogEnemy.deadState);

        if (rb.velocity.y < 0)
            stateMachine.ChangeState(frogEnemy.airState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
