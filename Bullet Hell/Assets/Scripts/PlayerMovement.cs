
using System;
using UnityEngine;

using Combat;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Combatant))]
public class PlayerMovement : MonoBehaviour
{
    // UNITY EDITOR ----------------------------//
    public float speed = 1;
    // ----------------------------------------//

    private Combatant combatant;
    private Rigidbody2D rb;
    Vector2 direction;
    

    void Awake()
    {
        // CHECK for Combatant Component
        var cb = this.GetComponent<Combatant>();
        if (cb != null)
        {
            this.combatant = cb;
            this.rb = this.combatant.GetComponent<Rigidbody2D>();
        }
        else
        {
            throw new MissingReferenceException(
                $"GameObject {this.gameObject.name} is missing component {new Combatant().GetType().Name}");
        }
    }

    //inputs are taken once per frame
    void Update()
    {

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        //slow down if neither are 0, sqrt2 movement in both directions. 
        //0.70710678118 is sqrt(2) / 2
        if (x != 0 && y != 0)
        {
            direction.x = x;
            direction.y = y;
            if (direction.magnitude > 1)
                direction = direction.normalized;
        }
        else
        {
            direction.x = x;
            direction.y = y;
        }
    }

    // ALGORITHM:
    //     MOVE player
    //     GET mouse position
    //     AIM weapon toward mouse location
    //     CALL puppetMaster
    //     SHOOT weapon if righ-click is clicked
    void FixedUpdate()
    {
        
        if (this.combatant.IsAlive() && !this.combatant.Disarmed())
        {
            // MOVE player 
            this.rb.velocity = this.direction * this.speed;
            //this.rb.MovePosition(
            //    this.rb.position + this.movement * this.speed * Time.fixedDeltaTime);
            // get mouse position
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // maintain the same z-value
            target.z = this.combatant.RangedWeapon.transform.position.z;
            // AIM weapon toward mouse location
            this.combatant.AimRangedWeapon(target);
            // CALL puppetMaster
            // Shoot weapon if RIGHT-CLICK is CLICKED
            if (Input.GetKey(KeyCode.Mouse1))
            {
                this.combatant.ShootRangedWeapon();
            }
        }
    }
}
