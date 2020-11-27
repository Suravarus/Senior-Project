using ProcGen;
using UnityEngine;

namespace Structures
{
    public class Room : MonoBehaviour, IRoom
    {
        // UNITY EDITOR 
        [Tooltip("What type of room is this?")]
        public RoomType __type;

        // ACCESSORS
        public int FloorNumber { get; set; }
        private RoomType Type { get; set; }

        // METHODS
        public RoomWall[] GetWalls()
        {
            return this.transform.GetComponentsInChildren<RoomWall>();
        }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }

        public RoomType GetRoomType() => this.Type;

        // MONOBEHAVIOUR
        void Awake()
        {
            this.Type = this.__type;
        }
    }
}
