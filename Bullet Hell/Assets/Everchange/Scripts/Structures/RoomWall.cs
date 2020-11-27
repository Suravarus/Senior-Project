using System;
using Utilities;
using UnityEngine;

namespace Structures
{
    public class RoomWall : MonoBehaviour
    {
        // Unity Editor
        [Tooltip("Whether or not the this wall's gate should be enabled.")]
        public bool __hasGate;
        public bool __openOnPlayerCollision;
        public CardinalDirection __cardinalPostion;

        // ACCESSORS
        private Gate WallGate { get; set; }
        public bool HasGate { get; set; }
        public bool OpenOnCollision
        {
            set => this.WallGate.OpenOnCollision = value;
            get => this.WallGate.OpenOnCollision;
        }
        /// <summary>
        /// Cardinal position of this door. (i.e. N, S, E, W)
        /// </summary>
        private CardinalDirection CardinalPosition { get; set; }

        // METHODS
        /// <summary>
        /// Opens the Gate if HasGate has been set to TRUE.
        /// Returns TRUE if the gate was opened.
        /// </summary>
        /// <returns>was the gate opened?</returns>
        bool OpenGate()
        {
            if (this.HasGate)
                this.WallGate.Open();
            return this.HasGate;
        }
        /// <summary>
        /// Closes the gate if HasGate has been set to TRUE.
        /// </summary>
        /// <param name="openOnCollision">should the gate open on collision?</param>
        /// <returns>was the gate closed?</returns>
        bool CloseGate(bool openOnCollision = false)
        {
            if (this.HasGate)
                this.WallGate.Close(openOnCollision);
            return this.HasGate;
        }
        // MONOBEHAVIOUR
        private void Awake()
        {
            this.WallGate = this.transform.GetComponentInChildren<Gate>();
            if (this.WallGate == null)
                throw new MissingMemberException($"missing child object {typeof(Gate)}");
            this.CardinalPosition = this.__cardinalPostion;
        }
    }
}
