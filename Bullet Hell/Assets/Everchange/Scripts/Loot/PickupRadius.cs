using UnityEngine;

namespace Loot
{
    [RequireComponent(typeof(CircleCollider2D))]
    class PickupRadius : MonoBehaviour
    {
        public bool inShop = false;
        private CircleCollider2D pickupCollider { get; set; }
        public void Awake()
        {
            this.pickupCollider = this.GetComponent<CircleCollider2D>();
            if (!this.pickupCollider.isTrigger)
                this.pickupCollider.isTrigger = true;
        }

        public void OnTriggerStay2D()
        {
            Debug.Log($"Press F to pickup {this.gameObject.name}");
        }
    }
}
