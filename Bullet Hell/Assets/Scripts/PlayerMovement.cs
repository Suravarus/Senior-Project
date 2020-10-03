
using UnityEngine;
using Combat;

public class PlayerMovement : MonoBehaviour
{
    // FIXME LINE-OF-SITE-TEAM[1] - Is this neccessary? If not, please remove.
    [SerializeField] FieldOfView fieldOfView;
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
        // FIXME LINE-OF-SITE-TEAM[1] - The following line was commented out as it was causing errors in Unity.
        // Please check to see if this line is neccessary or if it can be taken out.
        //
        //fieldOfView.setOrigin(this.transform.position);

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        //slow down if neither are 0, sqrt2 movement in both directions. 
        //0.70710678118 is sqrt(2) / 2
        if (x != 0 && y != 0)
        {
            movement.x = x * 0.70710678118f;
            movement.y = y * 0.70710678118f;
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
        this.combatant.AimRangedWeapon(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        // Shoot weapon if SPACEBAR is pressed
        if (Input.GetKey(KeyCode.Space))
        {
            this.combatant.ShootRangedWeapon();
        }
    }
}
