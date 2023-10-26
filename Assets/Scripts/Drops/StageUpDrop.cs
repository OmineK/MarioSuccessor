using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageUpDrop : DropEntity
{
    [SerializeField] Sprite stage2;
    [SerializeField] Sprite stage3;

    CircleCollider2D circleCollider;
    SpriteRenderer sr;

    protected override void Awake()
    {
        base.Awake();
        circleCollider = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        circleCollider.enabled = false;

        SetupDropSprite();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();

        if ((circleCollider.radius * 2) + 0.2f < transform.position.y - startPos.y)
        {
            circleCollider.enabled = true;
            isAppear = false;
            rb.velocity = Vector2.zero;

            Invoke(nameof(CanMove), 0.2f);
        }
    }

    void SetupDropSprite()
    {
        if (GameManager.instance.playerStage1)
            sr.sprite = stage2;

        if (GameManager.instance.playerStage2)
            sr.sprite = stage3;

        if (GameManager.instance.playerStage3)
            sr.sprite = stage3;
    }
}
