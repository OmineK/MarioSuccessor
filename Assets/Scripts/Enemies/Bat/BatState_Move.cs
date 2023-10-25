using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatState_Move : EnemyState
{
    Bat batEnemy;

    bool moveUp;
    bool moveDown;

    public BatState_Move(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Bat _batEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.batEnemy = _batEnemy;
    }

    public override void Enter()
    {
        base.Enter();

        moveUp = true;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (moveUp)
            enemy.SetVelocity(batEnemy.moveSpeed * batEnemy.facingDir, batEnemy.flyUpAndDownSpeed);

        if (moveDown)
            enemy.SetVelocity(batEnemy.moveSpeed * batEnemy.facingDir, -batEnemy.flyUpAndDownSpeed);

    }

    public override void Update()
    {
        base.Update();

        if (enemy.isDead)
            stateMachine.ChangeState(batEnemy.deadState);

        if (batEnemy.GroundBelow().distance >= batEnemy.maxFlyRangeDistance && moveUp)
        {
            moveDown = true;
            moveUp = false;
        }

        if (batEnemy.GroundBelow().distance <= batEnemy.minFlyRangeDistance && moveDown)
        {
            moveUp = true;
            moveDown = false;
        }

        if (!batEnemy.isGroundDetected() || batEnemy.isWallDetected())
            batEnemy.Flip();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
