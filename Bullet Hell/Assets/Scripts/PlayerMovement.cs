
using System;
using UnityEngine;
using UnityEngine.InputSystem;

using Input;
using Combat;
using Combat.UI;


[RequireComponent(typeof(WeaponWielder))]
public class PlayerMovement : MonoBehaviour
{
    // UNITY EDITOR ----------------------------//
    public float dash_strength = 2f;
    public float dash_timer = 0.4f;
    public MoveState move_state = MoveState.Move;
    // ----------------------------------------//

    public float dash_timer_temp = 0;
    private float temp_speed = 0;
    [Tooltip("units per second")]
    public float speed = 1;
    // ----------------------------------------//

    private WeaponWielder Wielder;
    private WeaponBarUI WeaponBar { get => this.Wielder.weaponBarUI; }
    private Rigidbody2D rb;
    private GameControls Keybindings { get; set; }
    Vector2 Direction { get; set; }
    Vector2 CursorScreenPosition { get; set; }
    Boolean ShootingPressed { get; set; }

    public enum MoveState
    {
        Move,
        Dash
    }

    void Awake()
    {
        // INITIALIZATIONS
        this.Direction = Vector2.zero;
        this.CursorScreenPosition = Vector2.zero;
        this.Keybindings = new GameControls();


        // GET COMPONENTS
        var cb = this.GetComponent<WeaponWielder>();
        if (cb != null)
        {
            this.Wielder = cb;
            this.Wielder.weaponBarUI.Wielder = this.Wielder;
            this.rb = this.Wielder.GetComponent<Rigidbody2D>();
        }
        else
        {
            throw new MissingReferenceException(
                $"GameObject {this.gameObject.name} is missing component {new Combatant().GetType().Name}");
        }

        // WRITE Weaponbar Bindings
        this.WeaponBar.PostStart.Add(wbar => {
            string[] binds = new string[3];

            binds[0] = this.Keybindings.WeaponBar.Cast_1.bindings.ToArray()[0].ToDisplayString();
            binds[1] = this.Keybindings.WeaponBar.Cast_2.bindings.ToArray()[0].ToDisplayString();
            binds[2] = this.Keybindings.WeaponBar.Cast_3.bindings.ToArray()[0].ToDisplayString();

            wbar.SetKeyBinds(binds);
        });
    }

    void Start()
    {
        
        // MOVEMENT LISTENERS
        this.Keybindings.Movement.Direction.performed += ctx => this.Direction = ctx.ReadValue<Vector2>();
        this.Keybindings.Movement.CursorPosition.performed += ctx => this.CursorScreenPosition = ctx.ReadValue<Vector2>();
        this.Keybindings.Combat.Shoot.performed += ctx => ShootingPressed = ctx.ReadValueAsButton();
        this.Keybindings.Movement.Dash.performed += ctx =>
        {
            var pressedDash = ctx.ReadValueAsButton();
            Debug.Log($"dash {pressedDash}");
            //dashing
            if (pressedDash)
            {
                this.move_state = MoveState.Dash;
                dash_timer_temp = dash_timer;
                temp_speed = speed;
                speed = speed * dash_strength;
            }
        };
        
        // WEAPON-BAR LISTENERS
        this.Keybindings.WeaponBar.Cast_1.performed += ctx => this.WeaponBar.EquipWeaponAt(0);
        this.Keybindings.WeaponBar.Cast_2.performed += ctx => this.WeaponBar.EquipWeaponAt(1);
        this.Keybindings.WeaponBar.Cast_3.performed += ctx => this.WeaponBar.EquipWeaponAt(2);
    }

    public void FixedUpdate()
    {
        if (this.Wielder.IsAlive())
        {

            switch (this.move_state)
            {
                case MoveState.Move:
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
                    break;

                case MoveState.Dash:
                    //no inputs, countdown timer
                    dash_timer_temp = dash_timer_temp - Time.deltaTime;
                    if (dash_timer_temp < 0)
                    {
                        //return to normal movement
                        move_state = MoveState.Move;
                        speed = temp_speed;
                    }
                    else if (dash_timer_temp < dash_timer / 4)
                    {
                        //slow down for end of dash 
                        speed = dash_strength * 0.4f * temp_speed;
                    }
                    break;

            }

            
        }
    }

    void OnEnable() { this.Keybindings.Enable(); }
    void OnDisable() { if (this.Keybindings != null) this.Keybindings.Disable(); }

    public Vector2 GetDirection() { return this.Direction; }
    public Vector2 GetCursorPosition() { return this.CursorScreenPosition; }
    public Boolean BtnShootPressed() { return this.ShootingPressed; }
}
