using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderState_Dead : EnemyState
{
    Spider spiderEnemy;

    public SpiderState_Dead(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Spider _spiderEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.spiderEnemy = _spiderEnemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetVelocity(0, 4);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
