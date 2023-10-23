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

        if (xInput == player.facingDir && player.isWallDetected())
        {
            Debug.Log(xInput);
            Debug.Log(player.facingDir);

            return;
        }

        if (xInput != 0)
            stateMachine.ChangeState(player.moveState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
