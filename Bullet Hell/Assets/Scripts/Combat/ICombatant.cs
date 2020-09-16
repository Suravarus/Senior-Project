using UnityEngine;
namespace Combat
{
    /// <summary> Interface with properties and methods
    /// required of all Objects that will engage in combat.</summary>
    public interface ICombatant
    {
        /// <summary>Handle taking damage.</summary>
        void TakeDamage(int damage);

        /// <summary>Any retaliation that happens during TakeDamage() method.</summary>
        void Retaliate(ref Combatant enemy);

        void Attack(ref Combatant enemy);

        /// <summary>Handle death. Should destroy <c>GameObject</c>.</summary>
        void Die();

        void Respawn(Vector2 spawnPoint);
    }

}
