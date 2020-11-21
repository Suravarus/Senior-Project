using System;
using Utilities;
using UnityEngine;

namespace Combat
{
    public interface IAmmo
    {
        /// <summary>
        /// The speed at which this bullet should travel.
        /// </summary>
        float Speed { get; set; }
        /// <summary>
        /// The added damage that this bullet causes.
        /// </summary>
        float Damage { get; set; }
        /// <summary>
        /// The weapon that from which the bullet was fired.
        /// </summary>
        IWeapon Weapon { get;}
        /// <summary>
        /// The WeaponWielder that fired this bullet.
        /// </summary>
        IWeaponWielder Shooter { get; set; }
        
        /// <summary>
        /// Data about this item that will be used displayed
        /// to the user via UI.
        /// </summary>
        GameInfo GetGameInfo();
        /// <summary>
        /// The starting world space position of this transform.
        /// </summary>
        Vector3 GetStartingPosition();
    }
}