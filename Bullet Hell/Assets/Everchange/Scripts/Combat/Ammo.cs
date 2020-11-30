using System;
using UnityEngine;

using Utilities;

namespace Combat
{
    [RequireComponent(typeof(Collider2D))]
    public class Ammo : CompGameInfo, IAmmo
    {
        [Header("VariaBullet2D")]
        public bool isVariaPrefab = false;

        [Header("Movement")]
        public float speed = 5f;

        [Header("Combat")]
        [Tooltip("This damage will be added to the damage of the weapon uses this ammo.")]
        public int damage = 0;
        public Boolean piercingPowerUp = false;

        public Weapon weapon;

        private Rigidbody2D rigidBody2D;
        private Vector3 startingPosition;

        // ACCESSORS
        public float Speed { get => this.speed; set => this.speed = value; }
        public float Damage { get => this.damage; set => this.damage = Mathf.RoundToInt(value); }
        public IWeapon Weapon { get => this.weapon; }
        public IWeaponWielder Shooter { get; set; }


        // ALGORITHM:
        //    IF the rgb2d component exists:
        //        POINT to component
        //    ELSE:
        //        THROW error
        protected override void Awake()
        {
            base.Awake();
            // IF has rgb2d component -> SET rgb2d
            // ELSE -> throw error
            if (this.GetComponent<Rigidbody2D>() != null)
            {
                this.rigidBody2D = this.GetComponent<Rigidbody2D>();
            } else
            {
                throw new MissingComponentException(
                    $"Missing component: {new Rigidbody2D().GetType().Name}");
            }

            this.Shooter = this.weapon.wielder;
        }

        // ALGORITHM:
        // - SET startingPosition
        // - SET velocity
        // - SET GameInfo
        void Start()
        {
            if (!this.isVariaPrefab)
            {
                this.GetComponent<Collider2D>().isTrigger = true;
                this.startingPosition = this.transform.position;
                this.rigidBody2D.velocity = this.transform.up * speed;
            }
            if (this.startingPosition == null)
                this.startingPosition = this.transform.position;
        }

        // ALGORITHM:
        // - IF this gameObject has travelled beyond the range of the weapon that fired it:
        // -   DESTROY this gameObject.
        private void FixedUpdate()
        {
            if (!this.isVariaPrefab && (this.transform.position - this.startingPosition).magnitude > this.weapon.range)
            {
                Destroy(this.gameObject);
            }
        }

        // PURPOSE:
        // - Since this method is called whenever this gameObject collides with ANY
        // - Collider2D object that is set to Collider.IsTrigger = True, this code
        // ALGORITHM:
        // - GET enemy combatant from the other collider.
        // - IF the other collider is not a Combatant, or it's an enemy Combatant:
        // -   THEN: 
        // -     SEND instancedID of collision object to ammoOwner
        // -     Destroy this gameobject.
        void OnTriggerEnter2D(Collider2D other)
        {
            // GET enemy combatant from the collisionInfo
            var isWeapon = other.GetComponent<Weapon>() != null;
            var isAmmo = other.GetComponent<Ammo>() != null;
            var isScanner = other.GetComponent<Structures.PlayerScanner>();
            if (!isWeapon && !isAmmo && !isScanner)
            {
                var collisionCombatant = other.GetComponent<Combatant>();
                var isOtherCombatant = collisionCombatant != null
                    && collisionCombatant.gameObject.GetInstanceID() != this.weapon.wielder.gameObject.GetInstanceID();
                var isEnemyCombatant = isOtherCombatant && collisionCombatant.CompareTag(this.weapon.wielder.EnemyTag);

                // IF the other collider is not a Combatant, or it's an enemy Combatant
                if (collisionCombatant == null || isEnemyCombatant)
                {
                    // Report the collision to the Combatant that shot the ammo.
                    if (this.Shooter != null)
                    this.Shooter.OnAmmoCollision(other.gameObject.GetInstanceID());
                    // Destroy this gameobject.
                    if(!this.isVariaPrefab && !piercingPowerUp)    Destroy(this.gameObject);
                }
            }
        }

        public Vector3 GetStartingPosition()
        {
            return this.startingPosition;
        }
    }
}
