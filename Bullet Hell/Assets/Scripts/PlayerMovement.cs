
using System;
using UnityEngine;

using Combat;


[RequireComponent(typeof(WeaponWielder))]
public class PlayerMovement : MonoBehaviour
{
    // UNITY EDITOR ----------------------------//
    public float speed = 1;
    // ----------------------------------------//

    private WeaponWielder Wielder;
    private Rigidbody2D rb;
    Vector2 direction;
    

    public void Awake()
    {
        // CHECK for Combatant Component
        var cb = this.GetComponent<WeaponWielder>();
        if (cb != null)
        {
            this.Wielder = cb;
            this.rb = this.Wielder.GetComponent<Rigidbody2D>();
        }
        else
        {
            throw new MissingReferenceException(
                $"GameObject {this.gameObject.name} is missing component {new Combatant().GetType().Name}");
        }
    }

    //inputs are taken once per frame
    public void Update()
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
    bool calc = true;
    public void FixedUpdate()
    {
        
        if (this.Wielder.IsAlive())
        {
            // MOVE player 
            this.rb.velocity = this.direction * this.speed;
            //this.rb.MovePosition(
            //    this.rb.position + this.movement * this.speed * Time.fixedDeltaTime);
            // get mouse position

            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // maintain the same z-value
            target.z = this.Wielder.GetWeaponWrapper().transform.position.z;

            //var a = (Vector2)(this.Wielder.RangedWeapon.ammoSpawnPoint.transform.position - this.Wielder.rangedWeaponWrapper.transform.position);
            //var b = (Vector2)(target - this.Wielder.rangedWeaponWrapper.transform.position);
            //Debug.Log($"weapon {a}");
            //Debug.Log($"mouse {b}");
            //var ang = Vector2.SignedAngle(a, b);
            //Debug.Log($"angle {ang}");
            //if (Mathf.Abs(ang) > 0)
            //{
            //    Debug.Log($"start {this.Wielder.RangedWeapon.transform.position}");
            //    this.Wielder.rangedWeaponWrapper.transform.RotateAround(this.Wielder.rangedWeaponWrapper.transform.position, Vector3.forward, ang + ang * .1f);
            //    Debug.Log($"after {this.Wielder.RangedWeapon.transform.position}");
            //}
            // AIM weapon toward mouse location
            this.Wielder.AimWeapon(target);
            // CALL puppetMaster
            // Shoot weapon if RIGHT-CLICK is CLICKED
            if (Input.GetKey(KeyCode.Mouse1))
            {
                this.Wielder.ShootWeapon();
            }
        }
    }
}
