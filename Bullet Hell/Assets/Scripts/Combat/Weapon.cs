
using UnityEngine;
using System;

using Combat;

public class Weapon : MonoBehaviour
{
    // ------ UNITY EDITOR -----------------------------//
    [Header("Development")]
    public Boolean drawGizmo = false;
    [Header("General")]
    public int price;
    [Header("Ammo")]
    public GameObject weaponAmmo;
    public Transform ammoSpawnPoint;
    public Combatant weaponOwner;
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
    // ------------------------------------------------//

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

    private void Start()
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
    void Update()
    {
        // IF the weapon is waiting to fire:
        if (this.WaitingToFire)
        {
            // UPDATE elapsed time since the last shot was fired
            this.TimeSinceFireRequest += Time.deltaTime - this.TimeSinceFireRequest;
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
                    a.speed = this.bulletSpeed + 10;
                if (this.baseDamage > 0)
                    a.damage = this.baseDamage;

                a.ammoOwner = this.GetComponentInParent<Combatant>();
                a.weapon = this;

                var st = this.transform.GetChild(0);

                Instantiate(a, st.position, st.rotation);

            }
            catch (Exception ex)
            {
                throw new Exception(
                "Weapon.Fire() - has Ammo been set?", ex);
            }
        }
        else
        {
            //if there is no ammo, make a poof sound?

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

    private Transform GetAmmoSpawnPoint()
    {
        return this.transform.GetChild(0);
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