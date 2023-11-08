using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] float moveSpeed;

    int currentWaypointIndex;

    Transform targetWaypoint;
    CapsuleCollider2D cd;

    void Awake()
    {
        cd = GetComponent<CapsuleCollider2D>();
    }

    void Start()
    {
        targetWaypoint = waypoints[0];
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.01f)
            targetWaypoint = GetNextWaypoint();
    }

    Transform GetNextWaypoint()
    {
        currentWaypointIndex++;

        if (currentWaypointIndex >= waypoints.Length)
            currentWaypointIndex = 0;

        return waypoints[currentWaypointIndex];
    }
}
