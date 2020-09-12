using UnityEngine;
namespace Combat
{
    public class Ammo : MonoBehaviour
    {
        [Header("Movement")]
        public float speed = 5f;

        [Header("Combat")]
        public int baseDamage = 1;

        public Rigidbody2D rb;

        ///// <summary>Sent when another object enters a trigger 
        /////     collider attached to this object (2D physics only).</summary>
        ///// See: https://docs.unity3d.com/ScriptReference/Collider2D.OnTriggerEnter2D.html
        void OnTriggerEnter2D(Collider2D collisionInfo)
        {
            print("Hit: " + collisionInfo.name);

            var enemy = collisionInfo.GetComponent<Combatant>();
            
            if (enemy != null)
            {
                enemy.TakeDamage(this.baseDamage);
                print("DEBUG: Asteroid health: " + enemy._health);
            }

            Destroy(this.gameObject);
        }

        private void Awake()
        {
            this.rb = this.GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            //this.tag = "laser";
            rb.velocity = transform.up * speed;
        }
    }
}
