using System;
using System.Collections.Generic;
using UnityEngine;
using UI;
using ProcGen;
using Combat;
using Combat.AI;

namespace Structures
{
    public class Room : MonoBehaviour
    {
        // UNITY EDITOR 
        [Tooltip("What type of room is this?")]
        public RoomType __type;

        // PROPERTIES
        private List<RoomWall> _walls = new List<RoomWall>();

        // ACCESSORS
        public int FloorNumber { get; set; }
        private RoomType Type { get; set; }
        private List<RoomWall> Walls 
        {
            set => this._walls = value;
            get => this._walls;
        }
        private List<RoomTrigger> RoomTriggers { get; set; }
        public PlayerScanner PlayerDetection { get; set; }
        private MOBParentObject MOBS { get; set; }
        public int AICount { get; set; }
        public int DeathCount { get; set; }
        public EventHandler RoomReady;

        // METHODS
        public RoomWall[] GetWalls()
        {
            if (this.Walls != null)
                return this.Walls.ToArray();
            else
                return null;
        }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }

        public RoomType GetRoomType() => this.Type;
        private void BeginCombat()
        {
            // seal room
            foreach (RoomWall w in this.Walls)
            {
                w.OpenOnCollision = false;
                w.CloseGate();
            }
            if (this.MOBS != null)
            {
                // reveal mobs
                this.MOBS.gameObject.SetActive(true);
                // add listener to mobs
                this.AICount = 0;
                this.DeathCount = 0;
                foreach (AIWeaponWielder ai in this.MOBS.GetComponentsInChildren<AIWeaponWielder>(true))
                {
                    this.AICount += 1;
                    ai.OnDeath.Add(c => OnAIDeath(c));
                }
            } else
            {
                this.OpenGates();
            }
            // deactivate all triggers
            foreach (RoomTrigger rt in this.RoomTriggers)
                rt.gameObject.SetActive(false);
        }

        private void OnAIDeath(Combatant c)
        {
            var ai = (Combatant)c;
            // update death-count
            this.DeathCount += 1;
            Debug.Log($"rm ai {ai.name} is dead [{this.DeathCount}|{this.AICount}]");
            // IF all AI are dead
            if (this.DeathCount == this.AICount)
            {
                Debug.Log($"rm: open gate");
                if(UnityEngine.Random.Range(1,4) == 1)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Loot.Looter>().KeyCount += 1;
                    Debug.Log("rm key dropped");
                }
                // open the gates
                this.OpenGates();
            }
        }

        private void OpenGates()
        {
            foreach (RoomWall wall in this.Walls)
                wall.OpenGate();
        }

        // MONOBEHAVIOUR
        void Awake()
        {
            this.Type = this.__type;
            this.PlayerDetection = this.transform.GetComponentInChildren<PlayerScanner>();
            // GET THE ROOMS WALLS
            var w = this.GetComponentsInChildren<RoomWall>(true);
            this.Walls.AddRange(w);
            // GET MOB parent object
            this.MOBS = this.transform.GetComponentInChildren<MOBParentObject>(true);
            //if (this.MOBS == null)
            //    throw new NullReferenceException($"{typeof(MOBParentObject)}");
            // GET ROOM TRIGGERS
            this.RoomTriggers = new List<RoomTrigger>();
            this.GetComponentsInChildren(this.RoomTriggers);
            if (this.PlayerDetection != null)
                this.PlayerDetection.PostStart += SetTriggers;
            else
                SetTriggers(this.PlayerDetection);
        }

        private void Start()
        {
            RoomReady?.Invoke(this, new EventArgs());
        }

        private void SetTriggers(object s = null, EventArgs ar = null)
        {
            if (s != null)
            {
                var playerScanner = (PlayerScanner)s;
                // IF room has triggers AND player is not in the room
                if (this.RoomTriggers != null && !playerScanner.PlayerCollision)
                {
                    // ADD TRIGGER LISTENERS
                    foreach (RoomTrigger t in this.RoomTriggers)
                    {
                        // TRIGGER runs this code when it is triggered by the player
                        t.Triggered += (object sender, EventArgs e) => BeginCombat();
                    }
                }
                else if (this.RoomTriggers != null)
                {
                    // Deactivate triggers
                    foreach (RoomTrigger t in this.RoomTriggers)
                    {
                        t.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
