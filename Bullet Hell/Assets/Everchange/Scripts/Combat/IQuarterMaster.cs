using System;

namespace Combat
{
    public interface IQuarterMaster
    {
        /// <summary>
        /// An array of weapons that are currently in 
        /// the the QuarterMaster's arsenal (inventory).
        /// </summary>
        /// <returns></returns>
        Weapon[] GetArsenal();
        /// <summary>
        /// Returns the weapon that is in the assigned index of 
        /// the arsenal. Can be NULL.
        /// </summary>
        /// <returns></returns>
        Weapon GetAssignedWeapon();
        /// <summary>
        /// The WeaponWrapper of the Wielder
        /// </summary>
        /// <returns></returns>
        WeaponWrapper GetWeaponWrapper();
        /// <summary>
        /// The weapon in slot 'i' is assigned to the Wielder even
        /// if there is not weapon there.
        /// </summary>
        /// <param name="i"></param>
        void AssignWeaponAt(int i);
        /// <summary>
        /// Returns TRUE if a WeaponBar has been set.
        /// </summary>
        /// <returns></returns>
        bool HasWeaponBar();
        /// <summary>
        /// Pickups up a new weapon and adds it to the arsenal (inventory).
        /// </summary>
        /// <param name="weapon"></param>
        void PickupWeapon(Weapon weapon);
    }
}