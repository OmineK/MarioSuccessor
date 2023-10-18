using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseState_Move : EnemyState
{
    Mouse mouseEnemy;

    public MouseState_Move(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Mouse _mouseEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.mouseEnemy = _mouseEnemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        rb.velocity = new Vector3(mouseEnemy.moveSpeed * mouseEnemy.facingDir, rb.velocity.y);
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
