﻿using UnityEngine;

namespace Combat
{
    public interface ICombatant
    {
        string EnemyTag { get; set; }
        int Health { get; set; }
        int MaxHealth { get; set; }
        int RangedDamage { get; }
        Weapon RangedWeapon { get; set; }

        void AimRangedWeapon(Vector3 targetPosition);
        void Awake();
        void Die();
        bool Disarmed();
        void FixedUpdate();
        Transform GetBodyTransform(Combatant.BodyPart bodyPart);
        bool IsAlive();
        bool ShootRangedWeapon();
        void Start();
        void TakeDamage(Combatant attacker);

        /// <summary>
        /// This method is called by Ammo when an "Ammo" object, 
        /// that was shot by this Combatant, collides with another object.
        /// </summary>
        /// <param name="instanceID">The InstanceID of the gameobject the Ammo collided with.</param>
        void OnAmmoCollision(int instanceID);
    }
}