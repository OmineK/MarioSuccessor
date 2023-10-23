using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExtraLife : MonoBehaviour
{
    [SerializeField] float wallCheckDistance;
    [SerializeField] LayerMask whatIsGround;

    int facingDir = 1;

    bool facingRight;
    bool isAppear;
    bool canMove;

    Vector3 startPos;

    CapsuleCollider2D capsCollider;
    Rigidbody2D rb;

    void Awake()
    {
        capsCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        capsCollider.enabled = false;
        startPos = transform.position;
        isAppear = true;
        facingRight = true;
    }

    void FixedUpdate()
    {
        if (WallDetected() && canMove)
        {
            if (facingDir > 0 && facingRight || facingDir < 0 && !facingRight)
            {
                facingDir *= -1;
                facingRight = !facingRight;
            }
        } 

        if (isAppear)
            rb.velocity = new Vector3(rb.velocity.x, 2f);

        if (canMove)
            rb.velocity = new Vector3(2.5f * facingDir, rb.velocity.y);
    }

    void Update()
    {
        if (capsCollider.size.y + 0.2f < transform.position.y - startPos.y)
        {
            capsCollider.enabled = true;
            isAppear = false;
            rb.velocity = Vector2.zero;

            Invoke(nameof(CanMove), 0.2f);
        }
    }

    void CanMove() => canMove = true;

    bool WallDetected() => Physics2D.Raycast(transform.position, Vector3.right * facingDir, wallCheckDistance, whatIsGround);

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance, transform.position.y));
    }
}
