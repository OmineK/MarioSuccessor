using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected Enemy enemy;
    protected EnemyStateMachine stateMachine;
    protected Rigidbody2D rb;

    string animBoolName;

    public EnemyState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        this.enemy = _enemy;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        rb = enemy.rb;
        enemy.anim.SetBool(animBoolName, true);
    }

    public virtual void FixedUpdate()
    {
        //TODO in specific state if enemy influence on physics
    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {
        enemy.anim.SetBool(animBoolName, false);
    }
}
