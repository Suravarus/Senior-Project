using UnityEngine;

namespace Utilities
{
    public abstract class PhysicsTool
    {
        public enum Direction
        {
            Up, Down, Left, Right
        }
        /// <summary>
        /// <para>Return the Direction Enum of the given angle as degress from the 
        /// horizontal. </para>
        /// </summary>
        /// <returns></returns>
        public static Direction DirectionFromHorizontal(Vector2 target)
        {
            float signedAngle = Vector2.SignedAngle(Vector2.right, target);
            var dir = Direction.Right;
            if (signedAngle >= 60f && signedAngle < 120f)
                dir = Direction.Up;
            else if (signedAngle >= 120f || signedAngle < -120f)
                dir = Direction.Left;
            else if (signedAngle >= -120f && signedAngle < -60f)
                dir = Direction.Down;

            return dir;
        }
    }
}
