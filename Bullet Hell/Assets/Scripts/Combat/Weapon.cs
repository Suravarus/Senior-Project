
using UnityEngine;
using System;

using Combat;
using UI;

public class Weapon : MonoBehaviour
{
    // ------ UNITY EDITOR -----------------------------//
    [Header("Development")]
    public Boolean drawGizmo = false;
    [Header("General")]
    public bool inShop = false;
    public int price;
    [Header("Ammo")]
    public GameObject weaponAmmo;
    public Transform ammoSpawnPoint;
    public WeaponWielder wielder;
    public int baseDamage;
    public float bulletSpeed;
    [Header("Combat")]
    [Tooltip("Bullets per second")]
    public float rateOfFire = 1f;
    public float range = 3f;
    private float _fireDelay;
    public bool infAmmo = false;
    public int ammo = 10;

    /// <summary>
    /// The Animator component for the shooting animation. Can be NULL.
    /// </summary>
    [Header("Animation")]
    [Tooltip("Animator this script interacts with in order to trigger a shooting animation. Can be NULL.")]
    public Animator shootingAnimator;
    [Tooltip("Whether this weapon will have to be flipped depending on if it's facing left or right.")]
    public Boolean flipEnabled = false;
    // ------------------------------------------------//

    /// <summary>
    /// Returns TRUE if this weapon has been flipped.
    /// </summary>
    private bool Flipped { get; set; }
    private float TimeSinceFireRequest { get; set; }
    private bool WaitingToFire { get; set; }
    private float FireDelay 
    {
        set
        {
            if (value > 0)
                this._fireDelay = value;
            else
                this._fireDelay = 0;

        }
        get
        {
            return this._fireDelay;
        }
    }
    public GameObject WeaponAmmo
    {
        set { this.weaponAmmo = value; }
        get { return this.weaponAmmo; }
    }

    public Slot UIAmmoSlot 
    {
        set
        {
            this._uiAmmoSlot = value;
            if (this._uiAmmoSlot != null)
            {
                if (!this.infAmmo)
                    this._uiAmmoSlot.SetText(this.ammo.ToString());
                else
                    this._uiAmmoSlot.SetText("---");
            }
        }

        get => this._uiAmmoSlot;
    }

    private Slot _uiAmmoSlot;

    public void Awake()
    {
        // check that fire rate has not been set to negative.
        if (this.rateOfFire <= 0f)
        {
            throw new ArgumentOutOfRangeException($"{this.GetType().Name}.{nameof(this.rateOfFire)}"
                , this.rateOfFire
                , $"Cannot be <= 0.");
        }
        // calculate FireDelay based on rateOfFire
        this.FireDelay = 0;
        // set initial parameters.
        this.TimeSinceFireRequest = 0f;
        this.WaitingToFire = false;

    }

    public void Start()
    {
        this.shootingAnimator = this.GetComponent<Animator>();
    }

    private float CalculateDelay()
    {
        return (1f / this.rateOfFire);
    }

    // ALGORITHM:
    //   IF the weapon is waiting to fire:
    //     UPDATE elapsed time since the last shot was fired
    //     SUBTRACT elapsed time from FireDelay
    //     IF FireDelay is 0:
    //       SET WaitingToFire = FALSE
    public void Update()
    {
        // IF the weapon is waiting to fire:
        if (this.WaitingToFire)
        {
            // UPDATE elapsed time since the last shot was fired (increate rate if we have the power up)
            if (wielder.rateOfFireTimerTemp > 0)
            {
                this.TimeSinceFireRequest += (Time.deltaTime * wielder.rateOfFireTimerStrength) - this.TimeSinceFireRequest;
            }
            else
            {
                this.TimeSinceFireRequest += Time.deltaTime - this.TimeSinceFireRequest;
            }


            // SUBTRACT elapsed time from FireDelay
            this.FireDelay -= this.TimeSinceFireRequest;
            // IF FireDelay is 0:
            if (this.FireDelay == 0)
            {
                // SET WaitingToFire = FALSE
                this.WaitingToFire = false;
            }
        }
    }

