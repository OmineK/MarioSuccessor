using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastSnailState_Move : EnemyState
{
    FastSnail fastSnailEnemy;

    public FastSnailState_Move(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, FastSnail _fastSnailEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.fastSnailEnemy = _fastSnailEnemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        rb.velocity = new Vector3(fastSnailEnemy.moveSpeed * fastSnailEnemy.facingDir, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.isWallDetected() || !enemy.isGroundDetected())
            enemy.Flip();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
