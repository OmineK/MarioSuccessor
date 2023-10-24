using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogState_Air : EnemyState
{
    Frog frogEnemy;

    public FrogState_Air(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Frog _frogEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.frogEnemy = _frogEnemy;
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

        if (enemy.isGroundDetected())
            stateMachine.ChangeState(frogEnemy.idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
