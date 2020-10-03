using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    Vector2 movement;

    //inputs are taken once per frame
    void Update()
    {

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        //slow down if neither are 0, sqrt2 movement in both directions. 
        //0.70710678118 is sqrt(2) / 2
        if (x != 0 && y != 0)
        {
            movement.x = x;
            movement.y = y;
            if (movement.magnitude > 1)
                movement = movement.normalized;
        }
        else
        {
            movement.x = x;
            movement.y = y;
        }
    }

    //called on a timer, not tied to framerate. Called 50 times per second by default.
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
