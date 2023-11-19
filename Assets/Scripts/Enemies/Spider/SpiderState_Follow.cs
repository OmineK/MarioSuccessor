using UnityEngine;

public class SpiderState_Follow : EnemyState
{
    float followTimer;
    float changeDirectionTimer;

    bool followWrongDirection = false;
    bool directionTimerReset = false;

    Spider spiderEnemy;
    Transform player;

    public SpiderState_Follow(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Spider _spiderEnemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.spiderEnemy = _spiderEnemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = spiderEnemy.playerHit().transform;
        followTimer = spiderEnemy.followTime;

        spiderEnemy.anim.speed = 1.6f;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        rb.velocity = new Vector3(spiderEnemy.followSpeed * spiderEnemy.facingDir, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        StateTimerHandler();
        SpiderFlipControler();

        if (enemy.isDead)
            stateMachine.ChangeState(spiderEnemy.deadState);

        if (followTimer < 0)
            stateMachine.ChangeState(spiderEnemy.moveState);
    }

    public override void Exit()
    {
        spiderEnemy.anim.speed = 1;

        base.Exit();
    }

    void StateTimerHandler()
    {
        followTimer -= Time.deltaTime;

        if (spiderEnemy.isPlayerDetected() && spiderEnemy.isGroundBetween())
        {
            if (spiderEnemy.playerHit().distance < spiderEnemy.forwardGroundHit().distance)
                followTimer = spiderEnemy.followTime;
        }

        if (spiderEnemy.isPlayerDetected() && !spiderEnemy.isGroundBetween())
            followTimer = spiderEnemy.followTime;
    }

    void SpiderFlipControler()
    {
        changeDirectionTimer -= Time.deltaTime;

        if (player.position.x > spiderEnemy.transform.position.x && spiderEnemy.facingDir == 1 ||
            player.position.x < spiderEnemy.transform.position.x && spiderEnemy.facingDir == -1)
        {
            followWrongDirection = true;
            directionTimerReset = true;
        }

        if (followWrongDirection && directionTimerReset)
        {
            changeDirectionTimer = 0.7f;
            directionTimerReset = false;
        }

        if (followWrongDirection && changeDirectionTimer < 0)
        {
            followWrongDirection = false;
            enemy.Flip();
        }

        if (enemy.isWallDetected() || !enemy.isGroundDetected())
            enemy.Flip();
    }

}
