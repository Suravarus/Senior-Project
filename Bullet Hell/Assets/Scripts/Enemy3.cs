using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    public Rigidbody2D playerRB;
    private Rigidbody2D enemyRB;
    private Vector2 lookDirection;
    public float speed = 3f;
    public float lowerRange = 3f;
    public float upperRange = 5f;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lookDirection = playerRB.transform.position - transform.position;

    }

    private void FixedUpdate()
    {
        float magnitude = lookDirection.magnitude;
        lookDirection = lookDirection.normalized;
        if (magnitude < lowerRange)//move away from player
        {
            enemyRB.MovePosition(enemyRB.position + -lookDirection * speed * Time.fixedDeltaTime);
        }
        else if (magnitude > upperRange)//move towards player
        {
            enemyRB.MovePosition(enemyRB.position + lookDirection * speed * Time.fixedDeltaTime);
        }

        //no movement otherwise

    }
}