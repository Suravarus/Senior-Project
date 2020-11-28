using UI;
using Utilities;
using UnityEngine;
using Combat.Audio;

namespace Combat
{
    public interface IWeapon
    {
        /// <summary>
        /// If set, this weapon will update this UISlot with 
        /// it's current AmmoCount.
        /// </summary>
        Slot UIAmmoSlot { get; set; }
        /// <summary>
        /// IF TRUE, the weapon will shoot regardless of the 
        /// AmmoCount.
        /// </summary>
        bool InfiniteAmmo { get; set; }
        /// <summary>
        /// The amount of ammo available for this
        /// weapon to use.
        /// </summary>
        int AmmoCount { get; set; }
        /// <summary>
        /// The ammo prefab that this weapon is to fire.
        /// </summary>
        IAmmo WeaponIAmmo { get; }
        Animator ShootingAnimator { get; set; }
        /// <summary>
        /// The WeaponWielder that is currently using 
        /// this weapon.
        /// </summary>
        WeaponWielder Wielder { get; set; }
        /// <summary>
        /// Returns TRUE if the weapon has to be flipped. This is used by
        /// the weapon wrapper.
        /// </summary>
        /// <returns></returns>
        bool RequiresFlip();
        /// <summary>
        /// Flips the weapon on the X-Axis
        /// </summary>
        void Flip();
        /// <summary>
        /// Returns TRUE if this weapon is currently flipped.
        /// </summary>
        /// <returns></returns>
        bool IsFlipped();
        /// <summary>
        /// Returns true if the target vector is within weapon range.
        /// </summary>
        /// <param name="target">Worldspace point</param>
        /// <param name="aggroDistance">if set, this will be used instead of weapon range.</param>
        /// <returns></returns>
        bool InRange(Vector3 target, float aggroDistance = 0f);
        /// <summary>
        /// Shoots the weapon only if all contraints are met.
        /// </summary>
        void RequestWeaponFire();

        /// <summary>
        /// Information such as weapon name, and description.
        /// This data will be displayed to the user via UI.
        /// </summary>
        GameInfo GetGameInfo();

        /// <summary>
        /// The distance that the bullets fired from this weapon
        /// can travel before they are destroyed.
        /// </summary>
        float GetRange();

        /// <summary>
        /// The damage that this weapon does regardless of Ammo
        /// </summary>
        float GetBaseDamage();
        /// <summary>
        /// The gameObject associated with this
        /// component.
        /// </summary>
        /// <returns></returns>
        GameObject GetGameObject();
        GameObject GetGunBarrel();
        /// <summary>
        /// The WeaponSounds component associated with this object.
        /// </summary>
        /// <returns></returns>
        WeaponSounds GetSpeaker();
    }
}