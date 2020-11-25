using UnityEngine;
namespace ProcGen
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
        IDoor[] GetDoors();
        /// <summary>
        /// Returns the gameObject associated with
        /// this room.
        /// </summary>
        /// <returns></returns>
        GameObject GetGameObject();
    }
}
