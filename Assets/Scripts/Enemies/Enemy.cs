using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Movement info")]
    public float moveSpeed;

    [Header("Game score info")]
    public int scoreValue;

    public EnemyStateMachine stateMachine { get; private set; }

    public bool isDead;

    CapsuleCollider2D capsuleCollider;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        if (isDead) return;

        base.FixedUpdate();

        stateMachine.currentState.FixedUpdate();
    }

    protected override void Update()
    {
        if (isDead) return;

        base.Update();

        stateMachine.currentState.Update();
    }

    public virtual void Die()
    {
        isDead = true;

        SetVelocity(0, 3);

        if (capsuleCollider != null)
            capsuleCollider.isTrigger = true;

        Destroy(this.gameObject, 2.2f);
    }
}
