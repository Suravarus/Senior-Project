using Combat;
using Combat.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ND_VariaBULLET;

public class Boss1Script : MonoBehaviour
{
    Transform player;
    Transform bossPos;
    ChaserAI movement;
    Rigidbody2D rb;
    CapsuleCollider2D hitbox;
    Combatant bossInfo;
    public SpreadPattern landingShot;
    public SpreadPattern circleShot;

    private bool isJumping = false;
    private int bossPhase = 1;
    private float jumpTime = 1;
    private int jumpSpeed = 20;
    private bool goingUp = false;
    private bool goingDown = false;
    private Vector2 target;
    private Vector2 direction;
    private bool isDead = false;
    private bool firing = false;
    private float fireAngle = 0.0f;

    private int count;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<ChaserAI>();
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<CapsuleCollider2D>();
        bossInfo = GetComponent<Combatant>();
        bossPos = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        target = bossPos.position;
    }

    // Update is called 50 times per second
    void FixedUpdate()
    {
        if(bossPhase == 1 && bossInfo.Health <= (bossInfo.MaxHealth / 2))
        {
            bossPhase = 2;
            jumpTime /= 2;
            jumpSpeed *= 2;
        }

        if (firing)
        {
            circleShot.TriggerAutoFire = false;
            firing = false;
        }


        if (isJumping)
        {
            if (goingUp)
            {
                rb.MovePosition(rb.position + direction * jumpSpeed * Time.fixedDeltaTime);
                if(target.y <= bossPos.position.y) //reached apex of jump
                {
                    goingUp = false;
                    Debug.Log(target.y + " : rising : " + bossPos.position.y );
                    StartCoroutine(Wait(jumpTime));
                }
            }
            else if (goingDown)
            {
                rb.MovePosition(rb.position + direction * jumpSpeed * Time.fixedDeltaTime);
                if (target.y > bossPos.position.y)
                { //make sure the enemy does not fall too far
                    Debug.Log(target.y + " : falling : " + bossPos.position.y);
                    rb.position = new Vector2(rb.position.x, target.y);
                }
                if(rb.position.y == target.y)
                {
                    Landing();
                }
            }
        }
        else
        {
            
            count++;
            if(count % 100 == 0)
            {
                Shoot();
            }
            if(count > 500)
            {
                if ((UnityEngine.Random.Range(0f, 1000f) <= count % 150) || count == 1300) //variable amount of time to jump
                    Jump();
            }
            else if(count == 1)
                landingShot.TriggerAutoFire = false;
        }

    }

    //Jump
    void Jump()
    {
        Debug.Log("count = " + count);
        if (isJumping == false)
        {
            isJumping = true;
            GoUp();
        }
    }

    void Shoot()
    {
        Vector2 temp = new Vector2(player.position.x - bossPos.position.x, player.position.y - bossPos.position.y).normalized;
        
        if(temp.x < 0)
        {
            fireAngle = 360 - (Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg * -1);
        }
        else
        {
            fireAngle = Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg;
        }
        circleShot.CenterRotation = fireAngle;
        Debug.Log("new fire angle is " + fireAngle);
        circleShot.TriggerAutoFire = true;
        firing = true;
    }


    IEnumerator Wait(float sec)
    {

        //yield on a new YieldInstruction that waits for some seconds.
        yield return new WaitForSeconds(sec);

        FallDown();

        yield return new WaitForSeconds(sec * .25f);

        Debug.Log("I am about to fall");
        goingDown = true;
    }

    void GoUp()
    {
        movement.moving = false;
        hitbox.isTrigger = true;
        goingUp = true;
        target = bossPos.position + new Vector3(0, 50, 0);
        Debug.Log("target position" + target);
        direction = (target - (Vector2)bossPos.position);
        direction = direction.normalized;
    }

    void FallDown()
    {
        goingUp = false;
        goingDown = true;
        target = player.position; //players location
        Debug.Log("target position " + target + "boss pos " + bossPos.position);
        direction = (target - (Vector2)bossPos.position).normalized;
        //This is where we would show some animation 
        //suggesting the boss is falling at the player
    }

    void Landing()
    {
        goingDown = false;
        movement.moving = true;
        hitbox.isTrigger = false;
        count = 0;
        isJumping = false;
        landingShot.TriggerAutoFire = true;
    }

}
