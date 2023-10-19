using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailState_Move : EnemyState
{
    Snail snailEnemy;

    public SnailState_Move(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Snail _snailEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.snailEnemy = _snailEnemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        rb.velocity = new Vector3(snailEnemy.moveSpeed * snailEnemy.facingDir, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.isDead)
        {
            stateMachine.ChangeState(snailEnemy.deadState);
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
