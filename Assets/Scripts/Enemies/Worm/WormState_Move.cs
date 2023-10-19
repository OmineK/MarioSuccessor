using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormState_Move : EnemyState
{
    Worm wormEnemy;

    public WormState_Move(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Worm _wormEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.wormEnemy = _wormEnemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        rb.velocity = new Vector3(wormEnemy.moveSpeed * wormEnemy.facingDir, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.isDead)
        {
            stateMachine.ChangeState(wormEnemy.deadState);
            return;
        }

        if (enemy.isWallDetected() || !enemy.isGroundDetected())
            enemy.Flip();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
