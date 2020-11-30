
using System.Collections.Generic;
using UnityEngine;

using Combat;

namespace Structures
{
    public class Floor : MonoBehaviour
    {
        private List<Room> _rooms = new List<Room>();
        public List<Room> Rooms 
        { get => this._rooms; set => this._rooms = value; }

        private void Awake()
        {
            this.Rooms = new List<Room>();
        }
    }
}
