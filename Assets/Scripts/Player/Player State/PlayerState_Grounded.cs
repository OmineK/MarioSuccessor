using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Grounded : PlayerState
{
    public PlayerState_Grounded(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.sharedMaterial = player.friction;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(player.jumpState);
    }

    public override void Exit()
    {
        base.Exit();

        rb.sharedMaterial = player.frictionless;
    }
}
