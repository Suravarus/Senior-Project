using System;
using UnityEngine;

namespace Combat
{
    public class Weapon : MonoBehaviour
    {
        [Header("Ammo")]
        public GameObject cannonAmmo;
        public int baseDamage;
        public float bulletSpeed;
        [Header("Combat")]
        [Tooltip("Bullets per second")]
        public float rateOfFire = 1f;
        private float fireDelay;
        // will keep track of the last time this weapon 'fired'
        // private float LastFiredDeltaTime { get; set; }
        // private float TimeOfFireRequest { get; set; }
        private float TimeSinceFireRequest { get; set; }
        // private float ScheduledTimeOfFire { get; set; }
        private bool WaitingToFire { get; set; }
        private float FireDelay { get; set; }
        public Ammo CannonAmmo { get; set; }

        public float RateOfFire
        {
            set { this.rateOfFire = value; }
            get { return this.rateOfFire; }
        }

        void Awake()
        {
            // check that fire rate has not been set to negative.
            if (this.rateOfFire < 0)
                throw new ArgumentOutOfRangeException(
                    "Cannon.Awake() - Rate of Fire cannot be < 0."
                );
            // calculate FireDelay based on rateOfFire
            this.FireDelay = (1f / this.rateOfFire);
            // set initial parameters.
            //this.LastFiredDeltaTime = 0f;
            //this.TimeOfFireRequest = 0f;
            this.TimeSinceFireRequest = 0f;
            //this.ScheduledTimeOfFire = 0f;
            this.WaitingToFire = false;
        }

        void FixedUpdate()
        {
            // Check if spacebar has is being pressed
            if (Input.GetKeyDown(KeyCode.Space) == true)
            {
                this.RequestCannonFire();
            }
            // update elapsed time
            this.TimeSinceFireRequest += Time.deltaTime;
            // check if cannon is waiting to fire
            if (this.WaitingToFire)
            {
                // check if current time == the requested time of fire.
                if (this.TimeSinceFireRequest >= this.FireDelay)
                {
                    // fire cannon
                    this.Fire();
                    // reset wait parameters
                    this.WaitingToFire = false;
                    //this.ScheduledTimeOfFire = 0f;
                    //this.TimeOfFireRequest = 0f;

                }
            }
        }

        public void RequestCannonFire()
        {
            if (this.FireDelay == 0)
            {
                this.Fire();
            }
            else if (!this.WaitingToFire)
            {
                // set the neccessary parameters for a delayed 'fire'
                this.TimeSinceFireRequest = 0f;
                // this.ScheduledTimeOfFire = this.TimeOfFireRequest + this.FireDelay;
                this.WaitingToFire = true;
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
                    a.speed = this.bulletSpeed;
                if (this.baseDamage > 0)
                    a.baseDamage = this.baseDamage;

                Instantiate(this.CannonAmmo,
                this.transform.position,
                this.transform.rotation);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Weapon.Fire() - has Ammo been set?", ex);
            }
        }
    }

}
