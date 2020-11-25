using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcGen
{
    public enum RoomType
    {
        /// <summary>
        /// Contains normal enemies.
        /// </summary>
        Normal,
        /// <summary>
        /// Contains a shop.
        /// </summary>
        Shop,
        /// <summary>
        /// A room for chests.
        /// </summary>
        Chest,
        /// <summary>
        /// Boss room.
        /// </summary>
        Boss,
        /// <summary>
        /// Safe-zone where the player spawns.
        /// </summary>
        Spawn
    }
}
