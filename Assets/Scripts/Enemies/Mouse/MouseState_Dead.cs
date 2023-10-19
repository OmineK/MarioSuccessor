using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseState_Dead : EnemyState
{
    Mouse mouseEnemy;

    public MouseState_Dead(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Mouse _mouseEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.mouseEnemy = _mouseEnemy;
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
