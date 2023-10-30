using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] Color firstColor;
    [SerializeField] Color secondColor;
    [SerializeField] float motionRange;
    [SerializeField] float moveSpeed;
    
    public int scoreValue;

    float maxYValue;
    float minYValue;

    bool moveUp;
    bool moveDown;

    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        sr.color = firstColor;
        InvokeRepeating(nameof(changeColor), 0, 0.3f);

        maxYValue = transform.position.y + motionRange;
        minYValue = transform.position.y - motionRange;

        moveDown = true;
    }

    void Update()
    {
        if (moveDown)
            MoveObjectDown();
        
        if (moveUp)
            MoveObjectUp();
    }

    void changeColor()
    {
        if (sr.color ==  firstColor)
            sr.color = secondColor;
        else if (sr.color == secondColor)
            sr.color = firstColor;
    }


    void MoveObjectDown()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - (moveSpeed * Time.deltaTime));

        if (transform.position.y <= minYValue)
        {
            moveUp = true;
            moveDown = false;
        }
    }

    void MoveObjectUp()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + (moveSpeed * Time.deltaTime));

        if (transform.position.y >= maxYValue)
        {
            moveDown = true;
            moveUp = false;
        }
    }
}
