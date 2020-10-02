using System.Collections;
using System;
using UnityEngine;
namespace Combat
{
    public class Combatant : MonoBehaviour, ICombatant
    {
        [Header("Combat")]
        // For editor use only. DO NOT USE IN CODE
        public int _health = 1;
        public int _maxHealth = 10;
        public int _armor = 0;
        public int _baseDamage = 0;
        // For editor use only. DO NOT USE IN CODE
        public int _shield = 0;

        [Tooltip("Prefab that will serve as the ranged weapon")]
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
                        $"{nameof(Combatant)}.{nameof(MaxHealth)} must be greated than {nameof(Health)}");
                }
            }

            get
            {
                return this._maxHealth;
            }
        }

        /// <summary>
        /// If applied, this should take damage after shields are
        /// exhausted.
        /// </summary>
        public int Armor
        {
            set
            {
                this._armor = value;
                if (this._armor < 0)
                    this._armor = 0;
            }
            get { return this._armor; }
        }
        /// <summary>
        /// If applied, this should be the first thing to take damage.
        /// </summary>
        public int Shield
        {
            set
            {
                this._shield = value;
                if (this._shield < 0)
                    this._shield = 0;
            }
            get { return this._shield; }
        }

        /// <summary>
        /// Base damage that this object should inflict.
        /// <br/> Should only be applied when no weapon is equiped.
        /// </summary>
        public int BaseDamage
        {
            set
            {
                this._baseDamage = value;
            }
            get
            {
                return this._baseDamage;
            }
        }

        // GET, SET Buffs
        ArrayList Buffs { get; set; }

        // ---------- Monobehaviour code --------------

        // ALGORITHM:
        // - CHECK for Collider2D Component
        // - SET Combat parameters
        // - CHECK for Ranged Weapon Game Object
        // - SET Combat parameters
        public virtual void Awake()
        {
            // CHECK for Collider2D Component -----------------
            var collider = this.GetComponent<Collider2D>();
            if (collider != null)
            {
                // IF Collider has not been set as a trigger -> THROW ERR
                if (!collider.isTrigger)
                {
                    var err = new NotSupportedException(
                        $"{this.GetType().Name} script requires that {collider.GetType().Name}.IsTrigger = true."
                        + "Please set it via the UnityEditor.");
                    throw err;
                }
            }
            else // IF Collider component does not exist -> THROW ERR
            {
                var err = new MissingComponentException($"Missing component: {new Collider2D().GetType().Name}");
                throw err;
            }

            // CHECK for Ranged Weapon Game Object
            if (this.rangedWeaponWrapper != null)
            {
                // Check for Weapon Component on Ranged Weapon Game Object
                var wp = this.rangedWeaponWrapper.GetComponentInChildren<Weapon>();
                if (wp != null)
                {
                    Debug.Log(wp.gameObject.name);
                    this.RangedWeapon = wp;
                } else
                {
                    throw new MissingComponentException(
                        $"The {nameof(this.rangedWeaponWrapper)} property of the {this.GetType().Name}" 
                        + $" component in {this.gameObject.name} is missing the {new Weapon().GetType().Name} component");
                }
            }
            else
            {
                throw new MissingComponentException(
                    $"Please set the {nameof(this.RangedWeapon)} field for the {this.GetType().Name} " +
                    $"component of the {this.gameObject.name} GameObject");
            }

            // SET Combat parameters - data will be validated by accessors.
            this.MaxHealth = this._maxHealth;
            this.Health = this._health;
            this.Armor = this._armor;
            this.Shield = this._shield;
            this.BaseDamage = this._baseDamage;
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

        // -------- IEnemy Implementation------------------

        /// <summary>
        /// Method that determines how the object takes damage.
        /// </summary>
        /// <param name="damage">Damage to be received</param>
        public virtual void TakeDamage(int damage)
        {
            this.Health -= damage;
            if (this.Health < 1)
                Die();
        }

        /// <summary>
        /// Default behaviour is to destroy the gameobject.
        /// <list type="bullet"><item>Should override</item></list>
        /// </summary>
        public virtual void Die()
        {
            Destroy(this.gameObject);
        }

        /// <summary>
        /// Method used to respawn this object.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        /// <param name="spawnPoint">Point where the object should spawn</param>
        public virtual void Respawn(Vector2 spawnPoint)
        {
            throw new NotImplementedException();
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
    }
}