public class PlayerState_Move : PlayerState_Grounded
{
    public PlayerState_Move(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (!player.isPushed)
            player.SetVelocity(xInput * player.currentMoveSpeed, rb.velocity.y);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!player.isPushed)
            player.SetVelocity(xInput * player.currentMoveSpeed, rb.velocity.y);
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
