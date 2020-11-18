using UnityEngine;

namespace Loot
{
    [RequireComponent(typeof(CircleCollider2D))]
    class PickupRadius : MonoBehaviour
    {
        public bool inShop = false;
        private CircleCollider2D Collider2D { get; set; }
        public void Awake()
        {
            if (!this.Collider2D.isTrigger)
                this.Collider2D.isTrigger = true;
        }

        public void OnTriggerStay2D()
        {
            Debug.Log($"Press F to pickup {this.gameObject.name}");
        }
    }
}
