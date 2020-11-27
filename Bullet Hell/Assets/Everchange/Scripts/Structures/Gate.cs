using UnityEngine;
using ProcGen;
using Utilities;
namespace Structures
{
    [RequireComponent(typeof(BoxCollider2D))]
    class Gate : MonoBehaviour
    {
        // Unity Editor
        public bool __openOnCollision = true;
        public CardinalDirection __cardinalPosition;
        // ACCESSORS
        /// <summary>
        /// Should the gate open on player collision?
        /// </summary>
        public bool OpenOnCollision { get; set; }
        public CardinalDirection CardinalPosition { get; set; }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (this.OpenOnCollision)
            {
                var player = collision.gameObject.GetComponent<PlayerMovement>();
                if (player != null)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
        /// <summary>
        /// Opens the gate.
        /// </summary>
        public void Open() => this.gameObject.SetActive(false);
        /// <summary>
        /// Closes the gate.
        /// </summary>
        /// <param name="openOnCollision">should the gate open on player collision?</param>
        public void Close(bool openOnCollision = false)
        {
            this.OpenOnCollision = openOnCollision;
            this.gameObject.SetActive(true);
        }

        // MONOBEHAVIOUR
        void Awake()
        {
            this.OpenOnCollision = this.__openOnCollision;
            this.CardinalPosition = this.__cardinalPosition;
        }
    }
}
