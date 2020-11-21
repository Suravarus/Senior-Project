using System;
using Utilities;
using UnityEngine;

namespace Combat
{
    public interface IAmmo
    {
        /// <summary>
        /// Data about this item that will be used displayed
        /// to the user via UI.
        /// </summary>
        GameInfo Info { get; set; }
        /// <summary>
        /// The price of this item.
        /// </summary>
        [Obsolete("This will be moved into the GameInfo class.")]
        int Price { get; set; }
        /// <summary>
        /// The speed at which this bullet should travel.
        /// </summary>
        float Speed { get; }
        /// <summary>
        /// The added damage that this bullet causes.
        /// </summary>
        int Damage { get; set; }
        /// <summary>
        /// The weapon that from which the bullet was fired.
        /// </summary>
        Weapon Weapon { get; set; }
        /// <summary>
        /// The WeaponWielder that fired this bullet.
        /// </summary>
        WeaponWielder Shooter { get; set; }
        /// <summary>
        /// The position at which this bullet spawned.
        /// </summary>
        Vector3 StartingPosition { get; set; }
    }
}