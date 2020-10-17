using Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Boss1Script : MonoBehaviour
{
    public Transform player;

    //ChaserAI movement;
    Rigidbody2D rb;
    CapsuleCollider2D hitbox;
    Combatant bossInfo;
    Transform bossPos;

    private bool isJumping = false;
    private int bossPhase = 1;
    private float jumpTime = 1;
    private int jumpSpeed = 3;
    private bool goingUp = false;
    private bool goingDown = false;
    private Transform target;
    private Vector2 direction;

    private int count;

    // Start is called before the first frame update
    void Start()
    {
        //movement = GetComponent<ChaserAI>();
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<CapsuleCollider2D>();
        bossInfo = GetComponent<Combatant>();
        bossPos = GetComponent<Transform>();
        target = bossPos;
    }

    // Update is called 50 times per second
    void FixedUpdate()
    {
        Debug.Log("Update frame");
        if(bossPhase == 1 && bossInfo.Health <= (bossInfo.MaxHealth / 2))
        {
            bossPhase = 2;
            jumpTime = 0.5f;
            jumpSpeed = 6;
        }

        if (isJumping)
        {
            if (goingUp)
            {
                rb.MovePosition(rb.position + direction * jumpSpeed * Time.fixedDeltaTime);
                if(target.position.y <= bossPos.position.y) //reached apex of jump
                {
                    Debug.Log("About to wait");
                    goingUp = false;
                    StartCoroutine(Wait(jumpTime));
                }
            }
            else if (goingDown)
            {
                rb.MovePosition(rb.position + direction * jumpSpeed * Time.fixedDeltaTime);
                Debug.Log("I'm falling");
                if (rb.position.y < target.position.y)
                { //make sure the enemy does not fall too far
                    rb.position = new Vector2(rb.position.x, target.position.y);
                }
                if(rb.position.y == target.position.y)
                {
                    Landing();
                }
            }
        }
        else
        {
            count++;
            if(count > 200)
            {
                //if (Random.Range(0f, 1000f) == 20) //variable amount of time to jump
                    Jump();
            }
        }

    }

    //Jump
    void Jump()
    {
        if (isJumping == false)
        {
            isJumping = true;
            GoUp();
        }



    }
    IEnumerator Wait(float sec)
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for some seconds.
        yield return new WaitForSeconds(sec);

        FallDown();

        yield return new WaitForSeconds(sec * .25f);

        Debug.Log("I am about to fall");
        goingDown = true;
        //After we have waited some seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    void GoUp()
    {
        //movement.moving = false;
        hitbox.enabled = false;
        goingUp = true;
        target.position = bossPos.position + new Vector3(0, 50, 0);
        direction = (target.position - bossPos.position).normalized;
    }

    void FallDown()
    {
        goingUp = false;
        goingDown = true;
        target.position = player.position; //players location
        direction = (target.position - bossPos.position).normalized;
        //This is where we would show some animation 
        //suggesting the boss is falling at the player
    }

    void Landing()
    {

        goingDown = false;
        //movement.moving = true;
        hitbox.enabled = true;
        count = 0;
    }

}
