
using System.Collections.Generic;
using UnityEngine;

using Utilities;

namespace Combat.Varia
{
    [RequireComponent(typeof(Collider2D))]
    public class VAmmo : CompGameInfo, IAmmo
    {
        // UNITY EDITOR
        [Range(0,10)]
        public float __speed;
        public float __damage;
        public IWeapon __weapon;
        public IWeaponWielder __shooter;
        // ACCESSORS
        private Vector3 StartingPosition { get; set; }
        public float Speed { get; set; }
        public float Damage { get; set; }
        public IWeapon Weapon { get; set; }
        public IWeaponWielder Shooter { get; set; }
        // METHODS
        public Vector3 GetStartingPosition() => this.StartingPosition;

        protected override void Awake()
        {
            base.Awake();
            // initializations
            this.StartingPosition = this.transform.position;
            this.Speed = this.__speed;
            this.Damage = this.__damage;
            this.Shooter = this.__shooter;

            // get vweapon
            var p = this.transform.parent.parent;
            this.Weapon = p.GetComponent<VWeapon>();
            if (this.Weapon == null)
                throw new MissingComponentException(
                    $"did not find component {this.Weapon.GetType()} in {p.name}");
        }

        void OnDestroy()
        {
            if (this.Shooter != null)
            {
                List<Collider2D> collisions = new List<Collider2D>();
                var collider = this.GetComponent<Collider2D>();
                collider.OverlapCollider(
                    new ContactFilter2D().NoFilter(),
                    collisions);

                int collisionId = this.Shooter.GetInstanceID();
                for (int i = 0; i < collisions.Count; i++)
                {
                    var victim = collisions[i].GetComponent<Combatant>();
                    if (victim != null)
                        collisionId = victim.GetInstanceID();
                }
                // Report the victim ID to the shooter.
                this.Shooter.OnAmmoCollision(collisionId);
            }
        }
    }
}
