using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Jump : PlayerState
{
    public PlayerState_Jump(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(rb.velocity.x, player.jumpForce);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        player.SetVelocity(player.moveSpeed * xInput, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.airState);

        if (player.isDead)
            stateMachine.ChangeState(player.deadState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}