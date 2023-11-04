using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FireObstacle : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] Transform[] fires;
    [SerializeField] GameObject fireBody;
    [SerializeField] Transform mainBody;

    [Header("Rotate direction")]
    [SerializeField] bool goRight;
    [SerializeField] bool goLeft;

    float rotateDir;
    float worldRotateDir;
    float zAngle;

    void Start()
    {
        if (goRight)
        {
            goLeft = false;
            rotateDir = 1;
        }
        else if (goLeft)
            rotateDir = -1;
        else if (!goRight && !goLeft)
        {
            goRight = true;
            rotateDir = 1;
        }
    }

    void Update()
    {
        FiresRotation();
    }

    void FiresRotation()
    {
        zAngle += 100 * (rotationSpeed / 2) * Time.deltaTime * rotateDir;

        foreach (Transform fire in fires)
        {
            fire.localRotation = Quaternion.Euler(0, 0, zAngle);
        }

        fireBody.transform.RotateAround(mainBody.transform.position, new Vector3(0, 0, 360), rotationSpeed * 10 * Time.deltaTime * rotateDir);
    }
}
