using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public float speed = 5f;

    public Rigidbody2D playerRB; //the players rigid 2d
    private Rigidbody2D enemyRB; //the unit which has the script attached
    private Vector2 lookDirection; //enemy vision direction

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lookDirection = (playerRB.transform.position - transform.position).normalized;
    }
    

    void FixedUpdate()
    {
        //enemyRB.AddForce(lookDirection * speed);
        enemyRB.MovePosition(enemyRB.position + lookDirection * speed * Time.fixedDeltaTime);

        Debug.DrawLine(playerRB.transform.position, transform.position);
    }

}
