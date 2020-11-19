using System;
using UnityEngine;

using Utilities;

namespace Combat
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Ammo : MonoBehaviour
    {
        [Tooltip("In-game name for this item. Will be stored in lower-case. Has a character-limit.")]
        public String _InGameName = "";
        [Tooltip("In-game description for this item. Will be stored as it was typed. Has a character-limit.")]
        public String _InGameDescription = "";

        [Header("General")]
        public int price;

        [Header("Movement")]
        public float speed = 5f;

        [Header("Combat")]
        [Tooltip("This damage will be added to the damage of the weapon uses this ammo.")]
        public int damage = 0;

        public Weapon weapon;

        public GameInfo GameInfo { get; set; }

        private Rigidbody2D rigidBody2D;
        private Vector3 startingPosition;
        

        // ALGORITHM:
        //    IF the rgb2d component exists:
        //        POINT to component
        //    ELSE:
        //        THROW error
        private void Awake()
        {
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

            // TODO COMBAT-TEAM[2] check that all properties have been set.
        }

        // ALGORITHM:
        // - SET startingPosition
        // - SET velocity
        // - SET GameInfo
        void Start()
        {
            this.GetComponent<SpriteRenderer>().sortingOrder = 2;
            this.startingPosition = this.transform.position;
            this.rigidBody2D.velocity = this.transform.up * speed;
            this.GetComponent<CapsuleCollider2D>().isTrigger = true;

            // SET GameInfo
            this.GameInfo = new GameInfo(this._InGameName, this._InGameDescription, this.price);
            this._InGameName = null;
            this._InGameDescription = null;
        }

        // ALGORITHM:
        // - IF this gameObject has travelled beyond the range of the weapon that fired it:
        // -   DESTROY this gameObject.
        private void FixedUpdate()
        {
            if ((this.transform.position - this.startingPosition).magnitude > this.weapon.range)
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
            if (!isWeapon && !isAmmo)
            {
                var collisionCombatant = other.GetComponent<Combatant>();
                var isOtherCombatant = collisionCombatant != null
                    && collisionCombatant.gameObject.GetInstanceID() != this.weapon.wielder.gameObject.GetInstanceID();
                var isEnemyCombatant = isOtherCombatant && collisionCombatant.tag == this.weapon.wielder.EnemyTag;

                // IF the other collider is not a Combatant, or it's an enemy Combatant
                if (collisionCombatant == null || isEnemyCombatant)
                {
                    // Report the collision to the Combatant that shot the ammo.
                    this.weapon.wielder.SendMessage(nameof(IWeaponWielder.OnAmmoCollision), other.gameObject.GetInstanceID(), SendMessageOptions.DontRequireReceiver);
                    // Destroy this gameobject.
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
