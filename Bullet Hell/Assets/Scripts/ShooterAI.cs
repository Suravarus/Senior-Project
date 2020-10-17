﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ShooterAI : MonoBehaviour
{
    public float Min_Dist = 1;
    public float Max_Dist = 2;
    public float speed = 3f;
    public float nextWaypointDistance = 1f;

    private Transform target;
    private Vector2 DistanceFromTarget;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player").GetComponent<Transform>();
        //sets target to the player
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
            return;

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
            reachedEndOfPath = false;
        //move along path
        DistanceFromTarget = target.transform.position - transform.position;
        if (DistanceFromTarget.magnitude > Max_Dist)
        {//walk towards path if outside of max distance 

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            rb.MovePosition(rb.position + direction.normalized * speed * Time.deltaTime);

        }
        else if (DistanceFromTarget.magnitude < Min_Dist)
        {//flee if too close
            rb.MovePosition(rb.position + -DistanceFromTarget.normalized * speed * Time.deltaTime);
        }

        //Vector2 force = direction * speed * Time.fixedDeltaTime;

        //rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}