    /// <summary>
    ///  Shoots the weapon taking into account the weapon's rate of fire.
    /// </summary>
    /// <author>Johnny Chavez - Combat Team</author>
    public void RequestWeaponFire()
    {
        if (!this.WaitingToFire)
        {
            //Debug.Log("Fired!");
            this.WaitingToFire = true;
            this.Shoot();
            this.FireDelay = this.CalculateDelay();
            this.TimeSinceFireRequest = 0f;
        }
    }

    /// <summary> fire cannon.</summary>
    /// <exception> Is Ammo NULL? </exception>
    private void Shoot()
    {
        if (ammo > 0 || infAmmo == true)
        {
            try
            {
                // IF Animator component is attached
                if (this.shootingAnimator != null)
                {
                    // PLAY shooting animation based on rateOfFire
                    this.shootingAnimator.Play("Shooting", -1, 1f / this.rateOfFire - 0.25f);
                }
                var a = this.WeaponAmmo.GetComponent<Ammo>();
                if(infAmmo == false) ammo = ammo - 1;

                // shoot the 'ammo' straight ahead
                if (this.bulletSpeed > 0)
                    a.speed = this.bulletSpeed;
                if (this.baseDamage > 0)
                    a.damage = this.baseDamage;
                if (wielder.piercingTimerTemp > 0)
                    a.piercingPowerUp = true;
                else
                    a.piercingPowerUp = false;
                a.weapon = this;

                var st = this.transform.GetChild(0);

                Instantiate(a, st.position, st.rotation);

                if (this.UIAmmoSlot != null)
                {   // FIXME similar to assignment code in UIAmmoSlot
                    if (!infAmmo)
                        this.UIAmmoSlot.SetText(this.ammo.ToString());
                    else
                        this.UIAmmoSlot.SetText("---");
                } 
            }
            catch (Exception ex)
            {
                throw new Exception(
                "Weapon.Fire() - has Ammo been set?", ex);
            }
        }
        // TODO give player some feedback when out of ammo
    }
    
    /// <summary>
    /// Returns TRUE if the distance between target and this weapon's
    /// position is within this weapon's range.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public Boolean InRange(Vector3 target)
    {
        return Vector3.Distance(target
            , this.transform.position) <= this.range;
    }

    private Transform GetAmmoSpawnPoint()
    {
        return this.transform.GetChild(0);
    }

    public bool IsFlipped()
    {
        return this.Flipped;
    }

    public void Flip()
    {
        if (!this.Flipped)
        {
            this.Flipped = true;
            this.transform.Rotate(Vector2.right, 180);
        } else
        {
            this.Flipped = false;
            this.transform.Rotate(Vector2.right, 180);
        }
    }

    public Boolean LineOfSight(Combatant c, Combatant.BodyPart bodyPart)
    {
        Boolean los = false;
        // get Bullets layer mask
        int layerMask = LayerMask.GetMask("Bullets");
        // set to all except bullets >> bit-operation
        layerMask = ~layerMask;

        var direction = (c.GetBodyTransform(bodyPart).position - this.GetAmmoSpawnPoint().position).normalized;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(
            this.GetAmmoSpawnPoint().position, 
            direction, 
            this.range, 
            layerMask
            );
        if (raycastHit2D.collider != null)
        {
            
            int objectID = raycastHit2D.collider.gameObject.GetInstanceID();
            if (objectID == c.gameObject.GetInstanceID())
            {
                los = true;
                if (this.drawGizmo)
                {
                    Debug.DrawLine(this.GetAmmoSpawnPoint().position, raycastHit2D.point, Color.yellow, 1, false);
                }
            } else
            {
                if (this.drawGizmo)
                {
                    Debug.DrawLine(this.GetAmmoSpawnPoint().position, raycastHit2D.point, Color.red, 1000, false);
                }
            }
        }

        return los;
    }

}