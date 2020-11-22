
using System;
using UnityEngine;

using UI;
using Utilities;

namespace Combat.Varia
{
    public class VWeapon : MonoBehaviour, IWeapon
    {
        // UNITY EDITOR ----------------------//
        [Header("General")]
        [TextArea(1, 1)]
        public string __weaponName = "";
        [TextArea(1,2)]
        public string __weaponDescription = "";
        [Min(0)]
        public float __price;
        [Header("Ammo")]
        public IAmmo __weaponAmmo;
        public Transform __ammoSpawnPoint;
        public WeaponWielder __wielder;
        public int __baseDamage;
        public bool __infAmmo = false;
        [Min(0)]
        public int __ammoCount = 0;
        [Header("Combat")]
        public float __range = 3f;
        [Header("Animation")]
        [Tooltip("Whether this weapon will have to be flipped depending on if it's facing left or right.")]
        public bool __flipEnabled = false;
        // -----------------------------------//

        // ACCESSORS
        private GameInfo Info { get; set; }
        private float Range { get; set; }
        private float BaseDamage { get; set; }
        private bool FlipEnabled { get; set; }
        private bool Flipped { get; set; }
        public Slot UIAmmoSlot { get; set; }
        public bool InfiniteAmmo { get; set; }
        public int AmmoCount { get; set; }
        public IAmmo WeaponAmmo { get; set; }
        public Animator ShootingAnimator { get; set; }
        public WeaponWielder Wielder { get; set; }
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

        public GameInfo GetGameInfo() => this.Info;

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
            throw new System.NotImplementedException();
        }

        // MONOBEHAVIOR
        void Awake()
        {
            // initilizations
            this.Info = new GameInfo(
                this.__weaponName,
                this.__weaponDescription,
                this.__price);
            this.Range = this.__range;
            this.BaseDamage = this.__baseDamage;
            this.FlipEnabled = this.__flipEnabled;
            this.Flipped = false;
            this.UIAmmoSlot = null;
            this.InfiniteAmmo = this.__infAmmo;
            this.AmmoCount = this.__ammoCount;
            this.WeaponAmmo = this.__weaponAmmo;
            if (this.WeaponAmmo == null)
                throw new MissingFieldException(
                    nameof(this.WeaponAmmo));
            this.ShootingAnimator = this.GetComponent<Animator>();
            if (this.ShootingAnimator == null)
                throw new MissingFieldException(
                    nameof(this.ShootingAnimator));
            this.Wielder = this.__wielder;
        }
    }
}
