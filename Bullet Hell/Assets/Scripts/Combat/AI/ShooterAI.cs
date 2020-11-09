using System;
using UnityEngine;
using Pathfinding;

namespace Combat.AI
{
    [Obsolete("This component will soon be merged with AICombatant and will no longer be accessible.")]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Seeker))]
    public class ShooterAI : MonoBehaviour
    {
        
        
        public float speed = 3f;
        public float nextWaypointDistance = 1f;
        public float acceleration;

        public Transform target;
        private Vector2 DistanceFromTarget;
        /// <summary>
        /// Whether the AI should attempt to get as close as possible to it's
        /// target.
        /// </summary>
        public Boolean chargeAtTheTarget = false;
        Path path;
        int currentWaypoint = 0;
        bool reachedEndOfPath = false;

        private float _minDist = 1;

        private float _timeSinceStatic = 0f;
        public float MinDist
        {
            set { this._minDist = Math.Abs(value); }
            get
            {
                if (!this.chargeAtTheTarget)
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
                if (!this.chargeAtTheTarget)
                    return this._maxDist;
                return 1;
            }
        }


        Seeker seeker;
        Rigidbody2D rb;


        // Start is called before the first frame update
        void Start()
        {
            if (this.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
            {
                seeker = GetComponent<Seeker>();
                rb = GetComponent<Rigidbody2D>();
                //this.target = GameObject.FindGameObjectWithTag("Player").transform;
                //sets target to the player
                InvokeRepeating("UpdatePath", 0f, .5f);
            }
        }

        void UpdatePath()
        {
            if (this.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
            {
                if (seeker.IsDone() && target != null)
                    seeker.StartPath(rb.position, target.position, OnPathComplete);
            }
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
            if (this.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static 
                && this.GetComponent<AICombatant>().IsAlive())
            {
                if (path == null)
                {
                    return;
                }

                if (!this.GetComponent<AICombatant>().InCombat()
                    || currentWaypoint >= path.vectorPath.Count-1)
                {
                    reachedEndOfPath = true;
                    return;
                }
                else
                {
                    this.reachedEndOfPath = false;

                    // calculations
                    var distanceFromWaypoint = Vector2.Distance(this.path.vectorPath[this.currentWaypoint], this.transform.position);
                    var distanceFromTarget = Vector2.Distance(this.target.transform.position, this.transform.position);

                    // check if too close to target
                    if (distanceFromTarget < this.MinDist)
                    {
                        this.Flee();
                    } 
                    else if (distanceFromTarget > this.MaxDist) // check if far from player
                    {
                        this.WalkTowardsNextWaypoint();
                    } else
                    {
                        this.rb.velocity = Vector2.zero;
                    }

                    float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

                    if (distance < nextWaypointDistance)
                    {
                        currentWaypoint++;
                    }
                }
            }
        }

        private void WalkTowardsPath()
        {
            Vector2 direction = ((Vector2)this.path.vectorPath[this.currentWaypoint] - this.rb.position).normalized;
            this.rb.MovePosition(this.rb.position + direction * this.speed * Time.deltaTime);
        }

        /// <summary>
        /// Makes this object move towards the next waypoint on the path.
        /// </summary>
        private void WalkTowardsNextWaypoint()
        {
            if (this._timeSinceStatic < 0)
                this._timeSinceStatic = 0;

            var direction = (this.path.vectorPath[this.currentWaypoint + 1]
                - this.transform.position).normalized;

            this._timeSinceStatic += Time.deltaTime;

            var newSpeed = this.rb.velocity.magnitude + this.acceleration * this._timeSinceStatic;
            
            if (newSpeed < this.speed)
                this.rb.velocity = direction * newSpeed;
            else
                this.rb.velocity = direction * this.speed;
        }

        private void Flee()
        {
            if (this._timeSinceStatic > 0 || this.rb.velocity.magnitude == 0)
                this._timeSinceStatic = 0;

            var dir = ((Vector2)this.target.transform.position - (Vector2)this.transform.position).normalized;

            this.rb.AddForce(dir * -this.acceleration * 1.5f);
        }
    }
}