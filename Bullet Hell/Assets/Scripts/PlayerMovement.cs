
using UnityEngine;
using Combat;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    Vector2 movement;

    private Combatant combatant;

    void Awake()
    {
        // CHECK for Combatant Component
        var cb = this.GetComponent<Combatant>();
        if (cb != null)
        {
            this.combatant = cb;
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

    // ALGORITHM:
    //     MOVE player
    //     AIM weapon toward mouse location
    //     SHOOT weapon if spacebar is pressed
    void FixedUpdate()
    {
        // move player 
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        // aim weapon toward mouse location
        if (!this.combatant.Disarmed())
        {
            // get mouse position
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // maintain the same z-value
            target.z = this.combatant.RangedWeapon.transform.position.z;
            this.combatant.AimRangedWeapon(target);
        }
            

        // Shoot weapon if RIGHT-CLICK is CLICKED
        if (Input.GetKey(KeyCode.Mouse1))
        {
            this.combatant.ShootRangedWeapon();
        }
    }
}
