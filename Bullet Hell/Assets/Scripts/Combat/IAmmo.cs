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
        int Price { get; set; }
        float Speed { get; }
        int Damage { get; set; }
        Weapon Weapon { get; set; }
        WeaponWielder Shooter { get; set; }
        
        Vector3 StartingPosition { get; set; }
    }
}