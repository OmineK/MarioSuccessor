using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailState_Dead : EnemyState
{
    Snail snailEnemy;

    public SnailState_Dead(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Snail _snailEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.snailEnemy = _snailEnemy;
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
