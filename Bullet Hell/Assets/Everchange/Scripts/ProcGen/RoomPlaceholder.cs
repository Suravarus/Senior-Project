using System;
using System.Collections.Generic;
using UnityEngine;

using Structures;
using Combat.AI;
using Utilities;

namespace ProcGen
{
    public class RoomPlaceholder : MonoBehaviour
    {
        // PROPERTIES
        public RoomType roomType;
        public bool __hallLeft;
        public bool __hallTop;
        public bool __hallRight;
        public bool __hallBottom;
        private GameObject[] AI;
        public bool AllClear = false;

        // ACCESSORS
        Dictionary<CardinalDirection, RoomWall> Walls { get; set; }
        private Floor ParentFloor { get; set; }
        private Room ActiveRoom { get; set; }

        // MONOBEHAVIOUR
        private void Awake()
        {
            this.ActiveRoom = this.transform.GetChild(0).GetComponent<Room>();
            this.ActiveRoom.RoomReady += (s, e) => SetRoomGates();
            this.ParentFloor = this.GetComponentInParent<Floor>();
            GameObject[] prefabs = new GameObject[0];
            // load prefabs
            switch (roomType)
            {
                case RoomType.Boss:
                    prefabs = Resources.LoadAll<GameObject>("Prefabs/Rooms/Boss");
                    break;
                case RoomType.Chest:
                    prefabs = Resources.LoadAll<GameObject>("Prefabs/Rooms/Chest");
                    break;
                case RoomType.Normal:
                    prefabs = Resources.LoadAll<GameObject>("Prefabs/Rooms/Normal");
                    break;
                case RoomType.Shop:
                    prefabs = Resources.LoadAll<GameObject>("Prefabs/Rooms/Shop");
                    break;
                case RoomType.Spawn:
                    prefabs = Resources.LoadAll<GameObject>("Prefabs/Rooms/Spawn");
                    break;
            }

            if (prefabs.Length > 0 && (roomType == RoomType.Spawn || roomType == RoomType.Normal))
            {
                int attempts = 0;
                var index = 0;
                Room room = null;
                bool repeat = true;
                do
                {
                    index = UnityEngine.Random.Range(0, prefabs.Length);
                    room = prefabs[index].GetComponent<Room>();
                    repeat = this.ParentFloor.Rooms.Contains(room);
                    attempts += 1;
                } while (repeat 
                    && attempts < prefabs.Length);

                if (!repeat)
                {
                    this.ParentFloor.Rooms.Add(room);
                    Destroy(this.transform.GetChild(0).gameObject);
                    Instantiate(room, this.transform.position, Quaternion.identity);
                    this.ActiveRoom = room;
                } else
                {
                    this.ActiveRoom = this.transform.GetChild(0).GetComponent<Room>();
                }
            }
        }
        private void Start()
        {
            SetRoomGates();
        }
        private void SetRoomGates()
        {
            var walls = this.ActiveRoom.transform.GetComponentsInChildren<RoomWall>();
            Debug.Log($"dbc {this.ActiveRoom.name}: {walls.Length}");
            foreach (RoomWall wall in walls)
            {
                wall.HasGate = false;

                if ((this.__hallTop && wall.GetCardinalPosition() == CardinalDirection.North)
                    || (this.__hallBottom && wall.GetCardinalPosition() == CardinalDirection.South)
                    || (this.__hallLeft && wall.GetCardinalPosition() == CardinalDirection.West)
                    || (this.__hallRight && wall.GetCardinalPosition() == CardinalDirection.East))
                    wall.HasGate = true;

                Debug.Log($"db {this.ActiveRoom.name}: {wall.GetCardinalPosition()} GATE:{wall.HasGate}");
            }
        }
    }
}
