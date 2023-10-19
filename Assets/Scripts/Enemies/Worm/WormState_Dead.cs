using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormState_Dead : EnemyState
{
    Worm wormEnemy;

    public WormState_Dead(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Worm _wormEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.wormEnemy = _wormEnemy;
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
