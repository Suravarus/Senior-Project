using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon : MonoBehaviour
{
    [Header("Ammo")]
    public GameObject cannonAmmo;
    public int baseDamage;
    public float bulletSpeed;
    [Header("Combat")]
    [Tooltip("Bullets per second")]
    public float rateOfFire = 1f;
    public float range = 1f;
    private float _fireDelay;
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
    public GameObject CannonAmmo
    {
        set { this.cannonAmmo = value; }
        get { return this.cannonAmmo; }
    }

    
    void Awake()
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

    private float CalculateDelay()
    {
        return (1f / this.rateOfFire);
    }

    void FixedUpdate()
    {
        //point towards mouse rotating around player
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

        // Check if spacebar has is being pressed
        if (Input.GetKey(KeyCode.Space))
        {
            this.RequestCannonFire();
        }

        if (this.WaitingToFire)
        {
            // update elapsed time
            this.TimeSinceFireRequest += Time.fixedDeltaTime;
            this.FireDelay -= this.TimeSinceFireRequest;
            if (this.FireDelay == 0)
            {
                this.WaitingToFire = false;
            }
        }
    }

    public void RequestCannonFire()
    {
        if (!this.WaitingToFire)
        {
            this.WaitingToFire = true;
            this.Fire();
            this.FireDelay = this.CalculateDelay();
            this.TimeSinceFireRequest = Time.deltaTime;
        }
    }

    /// <summary> fire cannon.</summary>
    /// <exception> Is Ammo NULL? </exception>
    private void Fire()
    {
        try
        {

            var a = this.CannonAmmo.GetComponent<Ammo>();
            // shoot the 'ammo' straight ahead
            if (this.bulletSpeed > 0)
                a.speed = this.bulletSpeed + 10;
            if (this.baseDamage > 0)
                a.baseDamage = this.baseDamage;

            a.ammoOwner = this.GetComponentInParent<Combatant>();
            a.weapon = this;

            var st = this.transform.GetChild(0);
            Debug.Log($"{st.name}, {st.localScale}");

            Instantiate(this.CannonAmmo, st.position, st.rotation);
        }

        catch (Exception ex)
        {
            throw new Exception(
                "Weapon.Fire() - has Ammo been set?", ex);
        }
    }
    
}
