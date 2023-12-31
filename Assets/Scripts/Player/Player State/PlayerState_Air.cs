using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Air : PlayerState
{
    public PlayerState_Air(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.gravityScale = 4f;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (player.isPushed)
            player.SetVelocity(rb.velocity.x, rb.velocity.y);
        else
            player.SetVelocity(player.currentMoveSpeed * 0.8f * xInput, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (player.isGroundDetected() || player.isSecondGroundDetected())
        {
            if (xInput == 0)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = 2f;
    }
}