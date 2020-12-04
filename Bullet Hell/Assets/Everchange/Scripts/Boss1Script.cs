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

    [Header("Projectiles")]

    [Tooltip("Number of frames between shooting at the player.")]
    public int shootingInterval = 100;

    [Tooltip("Minimum number of frames between landing and jumping again.")]
    public int lowerJumpingInterval = 500;

    [Tooltip("Maximum number of framess between landing and jumping again.")]
    public int upperJumpingInterval = 1300;


    [Header("Shot types")]
    public SpreadPattern landingShot;
    public SpreadPattern circleShot;
    public FireBullet mainShot;


    private bool isJumping = false;
    private int bossPhase = 1;
    private float jumpTime = 1;
    private int jumpSpeed = 20;
    private bool goingUp = false;
    private bool goingDown = false;
    private Vector2 target;
    private Vector2 direction;
    Vector2 roomCenter;
    private bool bulletHellPhase = false;
    private float fireAngle = 0.0f;
    private GameObject shadow;
    private bool shadowFollowing = false;

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
        roomCenter = bossPos.position;
    }

    // Update is called 50 times per second
void FixedUpdate()
    {
        if(player != null) { 
            if(bossPhase == 1 && bossInfo.Health <= (bossInfo.MaxHealth / 2) && !isJumping)
            {
                bossPhase = 2;
                jumpTime /= 2;
                jumpSpeed *= 2;
                landingShot.setFirstEmitterSpeed(15);
                landingShot.cloneFirstEmitter();
            }
            if (shadowFollowing)
            {
                if (bossPhase == 1)
                    shadow.transform.position = Vector2.MoveTowards(shadow.transform.position, player.position, jumpSpeed * Time.deltaTime);
                else
                    shadow.transform.position = Vector2.MoveTowards(shadow.transform.position, roomCenter, jumpSpeed * Time.deltaTime);
            }
            if (isJumping)
            {
                if (goingUp)
                {
                    rb.MovePosition(rb.position + direction * jumpSpeed * Time.fixedDeltaTime);
                    if (!shadowFollowing && (target.y - bossPos.position.y) < 20)
                        shadowFollowing = true;
                    if (target.y <= bossPos.position.y) //reached apex of jump
                    {
                        goingUp = false;
                        Debug.Log(target.y + " : rising : " + bossPos.position.y);
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
                    if (rb.position.y == target.y)
                    {
                        Landing();
                    }
                }
            }
            else if (!bulletHellPhase)
            {
                count++;
                if (count % shootingInterval == shootingInterval - 1)
                {
                    Aim(circleShot);
                }
                else if (count % shootingInterval == 0)
                {
                    mainShot.InstantiateShot();
                }
                if (count > lowerJumpingInterval)
                {
                    if ((UnityEngine.Random.Range(0f, 1000f) <= count % 150) || count >= upperJumpingInterval) //variable amount of time to jump
                        Jump();
                }
                else if (count == 1)
                    landingShot.TriggerAutoFire = false;
            }
            else
                Hell();
        }
        else
        {
            movement.moving = false;
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

    //Fire at current angle for given SpreadPattern
    void Shoot(SpreadPattern shooting)
    {
        Debug.Log("fire angle for " + shooting + " is " + shooting.CenterRotation);
        shooting.TriggerAutoFire = true;
    }

    //Aim given SpreadPattern to the current player location
    void Aim(SpreadPattern shooting)
    {
        Vector2 temp = new Vector2(player.position.x - bossPos.position.x, player.position.y - bossPos.position.y).normalized;

        if (temp.x < 0)
        {
            fireAngle = 360 - (Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg * -1);
        }
        else
        {
            fireAngle = Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg;
        }
        shooting.CenterRotation = fireAngle;
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
        if (bossPhase > 1)
            bulletHellPhase = true;
        movement.moving = false;
        hitbox.isTrigger = true;
        goingUp = true;
        target = bossPos.position + new Vector3(0, 50, 0);
        Debug.Log("target position" + target);
        direction = (target - (Vector2)bossPos.position);
        direction = direction.normalized;

        //shadow of boss
        Vector3 temp = new Vector3(bossPos.position.x, bossPos.position.y, 1);
        shadow = Resources.Load<GameObject>("Prefabs/Indicators/Red Indicator");
        shadow = Instantiate(shadow, temp, Quaternion.identity);
    }

    void FallDown()
    {
        goingUp = false;
        goingDown = true;
        if (!bulletHellPhase)
            target = player.position; //players location
        else
            target = roomCenter;
        Debug.Log("target position " + target + "boss pos " + bossPos.position);
        direction = (target - (Vector2)bossPos.position).normalized;
        shadowFollowing = false;
    }

    void Landing()
    {
        goingDown = false;
        hitbox.isTrigger = false;
        count = 0;
        isJumping = false;
        Destroy(shadow);
        if (!bulletHellPhase)
        {
            movement.moving = true;
            Shoot(landingShot);
        }
    }

    void Hell()
    {
        count++;
        landingShot.TriggerAutoFire = true;
        if (landingShot.CenterRotation > 180)
            landingShot.CenterRotation -= 355;
        else
            landingShot.CenterRotation += 5;


        if(count >= 500)
        {
            count = 0;
            bulletHellPhase = false;
            movement.moving = true;

        }
    }


}
