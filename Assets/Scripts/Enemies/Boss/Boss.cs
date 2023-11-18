using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [Space]
    public Transform player;

    [Header("Fireball info")]
    public GameObject bossFireballPref;
    [SerializeField] Transform fireballSpawnTransform;

    [Header("Boss hp info")]
    public int bossHP;

    public BossState_Move moveState { get; private set; }
    public BossState_Shoot shootState { get; private set; }
    public BossState_Dead deadState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        moveState = new BossState_Move(this, stateMachine, "Move", this);
        shootState = new BossState_Shoot(this, stateMachine, "Shoot", this);
        deadState = new BossState_Dead(this, stateMachine, "Shoot", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(moveState);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();

        if (isDead && stateMachine.currentState != deadState)
            stateMachine.ChangeState(deadState);
    }

    public void CreateBossFireball()
    {
        GameObject bossFireball = Instantiate(bossFireballPref, fireballSpawnTransform.position, Quaternion.identity);
        bossFireball.GetComponent<BossFireball>().player = player;
    }
}
