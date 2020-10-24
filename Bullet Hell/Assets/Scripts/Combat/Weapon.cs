
using UnityEngine;
using System;

using Combat;

public class Weapon : MonoBehaviour
{
    [Header("Ammo")]
    public GameObject weaponAmmo;
    public Combatant weaponOwner;
    public int baseDamage;
    public float bulletSpeed;
    [Header("Combat")]
    [Tooltip("Bullets per second")]
    public float rateOfFire = 1f;
    public float range = 3f;
    private float _fireDelay;
    public int ammoType = 0;
    public bool infAmmo = false;
    

    // will keep track of the last time this weapon 'fired'
    // private float LastFiredDeltaTime { get; set; }
    // private float TimeOfFireRequest { get; set; }
    private float TimeSinceFireRequest { get; set; }
    // private float ScheduledTimeOfFire { get; set; }
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

    
    void Awake()
    {
        weaponOwner = this.GetComponentInParent<Combatant>();
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
    void FixedUpdate()
    {
        // IF the weapon is waiting to fire:
        if (this.WaitingToFire)
        {
            // UPDATE elapsed time since the last shot was fired
            this.TimeSinceFireRequest += Time.fixedDeltaTime;
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
            Debug.Log("Fired!");
            this.WaitingToFire = true;
            this.Shoot();
            this.FireDelay = this.CalculateDelay();
            this.TimeSinceFireRequest = Time.deltaTime;
        }
    }

    /// <summary> fire cannon.</summary>
    /// <exception> Is Ammo NULL? </exception>
    private void Shoot()
    {
        try
        {
            var a = this.WeaponAmmo.GetComponent<Ammo>();
            // shoot the 'ammo' straight ahead
            if (this.bulletSpeed > 0)
                a.speed = this.bulletSpeed + 10;
            if (this.baseDamage > 0)
                a.damage = this.baseDamage; // FIXME [combat-update IV]

            a.ammoOwner = this.GetComponentInParent<Combatant>();
            a.weapon = this;

            var st = this.transform.GetChild(0);
            //Debug.Log($"{st.name}, {st.localScale}");

            //Use bullets linked to weapon type
            if (infAmmo)
            {
                Instantiate(this.WeaponAmmo, st.position, st.rotation);
            }
            else if (this.ammoType == 0)
            {
                if (this.weaponOwner.smallAmmo > 0)
                {
                    //Debug.Log("Small ammo fired");
                    Instantiate(this.WeaponAmmo, st.position, st.rotation);
                    this.weaponOwner.smallAmmo = this.weaponOwner.smallAmmo - 1;
                }
            }
            else if (this.ammoType == 1)
            {
                if (this.weaponOwner.mediumAmmo > 0)
                {
                    //Debug.Log("Medium ammo fired");
                    Instantiate(this.WeaponAmmo, st.position, st.rotation);
                    this.weaponOwner.mediumAmmo -= 1;
                }
            }
            else if (this.ammoType == 2)
            {
                if (this.weaponOwner.largeAmmo > 0)
                {
                    //Debug.Log("Large ammo fired");
                    Instantiate(this.WeaponAmmo, st.position, st.rotation);
                    this.weaponOwner.largeAmmo -= 1;
                }
            }
        }

        catch (Exception ex)
        {
            throw new Exception(
                "Weapon.Fire() - has Ammo been set?", ex);
        }
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
}
