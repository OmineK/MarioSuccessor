using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireball : MonoBehaviour
{
    [Header("Movement info")]
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float maxLifeTime;

    [Header("Explosion info")]
    [SerializeField] GameObject fireballExplosionPref;
    [SerializeField] Color explodeColor;

    [NonSerialized] public Transform player;

    float lifeTimer;

    void Start()
    {
        lifeTimer = maxLifeTime;
    }

    void Update()
    {
        RotationHandler();
        LifeTimeHandler();
        Movement();

        if (player.GetComponent<Player>().isDead)
            FireballExplosion();
    }

    void RotationHandler()
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed * -50f * Time.deltaTime));
    }

    void LifeTimeHandler()
    {
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0)
            FireballExplosion();
    }

    void Movement()
    {
        if (player.transform != null)
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void FireballExplosion()
    {
        AudioManager.instance.PlaySFX(12);

        GameObject explosion = Instantiate(fireballExplosionPref, transform.position, Quaternion.identity);
        explosion.transform.localScale = transform.localScale;
        var explosionMain = explosion.GetComponent<ParticleSystem>().main;
        explosionMain.startColor = explodeColor;

        Destroy(explosion.gameObject, 0.5f);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.GetComponent<Player>() != null)
        {
            if (!player.GetComponent<Player>().isDead)
                FireballExplosion();
        }
    }
}
