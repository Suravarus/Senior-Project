#region Script Synopsis
    //The base class for all non-physics type shots that are not re-poolable.
#endregion

using UnityEngine;

namespace ND_VariaBULLET
{
    public class ShotNonPhysics : ShotBaseColorizable
    {
        private Vector2 move;
        private float distanceTraveled = 0;

        public override void Update()
        {
            movement();
            base.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        private void movement()
        {
            move.x = scaledSpeed * Time.deltaTime * Trajectory.x;
            move.y = scaledSpeed * Time.deltaTime * Trajectory.y;

            transform.position += new Vector3(move.x, move.y, 0);
            if (!checkDistance())
            {
                this.distanceTraveled = 0;
                RePoolOrDestroy();
            }
        }

        private bool checkDistance()
        {
            this.distanceTraveled += move.magnitude;
            if (distanceTraveled < maxDistance)
                return true;
            return false;
        }
    }
}