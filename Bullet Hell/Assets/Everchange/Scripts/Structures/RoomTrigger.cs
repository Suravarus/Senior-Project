using System;
using UnityEngine;

namespace Structures
{
    [RequireComponent(typeof(Collider2D))]
    class RoomTrigger : MonoBehaviour
    {
        public event EventHandler Triggered;
        // COLLIDER 2D
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Triggered?.Invoke(this, new EventArgs());
            }
                
            
        }

        // MONOBEHAVIOUR
        private void Awake()
        {
            this.GetComponent<Collider2D>().isTrigger = true;
        }
    }
}
