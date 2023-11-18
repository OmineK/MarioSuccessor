using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState_Move : EnemyState
{
    float changeDirectionTimer;
    float shootTimer;

    bool followWrongDirection;
    bool directionTimerReset;

    Boss bossEnemy;
    Transform player;

    public BossState_Move(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Boss _bossEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.bossEnemy = _bossEnemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = bossEnemy.player;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        rb.velocity = new Vector3(bossEnemy.moveSpeed * bossEnemy.facingDir, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.isWallDetected() || !enemy.isGroundDetected())
            enemy.Flip();

        BossDirectrionMoveController();
        ShootStateChanger();
    }

    public override void Exit()
    {
        base.Exit();
    }

    void BossDirectrionMoveController()
    {
        changeDirectionTimer -= Time.deltaTime;

        if (player.position.x > bossEnemy.transform.position.x && bossEnemy.facingDir == 1 ||
            player.position.x < bossEnemy.transform.position.x && bossEnemy.facingDir == -1)
        {
            followWrongDirection = true;
            directionTimerReset = true;
        }

        if (followWrongDirection && directionTimerReset)
        {
            changeDirectionTimer = 0.7f;
            directionTimerReset = false;
        }

        if (followWrongDirection && changeDirectionTimer < 0)
        {
            enemy.Flip();
            followWrongDirection = false;
        }
    }

    void ShootStateChanger()
    {
        if (player.position.y - bossEnemy.transform.position.y > 3.9f)
        {
            shootTimer -= Time.deltaTime;

            if (shootTimer <= -2f)
            {
                stateMachine.ChangeState(bossEnemy.shootState);
                shootTimer = 0;
            }
        }
        else
            shootTimer = 0;
    }
}
