using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogState_Dead : EnemyState
{
    Frog frogEnemy;

    public FrogState_Dead(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Frog _frogEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.frogEnemy = _frogEnemy;
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
