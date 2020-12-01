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

        private bool _hasGate;

        // ACCESSORS
        private Gate WallGate { get; set; }
        public EventHandler OnReady;
        public bool HasGate 
        {
            set
            {
                this._hasGate = value;
                if (!this._hasGate)
                {
                    this.WallGate.Close();
                    this.WallGate.OpenOnCollision = false;
                }
            }
            get => this._hasGate;
        }
        public bool OpenOnCollision
        {
            set
            {
                this.__openOnPlayerCollision = value;
                this.WallGate.OpenOnCollision = value;
            }
            get => (this.WallGate.OpenOnCollision);
        }
        /// <summary>
        /// Cardinal position of this door. (i.e. N, S, E, W)
        /// </summary>
        private CardinalDirection CardinalPosition 
        { 
            get => this.__cardinalPostion; 
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
            this.WallGate = this.transform.GetComponentInChildren<Gate>(true);
            if (this.WallGate == null)
                throw new MissingMemberException($"missing child object {typeof(Gate)}");
            this.WallGate.OpenOnCollision = this.OpenOnCollision;
            this.WallGate.CardinalPosition = this.CardinalPosition;
            OnReady?.Invoke(this, new EventArgs());
        }
    }
}
