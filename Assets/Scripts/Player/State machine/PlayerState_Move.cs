using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Move : PlayerState
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

        player.SetVelocity(xInput * player.playerMoveSpeed, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (xInput == 0)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
