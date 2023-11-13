using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Finish : PlayerState
{
    public PlayerState_Finish(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(rb.velocity.x, player.currentJumpForce);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if (rb.velocity.y < 0)
            rb.gravityScale = 4f;
        else
            rb.gravityScale = 2f;

        if (player.isGroundDetected() || player.isSecondGroundDetected())
            stateMachine.ChangeState(player.finishState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
