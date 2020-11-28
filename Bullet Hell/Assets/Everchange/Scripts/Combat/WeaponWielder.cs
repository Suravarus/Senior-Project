using System;
using UnityEngine;

using Combat.Animation;
using Combat.UI;

namespace Combat
{
    /// <summary>
    /// Sub-class of Combatant. This is a Combatant that uses weapons.
    /// </summary>
    [System.Serializable]
    public class WeaponWielder : Combatant, IWeaponWielder
    {
        [Header("Animation")]
        [Tooltip("If TRUE, the sprite and animator components must be on the " 
            +"WeaponWrapper and only the Idle and RunUp animations will be used.")]
        public bool __spriteOnWrapper = false;
        [Header("Weapons")]
        [Tooltip("Child object that will be used to position ranged weapons.")]
        public WeaponWrapper rangedWeaponWrapper;
        public Weapon startingWeapon;

        [Header("Powerups")]
        public Boolean rateOfFirePowerUp = false;
        public float rateOfFireTimerStrength = 2f;
        public float rateOfFireTimer = 8f;
        public float rateOfFireTimerTemp = 0f;
        public Boolean piercingPowerUp = false;
        public float piercingTimer = 10f;
        public float piercingTimerTemp = 0f;

        [Tooltip("This should only be set for the Player.")]
        public WeaponBarUI weaponBarUI = null;

        private QuarterMaster quarterMaster;
        public QuarterMaster GetQuarterMaster()
        {
            return this.quarterMaster;
        }
        public PuppetMaster Puppeteer { set; get; }

        public IWeapon RangedWeapon
        {
            get { return this.GetQuarterMaster().GetAssignedWeapon(); }
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
                        this.rangedWeaponWrapper,
                        this.weaponBarUI);
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

            if (!this.__spriteOnWrapper && this.animator != null)
                this.Puppeteer = new PuppetMaster(this.animator, this, this.__spriteOnWrapper);
            else if (this.__spriteOnWrapper)
            {
                this.animator = this.GetWeaponWrapper().GetComponent<Animator>();
                if (this.animator != null)
                    this.Puppeteer = new PuppetMaster(this.animator, this, this.__spriteOnWrapper);
            }
        }

        public override void Update()
        {
            base.Update();

            if (this.Puppeteer != null)
                this.Puppeteer.PullTheStrings();

            //RateOfFire power up
            if (rateOfFirePowerUp) rateOfFireTimerTemp = rateOfFireTimer; rateOfFirePowerUp = false;
            if (rateOfFireTimerTemp > 0) rateOfFireTimerTemp = rateOfFireTimerTemp - Time.deltaTime;

            //piercing power up
            if (piercingPowerUp) piercingTimerTemp = piercingTimer; piercingPowerUp = false;
            if (piercingTimerTemp > 0) piercingTimerTemp = piercingTimerTemp - Time.deltaTime;
        }

        void OnDestroy()
        {
            var bullets = FindObjectsOfType<Ammo>();
            foreach(var b in bullets)
            {
                // FIXME - Combat - causing null ref error on scene close
                //if (b != null
                //    && b.Shooter != null
                //    && b.Shooter.GetGameObject() != null &&
                //    b.Shooter.GetGameObject().GetInstanceID() == this.gameObject.GetInstanceID())
                //    Destroy(b.gameObject);

                try 
                {
                    if (b.Shooter.GetGameObject().GetInstanceID() == this.gameObject.GetInstanceID())
                        Destroy(b);
                }
                catch(Exception ex) { Debug.LogWarning(ex.Message); }
            }
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
            Combatant.RotateTo(targetPosition, this.rangedWeaponWrapper.transform);
        }

        /// <summary>
        /// Shoots the RangedWeapon. The weapon's rate of fire is taken into account.
        /// </summary>
        public virtual Boolean ShootWeapon()
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
            this.RangedWeapon.GetGameObject().SetActive(false);
        }

        public virtual void OnAmmoCollision(int id)
        {
            return;
        }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }
        public void SavePlayer()
        {
            SaveSystem.SavePlayer(this);
        }

        public void LoadPlayer()
        {
            PlayerData data = SaveSystem.LoadPlayer();


            Vector3 position;
            position.x = data.position[0];
            position.y = data.position[1];
            position.z = data.position[2];

           // arsenal = player.GetQuarterMaster().GetArsenal();
            //ammo[0] = arsenal[0].ammo;
            //ammo[1] = arsenal[1].ammo;
            //ammo[2] = arsenal[2].ammo;

            //floorNumber = 1;
        }
    }
}
