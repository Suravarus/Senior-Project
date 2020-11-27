using UnityEngine;
namespace Structures
{
    public interface IRoom
    {
        /// <summary>
        /// The current floor number.
        /// </summary>
        int FloorNumber { get; set; }
        /// <summary>
        /// Array of doors for this room.
        /// </summary>
        /// <returns></returns>
        RoomWall[] GetWalls();
        /// <summary>
        /// Returns the gameObject associated with
        /// this room.
        /// </summary>
        /// <returns></returns>
        GameObject GetGameObject();
    }
}
