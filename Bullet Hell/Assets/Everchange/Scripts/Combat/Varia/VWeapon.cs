using System;
using System.Collections.Generic;
using UnityEngine;
using ND_VariaBULLET;

using UI;
using Loot;
using Combat.Audio;

namespace Combat.Varia
{
    public class VWeapon : GameItem, IWeapon
    {
        // UNITY EDITOR ----------------------//
        [Header("Combat")]
        public Transform __gunBarrel;
        [HideInInspector]
        public WeaponWielder __wielder;
        public int __baseDamage;
        public bool __infAmmo = false;
        [Min(0)]
        public int __ammoCount = 0;
        public float __range = 3f;
        [Tooltip("Bullets per second")]
        [Range(0.01f,50)]
        public float __fireRate = 1;
        [Header("Animation")]
        [Tooltip("Whether this weapon will have to be flipped depending on if it's facing left or right.")]
        public bool __flipEnabled = false;
        [Tooltip("Animator this script interacts with in order to trigger a shooting animation. Can be NULL.")]
        public Animator shootingAnimator;
        // -----------------------------------//

        // PROPERTIES
        private int _ammoCount;
        private float _fireDelay;
        // ACCESSORS
        public float Range { get; set; }
        private float BaseDamage { get; set; }
        public BasePattern[] Controllers { get; set; }
        private bool FlipEnabled { get; set; }
        private bool Flipped { get; set; }
        public Slot UIAmmoSlot { get; set; }
        public bool InfiniteAmmo { get; set; }
        public int AmmoCount
        {
            set
            {
                this._ammoCount = value;
                if (this.UIAmmoSlot != null)
                    UpdateAmmoSlot();
            }
            get => this._ammoCount;
        }
        public IAmmo WeaponIAmmo { get; set; }
        public Animator ShootingAnimator { get; set; }
        public WeaponWielder Wielder { get; set; }
        private bool TriggerPulled { get; set; }
        private float TriggerDownFrameCount { get; set; }
        private bool FlippedControllers { get; set; }
        private WeaponSounds Speaker { get; set; }
        private List<FireBullet> Emitters { get; set; }
        // fixme - new vweapon  code
        private float FireDelay
        {
            get;set;
        }
        private float TimeSinceFireRequest { get; set; }
        private bool WaitingToFire { get; set; }
        private float FireRate { get; set; }
        // METHODS
        public void Flip()
        {
            if (!this.Flipped)
            {
                this.Flipped = true;
                this.GetGunBarrel().transform.Rotate(Vector2.right, 180f);
                for(int i = 0; i < this.transform.childCount; i++)
                {
                    this.transform.GetChild(i).transform.Rotate(Vector2.right, 180f);
                }
            }
            else
            {
                this.Flipped = false;
                this.GetGunBarrel().transform.Rotate(Vector2.right, 180f);
                for (int i = 0; i < this.transform.childCount; i++)
                {
                    this.transform.GetChild(i).transform.Rotate(Vector2.right, 180f);
                }
            }
        }

        public float GetBaseDamage() => this.BaseDamage;

        public float GetRange() => this.Range;

        public bool InRange(Vector3 target)
        {
            return Vector3.Distance(
                target,
                this.transform.position) <= this.Range;
        }

