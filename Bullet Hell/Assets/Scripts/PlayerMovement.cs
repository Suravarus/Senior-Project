
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

        // WEAPON-BAR LISTENERS
        this.Keybindings.WeaponBar.Cast_1.performed += ctx => this.WeaponBar.EquipWeaponAt(0);
        this.Keybindings.WeaponBar.Cast_2.performed += ctx => this.WeaponBar.EquipWeaponAt(1);
        this.Keybindings.WeaponBar.Cast_3.performed += ctx => this.WeaponBar.EquipWeaponAt(2);
    }

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
    void OnDisable() { if (this.Keybindings != null) this.Keybindings.Disable(); }

    public Vector2 GetDirection() { return this.Direction; }
    public Vector2 GetCursorPosition() { return this.CursorScreenPosition; }
    public Boolean BtnShootPressed() { return this.ShootingPressed; }
}
