using UnityEngine;
using Pathfinding;

namespace Combat.AI
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Seeker))]
    public class ChaserAI : MonoBehaviour
    {

        public float speed = 200f;
        public float nextWaypointDistance = 1f;
        public bool moving = true;

        private Transform target;
        Path path;
        int currentWaypoint = 0;
        bool reachedEndOfPath = false;
        int count = 0;
        Seeker seeker;
        Rigidbody2D rb;


        // Start is called before the first frame update
        void Start()
        {
            seeker = GetComponent<Seeker>();
            rb = GetComponent<Rigidbody2D>();
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            //target = GameObject.Find("Player").GetComponent<Transform>();
            //sets target to the player
            InvokeRepeating("UpdatePath", 0f, .5f);
        }

        void UpdatePath()
        {
            if (moving)
            {
                if (seeker.IsDone() && target != null)
                {
                    seeker.StartPath(rb.position, target.position, OnPathComplete);
                }
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
            if (moving)
            {
                if (path == null)
                    return;

                if (this.target != null && !this.target.GetComponent<Combat.Combatant>().IsAlive()
                    || currentWaypoint >= path.vectorPath.Count)
                {
                    reachedEndOfPath = true;
                    return;
                }
                else
                    reachedEndOfPath = false;
                //move along path
                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                rb.MovePosition(rb.position + direction.normalized * speed * Time.deltaTime);


                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

                if (distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }
            }
        }
    }

}