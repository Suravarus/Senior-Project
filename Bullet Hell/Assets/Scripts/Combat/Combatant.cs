using System;
using UnityEngine;

using Combat.UI;
using Loot;
using Combat.Animation;
namespace Combat
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Combatant : MonoBehaviour, ICombatant
    {
        public static int MAX_WEAPONS { get { return 3; } }

        [Header("Combat")]
        // UnityEditor properties ---------------------------------------------
        [Tooltip("The tag associated with game objects to which this character should do damage.")]
        public string _enemyTag = "";
        public int _health = 1;
        public int _maxHealth = 10;
        [Header("UI")]
        public HealthBar HealthUI;
        [Header("Animation")]
        public Animator animator;
        // ---------------------------------------------------------------------
        public String EnemyTag { 
            set { this._enemyTag = value; } 
            get { return this._enemyTag; }
        }
        public enum BodyPart
        {
            Head,
            Chest
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

            // SET Combat parameters - data will be validated by accessors.
            this.EnemyTag = this._enemyTag;
            this.MaxHealth = this._maxHealth;
            this.Health = this._health;
        }

        public virtual void Start()
        {
            // TODO [UI] All combatants will require Healthbar in future.
            // update health bar
            if (this.HealthUI != null)
                this.HealthUI.UpdateValues(this);
            else
                throw new NullReferenceException(
                    $"{nameof(this.HealthUI)} has not been set for {this.GetType()} component in {this.gameObject.name}");
            if (this.animator == null)
                this.animator = this.GetComponent<Animator>();
        }
        public virtual void Update()
        {
            return;
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
        /// Method that determines how the object takes damage.
        /// </summary>
        /// <param name="damage">Damage to be received</param>
        public virtual void TakeDamage(WeaponWielder attacker)
        {
            this.Health -= attacker.RangedDamage;
            if (this.HealthUI != null) // TODO [UI] combatants will require Healthbar in future.
                this.HealthUI.UpdateValues(this);
            if (this.Health == 0)
                this.Die();
        }

        /// <summary>
        /// This method gets called before the destruction of this object.
        /// </summary>
        public virtual void OnDie()
        {
            this.enabled = false;
            this.GetComponent<Rigidbody2D>().Sleep();
            this.GetComponent<Collider2D>().enabled = false;
            // IF has ItemDrop component
            if (this.GetComponent<ItemDrop>() != null)
                this.GetComponent<ItemDrop>().Spawn(); // CALL ItemDrop
        }

        /// <summary>
        /// Default behaviour is to destroy the gameobject.
        /// <list type="bullet"><item>Should override</item></list>
        /// </summary>
        public void Die()
        {
            Destroy(this);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (this.IsAlive())
            {
                var ammo = other.gameObject.GetComponent<Ammo>();
                bool rangedAttack = (ammo != null
                    && ammo.weapon.wielder.IsAlive()
                    && ammo.weapon.wielder.tag == this.EnemyTag);

                if (rangedAttack)
                {
                    this.TakeDamage(ammo.weapon.wielder);
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
    }
}