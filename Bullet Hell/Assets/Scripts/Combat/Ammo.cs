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
        private Vector2 startingPosition;
        

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

            //var spawnPoint = this.GetComponentInParent<Transform>();
            //var weapon = spawnPoint.GetComponentInParent<Weapon>();
            //this.ammoOwner = weapon.GetComponentInParent<Combatant>();
            Debug.Log($"Ammo owner is {this.ammoOwner.name}");
   
        }

        // ALGORITHM:
        //     SET rgb2d velocity
        void Start()
        {
            this.startingPosition = this.transform.position;
            this.rigidBody2D.velocity = this.transform.up * speed;
        }

        private void FixedUpdate()
        {
            if (Vector2.SqrMagnitude(new Vector2(this.transform.position.x, this.transform.position.y) - this.startingPosition) > this.weapon.range)
            {
                Destroy(this.gameObject);
            }
        }

        // ALGORITHM:
        //     PRINT Debug collision info
        //     GET enemy combatant from the collisionInfo
        //     IF colliding object is an enemy:
        //         
        void OnTriggerEnter2D(Collider2D collisionInfo)
        {
            Debug.Log($"{this.GetType().Name} - {this.gameObject.name} collided with {collisionInfo.name}");

            var enemy = collisionInfo.GetComponent<Combatant>();

            if (enemy != null && enemy.name != this.ammoOwner.name)
            {
                enemy.TakeDamage(this.baseDamage);
                Debug.Log($"{this.GetType().Name}: enemy health is " + enemy._health);
                Destroy(this.gameObject);
            }
        }
    }
}
