using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Springboard : MonoBehaviour
{
    public float playerStage1PushForce;
    public float playerStage2and3PushForce;

    [NonSerialized] public Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
}
