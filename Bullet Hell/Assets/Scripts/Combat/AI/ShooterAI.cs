using System;
using UnityEngine;
using Pathfinding;

namespace Combat.AI
{
    public class ShooterAI : MonoBehaviour
    {
        
        
        public float speed = 3f;
        public float nextWaypointDistance = 1f;

        public Transform target;
        private Vector2 DistanceFromTarget;
        /// <summary>
        /// Whether the AI should attempt to get as close as possible to it's
        /// target.
        /// </summary>
        public Boolean closeTheGap = false;
        Path path;
        int currentWaypoint = 0;
        bool reachedEndOfPath = false;

        private float _minDist = 1;
        public float MinDist
        {
            set { this._minDist = Math.Abs(value); }
            get
            {
                if (!this.closeTheGap)
                {
                    return this._minDist;
                }
                else
                {
                    return 0;
                }
            }
        }

        private float _maxDist = 2;
        public float MaxDist
        {
            set { this._maxDist = Math.Abs(value); }
            get
            {
                if (!this.closeTheGap)
                    return this._maxDist;
                return 1;
            }
        }


        Seeker seeker;
        Rigidbody2D rb;


        // Start is called before the first frame update
        void Start()
        {
            seeker = GetComponent<Seeker>();
            rb = GetComponent<Rigidbody2D>();
            //this.target = GameObject.FindGameObjectWithTag("Player").transform;
            //sets target to the player
            InvokeRepeating("UpdatePath", 0f, .5f);
        }

        void UpdatePath()
        {
            if (seeker.IsDone() && target != null)
                seeker.StartPath(rb.position, target.position, OnPathComplete);
        }

        void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                path = p;
                currentWaypoint = 0;
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (path == null)
                return;

            if (!this.GetComponent<AICombatant>().InCombat()
                || currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
                reachedEndOfPath = false;
            //move along path
            DistanceFromTarget = target.transform.position - transform.position;
            if (this.closeTheGap || DistanceFromTarget.magnitude > this.MaxDist)
            {//walk towards path if outside of max distance 
                Debug.LogWarning($"Walking towards {this.target.gameObject.name}");
                this.WalkTowardsPath();
            }
            else if (DistanceFromTarget.magnitude < this.MinDist)
            {//flee if too close
                this.Flee();
            }

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }

        private void WalkTowardsPath()
        {
            Vector2 direction = ((Vector2)this.path.vectorPath[this.currentWaypoint] - this.rb.position).normalized;
            this.rb.MovePosition(this.rb.position + direction.normalized * this.speed * Time.deltaTime);
        }

        private void Flee()
        {
            this.rb.MovePosition(
                this.rb.position + -this.DistanceFromTarget.normalized * this.speed * Time.deltaTime);
        }
    }
}