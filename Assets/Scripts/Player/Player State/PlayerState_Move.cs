using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Move : PlayerState_Grounded
{
    public PlayerState_Move(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (xInput == 0 || player.isWallDetected())
            stateMachine.ChangeState(player.idleState);

        if (!player.isGroundDetected() && !player.isSecondGroundDetected())
            stateMachine.ChangeState(player.airState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
