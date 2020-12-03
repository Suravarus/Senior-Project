
using System.Collections.Generic;
using UnityEngine;

using Combat;

namespace Structures
{
    public class Floor : MonoBehaviour, IFloor
    {
        private List<Room> _rooms = new List<Room>();
        private bool _playerPositioned = false;
        public List<Room> Rooms 
        { get => this._rooms; set => this._rooms = value; }
        private bool PlayerPositioned {
            get => this._playerPositioned; 
            set => this._playerPositioned = value; 
        }
        private SpawnPoint PlayerSpawnPoint { get; set; }
        public void OnSpawnpointReady(SpawnPoint s)
        {
            this.PlayerSpawnPoint = s;
        }

        private void Awake()
        {
            this.Rooms = new List<Room>();
        }

        void Update()
        {
            if (!this.PlayerPositioned && this.PlayerSpawnPoint != null)
            {
                this.PlayerPositioned = true;
                var p = GameObject.FindGameObjectWithTag("Player");
                p.transform.position = this.PlayerSpawnPoint.transform.position;
                Debug.Log("Floor: player positioned");
            }
        }
    }
}
