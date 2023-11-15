using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Idle : PlayerState_Grounded
{
    public PlayerState_Idle(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (player.isWallDetected())
        {
            if (xInput > 0 && player.facingDir == 1 ||
                xInput < 0 && player.facingDir == -1)
                return;
        }

        if (xInput != 0)
            stateMachine.ChangeState(player.moveState);

        if (!player.isGroundDetected() && !player.isSecondGroundDetected())
            stateMachine.ChangeState(player.airState);
    }

    public override void Exit()
    {
        base.Exit();

        player.anim.SetBool("Idle", false);
        player.anim.SetBool("Idle2", false);
        player.anim.SetBool("Idle3", false);
    }
}