        public bool IsFlipped() => this.Flipped;
        // FIXME VI - needs implementation
        public void RequestWeaponFire()
        {
            if (!this.WaitingToFire)
            {
                this.WaitingToFire = true;
                this.Shoot();
                this.FireDelay = this.CalculateDelay() - this.FireDelay;
                this.TimeSinceFireRequest = 0f;
            }
        }
        private void Shoot()
        {
            if (this.InfiniteAmmo || this.AmmoCount > 0)
            {
                
                try
                {
                    // IF Animator component is attached
                    if (this.shootingAnimator != null)
                    {
                        // PLAY shooting animation based on rateOfFire
                        this.shootingAnimator.Play("Shooting", 0, 1f / this.FireRate - 0.25f);
                    }
                    // PLAY shooting sound
                    if (this.Speaker != null)
                        this.Speaker.PlaySound(WeaponSounds.Sounds.Shot);

                    // instantiate bullets
                    foreach (FireBullet fb in this.Emitters)
                    {
                        fb.InstantiateShot();
                    }
                    if (!this.InfiniteAmmo) this.AmmoCount -= 1;

                    // update ammo ui if exists
                    if (this.UIAmmoSlot != null)
                        UpdateAmmoSlot();
                } catch(Exception ex)
                {
                    throw new Exception(
                        "Weapon.Fire() - has Ammo been set?", ex);
                }
            } else if (this.Speaker != null)
            {
                this.Speaker.PlaySound(WeaponSounds.Sounds.NoAmmo);
            }
        }
        private float CalculateDelay()
        {
            return (1f / this.FireRate);
        }

        public void StopFiring()
        {
            //foreach (BasePattern bp in this.Controllers)
            //{
            //    bp.TriggerAutoFire = false;
            //}
            this.TriggerPulled = false;
        }

        private void UpdateAmmoSlot()
        {
            if (!this.InfiniteAmmo)
                this.UIAmmoSlot.SetText($"{this.AmmoCount:D3}");
            else
                this.UIAmmoSlot.SetText("INF");
        }

        public GameObject GetGameObject() => this.gameObject;
        public bool RequiresFlip() => this.FlipEnabled;
        public GameObject GetGunBarrel() => this.gameObject.transform.parent.gameObject;
        public WeaponSounds GetSpeaker() => this.Speaker;
        // MONOBEHAVIOR
        protected override void Awake()
        {
            base.Awake();
            // initializations
            this.Range = this.__range;
            this.BaseDamage = this.__baseDamage;
            this.Controllers = this.transform.GetComponentsInChildren<BasePattern>();
            if (this.Controllers.Length < 1)
                throw new MissingComponentException(
                    $"{this.GetType()} has no controllers <{typeof(BasePattern)}>");
            this.FlipEnabled = this.__flipEnabled;
            this.Flipped = false;
            this.UIAmmoSlot = null;
            this.InfiniteAmmo = this.__infAmmo;
            this.AmmoCount = this.__ammoCount;
            this.ShootingAnimator = this.GetComponent<Animator>();
            this.Wielder = this.__wielder;
            this.FlippedControllers = this.Flipped;
            this.Speaker = this.GetComponent<WeaponSounds>();
            this.Emitters = new List<FireBullet>();
            // populate emitters list
            foreach(BasePattern bp in this.Controllers)
            {
                var fblist = bp.GetComponentsInChildren<FireBullet>();
                this.Emitters.AddRange(fblist);
            }
            // calculate FireDelay based on rateOfFire
            this.FireDelay = 0f;
            this.FireRate = this.__fireRate;
            // set initial parameters.
            this.TimeSinceFireRequest = 0f;
            this.WaitingToFire = false;
            this.Speaker = this.GetComponent<WeaponSounds>();
        }

        protected virtual void FixedUpdate()
        {
            
            if (this.GetGunBarrel().transform.parent != null)
            {
                this.Controllers[0].Pitch = this.transform.rotation.z;
            }

            // IF the weapon is waiting to fire:
            if (this.WaitingToFire)
            {
                // UPDATE elapsed time since the last shot was fired (increate rate if we have the power up)
                if (this.Wielder.rateOfFireTimerTemp > 0)
                {
                    this.TimeSinceFireRequest += (Time.deltaTime * this.Wielder.rateOfFireTimerStrength) - this.TimeSinceFireRequest;
                }
                else
                {
                    this.TimeSinceFireRequest += Time.deltaTime - this.TimeSinceFireRequest;
                }


                // SUBTRACT elapsed time from FireDelay
                this.FireDelay -= this.TimeSinceFireRequest;
                // IF FireDelay is 0:
                if (this.FireDelay <= 0f)
                {
                    // SET WaitingToFire = FALSE
                    this.WaitingToFire = false;
                }
            }
        }
    }
}