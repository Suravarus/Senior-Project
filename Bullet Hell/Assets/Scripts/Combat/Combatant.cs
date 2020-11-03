using System;
using UnityEngine;

using Combat.UI;
namespace Combat
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Combatant : MonoBehaviour, ICombatant
    {
        [Header("Combat")]
        // UnityEditor properties ---------------------------------------------
        [Tooltip("The tag associated with game objects to which this character should do damage.")]
        public string _enemyTag = "";
        public int _health = 1;
        public int _maxHealth = 10;
        [Header("AmmoPouch")]
        [Obsolete("These will be removed in the future. Ammo values will be taken from Inventory.")]
        public int smallAmmo = 0;
        public int mediumAmmo = 0;
        public int largeAmmo = 0;
        [Header("UI")]
        public HealthBar HealthUI;
        // ---------------------------------------------------------------------

        public enum BodyPart
        {
            Head,
            Chest
        }

        /// <summary>
        /// When passing in a value, this accessor makes sure that the Tag has
        /// been defined in the UnityEditor.
        /// </summary>
        /// <exception cref="MissingFieldException">Cannot set to empty or null string.</exception>
        /// <exception cref="Exception">The passed in value is not a valid tag that has been defined in Unity.</exception>
        public string EnemyTag
        {
            set
            {
                // CHECK that the tag is defined
                bool valid = false;
                foreach (String t in UnityEditorInternal.InternalEditorUtility.tags)
                {
                    if (value.Equals(t))
                        valid = true;
                }
                if (!valid)
                {
                    // THROW err if value is empty
                    if (String.IsNullOrEmpty(value))
                        throw new MissingFieldException(this.GetType().Name, nameof(this.EnemyTag));

                    // THROW err if not a valid tag.
                    throw new Exception($"Value \'{value}\' is not a valid tag.");
                }

            }

            get { return this._enemyTag; }
        }

        [Tooltip("Child object that will be used to position ranged weapons.")]
        public GameObject rangedWeaponWrapper;

        private Weapon _rangedWeapon;
        public Weapon RangedWeapon
        {
            set { this._rangedWeapon = value; }
            get { return this._rangedWeapon; }
        }

        /// <summary>
        /// Current player health.
        /// <list type="bullet">
        ///     <item>Will throw Exception if greater than MaxHealth</item>
        /// </list>
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int Health
        {
            set
            {
                this._health = value;
                if (this._health > this.MaxHealth)
                    throw new ArgumentOutOfRangeException(nameof(Health)
                        , this._health
                        , $"Cannot be greater than {nameof(MaxHealth)}: {this.MaxHealth}");
                if (this._health < 0)
                    this._health = 0;
            }
            get { return this._health; }
        }

        public int MaxHealth
        {
            set
            {
                this._maxHealth = value;
                if (this._maxHealth < this.Health)
                {
                    throw new Exception(
                        $"{nameof(Combatant)}.{nameof(MaxHealth)} must be greater than {nameof(Health)}");
                }
            }

            get
            {
                return this._maxHealth;
            }
        }

        private int _magicArmor = 0;
        /// <summary>
        /// Armor that is applied to the player via buffs.
        /// </summary>
        private int MagicArmor
        {
            set
            {
                this._magicArmor = value > 0 ? value : 0;
            }
            get { return this._magicArmor; }
        }

        private CapsuleCollider2D _capsuleCollider2D;
        private CapsuleCollider2D CapsuleCollider2D
        {
            set
            {
                if (!value.isTrigger)
                    this._capsuleCollider2D = value;
                else
                    throw new Exception($"The {new CapsuleCollider2D().GetType().Name}"
                        + $" for the {this.gameObject.name} gameobject must be set to FALSE.");
            }
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

        // ---------- Monobehaviour code --------------

        // ALGORITHM:
        // - CHECK for Collider2D Component
        // - SET Combat parameters
        // - CHECK for RangedWeaponWrapper
        // - SET Combat parameters
        public virtual void Awake()
        {
            // CHECK for Collider2D Component -----------------
            var collider = this.GetComponent<CapsuleCollider2D>();
            if (collider != null)
            {
                this.CapsuleCollider2D = collider;
            }
            else // IF Collider component does not exist -> THROW ERR
            {
                var err = new MissingComponentException(
                    $"Missing component: {new CapsuleCollider2D().GetType().Name}");
                throw err;
            }

            // CHECK for WeaponWrapper
            if (this.rangedWeaponWrapper != null)
            {
                // SET RangedWeapon if it exists
                var wp = this.rangedWeaponWrapper.GetComponentInChildren<Weapon>();
                if (wp != null)
                {
                    this.RangedWeapon = wp;
                }
            }
            else // THROW ERR for missing weaponWrapper.
            {
                throw new MissingFieldException(this.GetType().Name, nameof(this.rangedWeaponWrapper));
            }

            // SET Combat parameters - data will be validated by accessors.
            this.EnemyTag = this._enemyTag;
            this.MaxHealth = this._maxHealth;
            this.Health = this._health;
        }

        public virtual void Start()
        {
            // FIXME All combatants will require Healthbar in future.
            // update health bar
            if (this.HealthUI != null)
                this.HealthUI.UpdateValues(this);
        }

        public virtual void FixedUpdate()
        {
            if (!this.IsAlive())
            {
                this.Die();
            }
        }


        /// <summary>
        /// Aims the ranged weapon at the specified target position.
        /// </summary>
        /// <param name="targetPosition">The point in world-space at which the target is at.</param>
        public void AimRangedWeapon(Vector3 targetPosition)
        {
            // TODO COMBAT-TEAM[1] - Will there ever be a STATE in which the Player is Disarmed?
            Combatant.RotateTo(targetPosition, this.rangedWeaponWrapper.transform);
        }

        /// <summary>
        /// Shoots the RangedWeapon. The weapon's rate of fire is taken into account.
        /// </summary>
        public Boolean ShootRangedWeapon()
        {
            if (!this.Disarmed())
            {
                this.RangedWeapon.RequestWeaponFire();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns TRUE if the player is still alive.
        /// </summary>
        /// <returns></returns>
        public Boolean IsAlive()
        {
            return this.Health > 0;
        }

        /// <summary>
        /// Returns TRUE if this Combatant has no weapon equipped.
        /// </summary>
        /// <returns></returns>
        public Boolean Disarmed()
        {
            return this.RangedWeapon == null;
        }

        /// <summary>
        /// Method that determines how the object takes damage.
        /// </summary>
        /// <param name="damage">Damage to be received</param>
        public void TakeDamage(int damage)
        {
            this.Health -= damage;
            if (this.HealthUI != null) // FIXME All combatants will require Healthbar in future.
                this.HealthUI.UpdateValues(this);
        }

        /// <summary>
        /// Default behaviour is to destroy the gameobject.
        /// <list type="bullet"><item>Should override</item></list>
        /// </summary>
        public virtual void Die()
        {
            // create new death object
            var deathSprite = Resources.Load<Sprite>("Sprites/Skull");
            this.GetComponent<SpriteRenderer>().sprite = deathSprite;
            this.RangedWeapon.gameObject.SetActive(false);
            this.transform.localScale = new Vector3(5, 5, 1);
            this.GetComponent<Rigidbody2D>().Sleep();
            this.GetComponent<Collider2D>().enabled = false;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (this.IsAlive())
            {
                var ammo = other.gameObject.GetComponent<Ammo>();
                bool rangedAttack = (ammo != null
                    && ammo.ammoOwner.IsAlive()
                    && ammo.ammoOwner.tag == this.EnemyTag);

                if (rangedAttack)
                {
                    this.TakeDamage(ammo.ammoOwner.RangedDamage);
                }
            }
        }

        /// <summary>
        /// Rotates the given <c>Transform</c> object around the Z-axis and 
        /// towards the target vector vector.
        /// </summary>
        /// <author>Umair - Combat Team</author>
        /// <param name="targetVector">Vector that points to the desired target</param>
        /// <param name="transform">The Transform of the object that is to be rotated</param>
        public static void RotateTo(Vector3 targetVector, Transform transform)
        {
            // Update weapon postion based on player input by pointing towards mouse rotating around player
            Vector3 difference = targetVector - transform.position;
            difference.Normalize();
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        }

        /// <summary>
        /// Returns the transform associated with the given BodyPart
        /// position of this gameObject.
        /// </summary>
        /// <returns></returns>
        public Transform GetBodyTransform(BodyPart bodyPart)
        {
            var trans = this.transform.Find(bodyPart.ToString());
            if (trans != null)
            {
                return trans;
            }
            else
            {
                return this.transform;
            }
        }

        public virtual void OnAmmoCollision(int instanceID)
        {
            return;
        }
    }
}