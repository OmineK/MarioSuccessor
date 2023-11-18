using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState_Shoot : EnemyState
{
    Boss bossEnemy;

    float shootTimer;
    bool shoot;

    public BossState_Shoot(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Boss _bossEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.bossEnemy = _bossEnemy;
    }

    public override void Enter()
    {
        base.Enter();
        shootTimer = 1;
        shoot = true;

        enemy.SetZeroVelocity();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0.5f && shoot && bossEnemy.player.GetComponent<Player>().isDead == false)
        {
            bossEnemy.CreateBossFireball();
            shoot = false;
        }

        if (shootTimer <= 0)
            stateMachine.ChangeState(bossEnemy.moveState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
