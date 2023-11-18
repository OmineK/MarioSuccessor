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
    }

    void RotationHandler()
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed * -50f * Time.deltaTime));
    }

    void LifeTimeHandler()
    {
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0)
        {
            AudioManager.instance.PlaySFXwithPitchChange(12);

            GameObject explode = Instantiate(fireballExplosionPref, transform.position, Quaternion.identity);
            explode.GetComponent<ParticleSystem>().startColor = explodeColor;

            Destroy(explode, 0.5f);
            Destroy(this.gameObject);
        }
    }
}
