
using System;
using UnityEngine;

using Input;
using Combat;


[RequireComponent(typeof(WeaponWielder))]
public class PlayerMovement : MonoBehaviour
{
    // UNITY EDITOR ----------------------------//
    public float speed = 1;
    // ----------------------------------------//

    private WeaponWielder Wielder;
    private Rigidbody2D rb;
    private GameControls Keybindings { get; set; }
    Vector2 Direction { get; set; }
    Vector2 CursorScreenPosition { get; set; }
    Boolean ShootingPressed { get; set; }

    public void Awake()
    {
        this.Direction = Vector2.zero;
        this.CursorScreenPosition = Vector2.zero;

        this.Keybindings = new GameControls();
        this.Keybindings.Movement.Direction.performed += ctx => this.Direction = ctx.ReadValue<Vector2>();
        this.Keybindings.Movement.CursorPosition.performed += ctx => this.CursorScreenPosition = ctx.ReadValue<Vector2>();
        this.Keybindings.Combat.Shoot.performed += ctx => 
        {
            ShootingPressed = ctx.ReadValueAsButton();
        };

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
        //float x = Input.GetAxisRaw("Horizontal");
        //float y = Input.GetAxisRaw("Vertical");
        ////slow down if neither are 0, sqrt2 movement in both directions. 
        ////0.70710678118 is sqrt(2) / 2
        //if (x != 0 && y != 0)
        //{
        //    direction.x = x;
        //    direction.y = y;
        //    if (direction.magnitude > 1)
        //        direction = direction.normalized;
        //}
        //else
        //{
        //    direction.x = x;
        //    direction.y = y;
        //}
    }

    // ALGORITHM:
    //     MOVE player
    //     GET mouse position
    //     AIM weapon toward mouse location
    //     CALL puppetMaster
    //     SHOOT weapon if righ-click is clicked
    public void FixedUpdate()
    {
        if (this.Wielder.IsAlive())
        {
            // MOVE player 
            this.rb.velocity = this.Direction * this.speed;
            // get mouse position
            Vector3 target = Camera.main.ScreenToWorldPoint(CursorScreenPosition);
            // maintain the same z-value
            target.z = this.Wielder.GetWeaponWrapper().transform.position.z;

            // AIM weapon toward mouse location
            this.Wielder.AimWeapon(target);
            // Shoot weapon if player pressed shooting button
            if (this.ShootingPressed)
            {
                this.Wielder.ShootWeapon();
            }
        }
    }

    void OnEnable() { this.Keybindings.Enable(); }
    void OnDisable() { this.Keybindings.Disable(); }

    public Vector2 GetDirection() { return this.Direction; }
    public Vector2 GetCursorPosition() { return this.CursorScreenPosition; }
    public Boolean BtnShootPressed() { return this.ShootingPressed; }
}
