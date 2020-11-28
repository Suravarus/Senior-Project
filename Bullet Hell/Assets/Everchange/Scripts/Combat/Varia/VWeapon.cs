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
        public int __fireRate = 1;
        [Header("Animation")]
        [Header("Animation")]
        [Tooltip("Whether this weapon will have to be flipped depending on if it's facing left or right.")]
        public bool __flipEnabled = false;
        // -----------------------------------//
        // PROPERTIES
        // PROPERTIES
        private int _ammoCount;
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
            if (this.InfiniteAmmo || this.AmmoCount > 0)
            {
                if (this.Speaker != null)
                    this.Speaker.PlaySound(WeaponSounds.Sounds.Shot);
                //foreach (BasePattern bp in this.Controllers)
                //{
                //    bp.TriggerAutoFire = true;
                //}
                
                foreach (FireBullet fb in this.Emitters)
                {
                    fb.InstantiateShot();
                }
                this.TriggerPulled = true;
                this.TriggerDownFrameCount = 0;
            }
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
        }

        protected virtual void Update()
        {
            if (this.TriggerPulled)
            {
                if (this.TriggerDownFrameCount < 1)
                {
                    this.TriggerDownFrameCount += 1;
                }
                else
                {
                    this.AmmoCount -= 1;
                    StopFiring();
                }
            }
            
            if (this.GetGunBarrel().transform.parent != null)
            {
                this.Controllers[0].Pitch = this.transform.rotation.z;
            }
        }
    }
}