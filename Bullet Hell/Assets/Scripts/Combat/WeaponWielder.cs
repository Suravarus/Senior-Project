﻿using System;
using UnityEngine;

using Combat.Animation;

namespace Combat
{
    public class WeaponWielder : Combatant, IWeaponWielder
    {

        [Header("Weapons")]
        [Tooltip("Child object that will be used to position ranged weapons.")]
        public WeaponWrapper rangedWeaponWrapper;
        public Weapon startingWeapon;

        private Vector2 OffsetVector { get; set; }
        private Boolean Flipped = false;

        private QuarterMaster quarterMaster;
        public QuarterMaster GetQuarterMaster()
        {
            return this.quarterMaster;
        }
        public PuppetMaster Puppeteer { set; get; }

        public Weapon RangedWeapon
        {
            get { return this.GetQuarterMaster().FirstWeapon(); }
        }

        public int RangedDamage
        {
            get
            {
                var total = this.RangedWeapon.baseDamage;
                total += this.RangedWeapon.WeaponAmmo.GetComponent<Ammo>().damage;
                return total;
            }
        }
        public override void Start()
        {
            base.Start();

            // CHECK for WeaponWrapper
            if (this.rangedWeaponWrapper != null)
            {
                // SET RangedWeapon if it exists
                if (this.startingWeapon != null)
                {
                    this.quarterMaster = new QuarterMaster(this, 
                        new Weapon[] { this.startingWeapon },
                        this.rangedWeaponWrapper);
                }
                else
                {
                    throw new NullReferenceException($"{nameof(this.startingWeapon)} is null.");
                }
            }
            else // THROW ERR for missing weaponWrapper.
            {
                throw new MissingFieldException(this.GetType().Name, nameof(this.rangedWeaponWrapper));
            }

            if (this.animator != null)
                this.Puppeteer = new PuppetMaster(this.animator, this);
        }

        public override void Update()
        {
            base.Update();

            if (this.Puppeteer != null)
                this.Puppeteer.PullTheStrings();
            
        }

        public WeaponWrapper GetWeaponWrapper()
        {
            return this.GetQuarterMaster().GetWeaponWrapper();
        }

        /// <summary>
        /// Aims the ranged weapon at the specified target position.
        /// </summary>
        /// <param name="targetPosition">The point in world-space at which the target is at.</param>
        public void AimWeapon(Vector3 targetPosition)
        {
            // TODO COMBAT-TEAM[1] - Will there ever be a STATE in which the Player is Disarmed?
            Combatant.RotateTo(targetPosition, this.rangedWeaponWrapper.transform);
        }

        /// <summary>
        /// Shoots the RangedWeapon. The weapon's rate of fire is taken into account.
        /// </summary>
        public Boolean ShootWeapon()
        {
            if (!this.Disarmed())
            {
                this.RangedWeapon.RequestWeaponFire();
                return true;
            }
            return false;
        }

        public bool Disarmed()
        {
            return this.RangedWeapon == null;
        }

        public override void OnDie()
        {
            base.OnDie();
            this.RangedWeapon.gameObject.SetActive(false);
        }

        public virtual void OnAmmoCollision(int id)
        {
            return;
        }
        
    }
}