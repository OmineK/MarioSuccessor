using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Dead : PlayerState
{
    public PlayerState_Dead(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0, 10f);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (player.transform.position.y < -6)
        {
            player.SetVelocity(0, 0);
            player.rb.isKinematic = true;
        }
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.isKinematic = false;
    }
}
