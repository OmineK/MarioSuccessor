using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastSnailState_Dead : EnemyState
{
    FastSnail fastSnailEnemy;

    public FastSnailState_Dead(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, FastSnail _fastSnailEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.fastSnailEnemy = _fastSnailEnemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetVelocity(0, 3);
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
