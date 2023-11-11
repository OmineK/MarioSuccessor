using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnacleState_Dead : EnemyState
{
    Barnacle barnacleEnemy;

    float deadStateTimer;

    bool showedUp;

    public BarnacleState_Dead(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Barnacle _barnacleEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.barnacleEnemy = _barnacleEnemy;
    }

    public override void Enter()
    {
        base.Enter();
        deadStateTimer = 1.6f;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (deadStateTimer < 0f)
        {
            if (!barnacleEnemy.isHidden())
                enemy.SetVelocity(rb.velocity.x, -2f);
        }

        if (!barnacleEnemy.isGroundDetected() && !showedUp)
        {
            enemy.SetZeroVelocity();
            showedUp = true;
        }
    }

    public override void Update()
    {
        base.Update();

        deadStateTimer -= Time.deltaTime;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
