using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExtraLifeDrop : DropEntity
{
    CapsuleCollider2D capsCollider;

    protected override void Awake()
    {
        base.Awake();
        capsCollider = GetComponent<CapsuleCollider2D>();
    }

    protected override void Start()
    {
        base.Start();
        capsCollider.enabled = false;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();

        if (capsCollider.size.y + 0.2f < transform.position.y - startPos.y)
        {
            capsCollider.enabled = true;
            isAppear = false;
            rb.velocity = Vector2.zero;

            Invoke(nameof(CanMove), 0.2f);
        }
    }
}
