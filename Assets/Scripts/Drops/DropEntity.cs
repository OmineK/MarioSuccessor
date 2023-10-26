using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropEntity : MonoBehaviour
{
    [Header("Drop movement info")]
    [SerializeField] float spawnSpeed;
    [SerializeField] float moveSpeed;

    [SerializeField] float wallCheckDistance;
    [SerializeField] LayerMask whatIsGround;

    protected int facingDir = 1;

    protected bool facingRight;
    protected bool isAppear;
    protected bool canMove;

    protected Vector3 startPos;

    protected Rigidbody2D rb;

    protected virtual void Awake()
    {  
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        startPos = transform.position;
        isAppear = true;
        facingRight = true;
    }

    protected virtual void FixedUpdate()
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
            rb.velocity = new Vector3(rb.velocity.x, spawnSpeed);

        if (canMove)
            rb.velocity = new Vector3(moveSpeed * facingDir, rb.velocity.y);
    }

    protected virtual void Update()
    {
        if (transform.position.y < -7)
            Destroy(this.gameObject);
    }

    protected void CanMove() => canMove = true;

    bool WallDetected() => Physics2D.Raycast(transform.position, Vector3.right * facingDir, wallCheckDistance, whatIsGround);

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance, transform.position.y));
    }
}
