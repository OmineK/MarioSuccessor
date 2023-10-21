using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBox : MonoBehaviour
{
    [SerializeField] int scoreIncreaseAmount;
    [SerializeField] int hitsNumber;

    Vector3 startPos;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        if (startPos != transform.position)
        {
            rb.velocity = Vector2.down;

            if (transform.position.y <= startPos.y)
            {
                rb.velocity = Vector2.zero;
                transform.position = startPos;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            hitsNumber--;
            GameManager.instance.IncreaseSocre(scoreIncreaseAmount);
            rb.velocity = Vector2.up * 3;

            if (hitsNumber <= 0)
                Destroy(this.gameObject, 0.05f);
        }
    }
}
