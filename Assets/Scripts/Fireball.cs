using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [Header("Movement info")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float rotationSpeed;
    [SerializeField] float maxLifeTime;

    [Header("Collision info")]
    [SerializeField] Transform wallCheck;
    [SerializeField] Transform wallCheck2;
    [SerializeField] float wallCheckDistance;
    [SerializeField] LayerMask whatIsGround;

    [NonSerialized] public float facingDir = 1;

    float lifeTimer;

    bool jump;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        lifeTimer = maxLifeTime;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector3(moveSpeed * facingDir, rb.velocity.y);
        transform.Rotate(new Vector3(0, 0, rotationSpeed * 5f * -facingDir));

        if (jump)
        {
            rb.AddForce(new Vector3(0 , jumpForce / 100, 0));
            jump = false;
        }
    }

    void Update()
    {
        SelfDestroyAfterLifeTime();
    }

    void SelfDestroyAfterLifeTime()
    {
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0)
            Destroy(this.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        jump = true;

        if (WallDetected() || WallDetected2() || WallDetected3())
            Destroy(this.gameObject);

        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy currentEnemy = collision.gameObject.GetComponent<Enemy>();
            GameManager.instance.IncreaseSocre(currentEnemy.scoreValue / 2);
            currentEnemy.Die();
            Destroy(this.gameObject);
        }
    }

    bool WallDetected() => Physics2D.Raycast(wallCheck.position, Vector3.right * facingDir, wallCheckDistance, whatIsGround);
    bool WallDetected2() => Physics2D.Raycast(wallCheck2.position, Vector3.right * facingDir, wallCheckDistance, whatIsGround);
    bool WallDetected3() => Physics2D.Raycast(transform.position, Vector3.right * facingDir, wallCheckDistance, whatIsGround);

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
        Gizmos.DrawLine(wallCheck2.position, new Vector3(wallCheck2.position.x + wallCheckDistance * facingDir, wallCheck2.position.y));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance * facingDir, transform.position.y));
    }
}
