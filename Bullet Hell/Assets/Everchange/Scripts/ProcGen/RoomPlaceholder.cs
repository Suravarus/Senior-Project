using UnityEngine;
using Combat.AI;
using System.Collections.Generic;
using Structures;
namespace ProcGen
{
    public class RoomPlaceholder : MonoBehaviour
    {
        public RoomType roomType;
        public bool __hallLeft;
        public bool __hallTop;
        public bool __hallRight;
        public bool __hallBottom;
        GameObject[] RoomList;
        public int AIcount, deathCount;
        public GameObject[] AI;
        public bool AllClear = false;

        private void Start()
        {
            switch(roomType)
            {
                case RoomType.Boss:
                    RoomList = Resources.LoadAll<GameObject>("Prefabs/Rooms/Boss");
                    break;
                case RoomType.Chest:
                    RoomList = Resources.LoadAll<GameObject>("Prefabs/Rooms/Chest");
                    break;
                case RoomType.Normal:
                    RoomList = Resources.LoadAll<GameObject>("Prefabs/Rooms/Normal");
                    break;
                case RoomType.Shop:
                    RoomList = Resources.LoadAll<GameObject>("Prefabs/Rooms/Shop");
                    break;
                case RoomType.Spawn:
                    RoomList = Resources.LoadAll<GameObject>("Prefabs/Rooms/Spawn");
                    break;
            }
            if (roomType == RoomType.Normal)
            {
                var room = RoomList[Random.Range(0, RoomList.Length)];
                //room.transform.SetParent(this.transform);
                Destroy(this.transform.GetChild(0).gameObject);
                Instantiate(room, this.transform.position , Quaternion.identity);
            }

        }
        private void Awake()
        {
            AIcount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            List<Combat.AI.AIWeaponWielder> enemy = new List<Combat.AI.AIWeaponWielder>();
            foreach (Combat.AI.AIWeaponWielder enemies in enemy)
            {
                enemies.OnDeath.Add(c =>

                {
                    deathCount += 1;
                    if (deathCount == AIcount)
                    {
                        AllClear = true;
                    }
                });

            }
        }
        private void Update()
        {
            if(AllClear == true )
            {
                this.transform.GetChild(0).GetComponent<RoomWall>().OpenGate();
            }
        }
    }
}
