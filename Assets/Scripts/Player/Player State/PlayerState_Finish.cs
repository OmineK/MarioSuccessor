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

        player.SetVelocity(0, 20);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (rb.velocity.y < 0)
            rb.gravityScale = 4f;
        else
            rb.gravityScale = 2f;

        if (player.isGroundDetected() || player.isSecondGroundDetected())
            player.SetVelocity(0, 20);
    }

    public override void Exit()
    {
        base.Exit();

        rb.gravityScale = 2f;
    }
}
