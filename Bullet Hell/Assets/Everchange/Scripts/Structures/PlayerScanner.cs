using System;
using UnityEngine;

namespace Structures
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerScanner : MonoBehaviour
    {
        public EventHandler PlayerEntered;
        public EventHandler PlayerLeft;
        public EventHandler PostStart;

        private bool CollidingWithPlayer { get; set; }
        /// <summary>
        /// Returns TRUE, if colliding with player.
        /// </summary>
        public bool PlayerCollision => this.CollidingWithPlayer;

        // collider
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!this.CollidingWithPlayer && collision.CompareTag("Player"))
                this.CollidingWithPlayer = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                PlayerEntered?.Invoke(this, new EventArgs());
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                PlayerLeft?.Invoke(this, new EventArgs());
        }

        // monobehaviour
        private void Awake()
        {
            this.GetComponent<Collider2D>().isTrigger = true;
        }

        private void Start()
        {

            this.PostStart?.Invoke(this, new EventArgs());
        }
    }
}
