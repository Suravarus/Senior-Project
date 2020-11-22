using UnityEngine;

namespace Combat
{
    public interface ICombatant
    {
        string EnemyTag { get; set; }
        int Health { get; set; }
        int MaxHealth { get; set; }

        void Awake();
        void Start();
        void Die();
        Transform GetBodyTransform(Combatant.BodyPart bodyPart);
        bool IsAlive();
        void TakeDamage(Ammo a);
    }
}