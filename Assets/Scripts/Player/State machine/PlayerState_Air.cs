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
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        player.SetVelocity(player.moveSpeed * 0.8f * xInput, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (player.isGroundDetected())
            stateMachine.ChangeState(player.idleState);            
    }

    public override void Exit()
    {
        base.Exit();
    }
}
