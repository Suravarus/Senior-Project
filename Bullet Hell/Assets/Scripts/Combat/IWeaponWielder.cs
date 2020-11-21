using UnityEngine;

using Combat.Animation;
namespace Combat
{
    public interface IWeaponWielder
    {
        /// <summary>
        /// The Weapon that is currently equipped, if any.
        /// </summary>
        Weapon RangedWeapon { get; }
        /// <summary>
        /// The PuppetMaster that handles combat animations for this
        /// Wielder.
        /// </summary>
        PuppetMaster Puppeteer { set; get; }
        /// <summary>
        /// The QuarterMaster assigned to this Wielder.
        /// </summary>
        /// <returns></returns>
        QuarterMaster GetQuarterMaster();
        /// <summary>
        /// Component in the transform that is used
        /// to position the weapons used by this Wielder.
        /// </summary>
        /// <returns></returns>
        WeaponWrapper GetWeaponWrapper();
        int GetInstanceID();
        /// <summary>
        /// Immediately kills this Wielder.
        /// </summary>
        void Die();
        /// <summary>
        /// Returns true if there is no weapon equipped.
        /// </summary>
        /// <returns></returns>
        bool Disarmed();
        /// <summary>
        /// Fires the equipped weapon.
        /// </summary>
        /// <returns></returns>
        bool ShootWeapon();
        /// <summary>
        /// Rotates the WeaponWrapper in such a way
        /// that the weapon is aiming towards the
        /// given target vector in WorldSpace
        /// </summary>
        /// <param name="target">Point in WorldSpace</param>
        void AimWeapon(Vector3 target);
        /// <summary>
        /// Method that should get called when a bullet fired from this
        /// Wielder's weapon collides with something.
        /// </summary>
        /// <param name="id"></param>
        void OnAmmoCollision(int id);
        /// <summary>
        /// The gameobject associated with this object.
        /// </summary>
        /// <returns>The gameobject associated with this object.</returns>
        GameObject GetGameObject();
    }
}