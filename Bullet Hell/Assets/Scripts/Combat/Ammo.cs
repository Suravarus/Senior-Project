using System;
using UnityEngine;
namespace Combat
{
    public class Ammo : MonoBehaviour
    {
        [Header("Movement")]
        public float speed = 5f;

        [Header("Combat")]
        public int baseDamage = 1;

        public Combatant ammoOwner;
        public Weapon weapon;

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
            Debug.Log($"Ammo owner is {this.ammoOwner.name}");
        }

        // ALGORITHM:
        // - SET startingPosition
        // - SET velocity
        void Start()
        {
            this.startingPosition = this.transform.position;
            this.rigidBody2D.velocity = this.transform.up * speed;
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

        // FIXME COLLISION-TEAM[1] This should be on the Combatant script.
        // PURPOSE:
        // - Since this method is called whenever this gameObject collides with ANY
        // - Collider2D object that is set to Collider.IsTrigger = True, this code
        // - is being used to apply damage to any enemy this object collides with.
        // ALGORITHM:
        // - PRINT Debug collision info
        // - GET enemy combatant from the collisionInfo
        // - IF collided with enemy<Combatabt> that does not own this ammo:
        //     DO damage to enemy
        //     DESTROY this gameObject
        // - ELSE:
        // -   DESTROY this gameObject
        void OnTriggerEnter2D(Collider2D collisionInfo)
        {
            // PRINT Debug collision info
            Debug.Log($"{this.GetType().Name} - {this.gameObject.name} collided with {collisionInfo.name}");
            // GET enemy combatant from the collisionInfo
            var enemy = collisionInfo.GetComponent<Combatant>();
            // IF collided with enemy<Combatant> that does not own this ammo: 
            if (enemy != null)
            {
                if(enemy.name != this.ammoOwner.name)
                {
                    // DO damage to enemy
                    enemy.TakeDamage(this.baseDamage);
                    Debug.Log($"{this.GetType().Name}: enemy health is " + enemy._health);
                    Destroy(this.gameObject);
                }
            } else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
