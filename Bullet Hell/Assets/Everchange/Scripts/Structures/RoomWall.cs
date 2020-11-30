using System;
using Utilities;
using UnityEngine;

namespace Structures
{
    /// <summary>
    /// This component is used on the outermost walls of a room.
    /// Will have a gate which can be opened or closed.
    /// It will also have a cardinal postition indicating where it is in the room. (n, s, e, w)
    /// </summary>
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
        private CardinalDirection CardinalPosition 
        { 
            get => this.__cardinalPostion; 
            set => this.__cardinalPostion = value; 
        }

        // METHODS
        /// <summary>
        /// Opens the Gate if HasGate has been set to TRUE.
        /// Returns TRUE if the gate was opened.
        /// </summary>
        /// <returns>was the gate opened?</returns>
        public bool OpenGate()
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
        public void CloseGate(bool openOnCollision = false)
        {
            this.WallGate.Close(openOnCollision);
        }
        public CardinalDirection GetCardinalPosition() => this.CardinalPosition;
        // MONOBEHAVIOUR
        private void Awake()
        {
            this.WallGate = this.transform.GetComponentInChildren<Gate>();
            if (this.WallGate == null)
                throw new MissingMemberException($"missing child object {typeof(Gate)}");
            this.CardinalPosition = this.__cardinalPostion;
            this.CardinalPosition = this.__cardinalPostion;
            this.OpenOnCollision = this.__openOnPlayerCollision;

            this.WallGate.CardinalPosition = this.CardinalPosition;
            this.WallGate.OpenOnCollision = this.OpenOnCollision;
        }
    }
}
