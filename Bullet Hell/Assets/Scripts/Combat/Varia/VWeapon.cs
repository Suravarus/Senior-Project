
using System;
using UnityEngine;
using ND_VariaBULLET;

using UI;
using Utilities;

namespace Combat.Varia
{
    public class VWeapon : CompGameInfo, IWeapon
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
        [Header("Animation")]
        [Tooltip("Whether this weapon will have to be flipped depending on if it's facing left or right.")]
        public bool __flipEnabled = false;
        // -----------------------------------//
        // PROPERTIES
        private bool _triggerPulled = false;
        // ACCESSORS
        private float Range { get; set; }
        private float BaseDamage { get; set; }
        public BasePattern[] Controllers { get; set; }
        private bool FlipEnabled { get; set; }
        private bool Flipped { get; set; }
        public Slot UIAmmoSlot { get; set; }
        public bool InfiniteAmmo { get; set; }
        public int AmmoCount { get; set; }
        public IAmmo WeaponIAmmo { get; set; }
        public Animator ShootingAnimator { get; set; }
        public WeaponWielder Wielder { get; set; }
        private bool TriggerPulled 
        {
            get => this._triggerPulled;
            set
            {
                if (value == true)
                    this.TimeSinceTriggerPull = 0f;
                this._triggerPulled = true;
            }
        }
        private float TimeSinceTriggerPull { get; set; }
        // METHODS
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
            foreach(BasePattern bp in this.Controllers)
            {
                bp.TriggerAutoFire = true;
            }
            this.TriggerPulled = true;
        }
        private void Shoot()
        {
            
        }

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
        }

        void FixedUpdate()
        {
            //if (this.WeaponController.TriggerAutoFire == true)
            //    this.WeaponController.TriggerAutoFire = false;
        }
    }
}
