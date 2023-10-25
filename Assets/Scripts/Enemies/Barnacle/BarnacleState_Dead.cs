using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnacleState_Dead : EnemyState
{
    Barnacle barnacleEnemy;

    public BarnacleState_Dead(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Barnacle _barnacleEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.barnacleEnemy = _barnacleEnemy;
    }

    public override void Enter()
    {
        base.Enter();
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
