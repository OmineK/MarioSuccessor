using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBox : MonoBehaviour
{
    [SerializeField] int scoreIncreaseAmount;
    [SerializeField] int hitsNumber;
    [SerializeField] GameObject extraDropPref;

    bool extraDropIsDropped = false;

    Vector3 startPos;

    Rigidbody2D rb;
    SpriteRenderer sr;
    ParticleSystem coinParticle;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        coinParticle = GetComponentInChildren<ParticleSystem>();
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
            rb.velocity = Vector2.up * 3;

            if (extraDropPref == null || extraDropIsDropped)
            {
                AudioManager.instance.PlaySFX(6);
                GameManager.instance.IncreaseSocre(scoreIncreaseAmount);
                coinParticle.Play();
            }
            else
            {
                extraDropIsDropped = true;
                AudioManager.instance.PlaySFX(7);
                Instantiate(extraDropPref, transform.position, Quaternion.identity);
            }

            if (hitsNumber <= 0)
            {
                Invoke(nameof(DisableSpriteRenderer), 0.1f);
                Destroy(this.gameObject, 0.6f);
            }
        }
    }

    void DisableSpriteRenderer() => sr.enabled = false;
}
