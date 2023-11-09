using UnityEngine;

public class PlayerState_Jump : PlayerState
{
    float jumpStateTimer;

    public PlayerState_Jump(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        jumpStateTimer = 0.5f;

        player.SetVelocity(rb.velocity.x, player.currentJumpForce);

        player.aM.PlaySFX(4);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (player.canMove)
            player.SetVelocity(player.currentMoveSpeed * xInput, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        jumpStateTimer -= Time.deltaTime;

        if (jumpStateTimer < 0)
        {
            if (player.isGroundDetected() || player.isSecondGroundDetected())
                stateMachine.ChangeState(player.idleState);
        }

        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.airState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
