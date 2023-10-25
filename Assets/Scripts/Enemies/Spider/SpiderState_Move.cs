using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderState_Move : EnemyState
{
    Spider spiderEnemy;

    public SpiderState_Move(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Spider _spiderEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.spiderEnemy = _spiderEnemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        rb.velocity = new Vector3(spiderEnemy.moveSpeed * spiderEnemy.facingDir, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.isDead)
            stateMachine.ChangeState(spiderEnemy.deadState);

        SwitchToFollowState();

        if (enemy.isWallDetected() || !enemy.isGroundDetected())
            enemy.Flip();
    }

    public override void Exit()
    {
        base.Exit();
    }

    void SwitchToFollowState()
    {
        if (spiderEnemy.isPlayerDetected() && spiderEnemy.isGroundBetween())
        {
            if (spiderEnemy.playerHit().distance < spiderEnemy.forwardGroundHit().distance)
                stateMachine.ChangeState(spiderEnemy.followState);
        }

        if (spiderEnemy.isPlayerDetected() && !spiderEnemy.isGroundBetween())
            stateMachine.ChangeState(spiderEnemy.followState);
    }
}
