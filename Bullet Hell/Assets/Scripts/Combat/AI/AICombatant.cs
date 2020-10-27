
using System;
using UnityEngine;
using Combat;

namespace Combat.AI
{
    [RequireComponent(typeof(Pathfinding.Seeker))]
    public class AICombatant : Combatant
    {
        // UnityEditor FIELDS -----------------------------------
        [Header("Behavior Parameters")]
        [Tooltip("Should this AI only attack if provoked?.")]
        public Boolean passive = false;
        [Tooltip("Should this AI scan for enemies in Start(). (default = false)")]
        public Boolean scanAtStart = false;
        [Tooltip("If TRUE(default), this combatant will never run out of ammo.")]
        public Boolean infinitAmmo = true;
        [Tooltip("Speed at which this AI should move.")]
        public float speed = 5f;
        // ------------------------------------------------------


        private Combatant currentTarget { get; set; }
        private Boolean ScanInProgress { get; set; }
        private Combatant[] _enemyCombatantsArr;
        public Combatant[] EnemyCombatantsArr
        {
            set { this._enemyCombatantsArr = value; }
            get { return this._enemyCombatantsArr; }
        }

        public override void Start()
        {
            base.Start(); // call base class start method.
            // SET infiniteAmmo
            this.RangedWeapon.infAmmo = this.infinitAmmo;
            // IF scanAtStart = TRUE:
            if (this.scanAtStart)
                this.ScanForEnemies(); // SCAN for Enemies
        }

        // ALGORITHM:
        // - IF not in combat:
        // -   SET nearest, within-weapon-range, enemy as current target
        // - ELSE:
        // -   AGGRO on currentTarget
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!this.InCombat())
            {
                this.AquireNewTarget();
            } else
            {
                this.Aggro(this.currentTarget);
            }
            
        }

        public bool HasEnemies()
        {
            return !this.ScanInProgress && this.EnemyCombatantsArr != null && this.EnemyCombatantsArr.Length > 0;
        }

        /// <summary>
        /// Returns TRUE if this AI is targeting a combatant.
        /// </summary>
        /// <returns></returns>
        public Boolean HasTarget()
        {
            return !this.ScanInProgress
                && this.currentTarget != null;
        }

        /// <summary>
        /// Returns TRUE if this AI is targeting a Combatant that is alive. Otherwise,
        ///  it tries to obtain a new target and returns FALSE.
        ///  <para>RECURSIVE</para>
        /// </summary>
        /// <returns></returns>
        public Boolean InCombat()
        {
            return this.HasTarget() && this.currentTarget.IsAlive();
        }

        /// <summary>
        /// Scans for the Rigidbodies of gameobjects with the Combatant.EnemyTag tag.
        /// <list type="bullet">
        ///   <item>RETURNS TRUE if enemies have been found.</item>
        /// </list>
        /// </summary>
        /// <returns>TRUE if enemies have been found.</returns>
        private bool ScanForEnemies()
        {
            // UPDATE scanning status
            this.ScanInProgress = true;
            // REMOVE previous enemies
            this.EnemyCombatantsArr = null;
            // SEARCH for Enemy GameObjects
            var enemyGameObjects = GameObject.FindGameObjectsWithTag(this.EnemyTag);

            // SET Enemy Combatants
            if (enemyGameObjects.Length > 0)
            {
                this.EnemyCombatantsArr = new Combatant[enemyGameObjects.Length];
                // SET PlayerRB field
                for (int i = 0; i < enemyGameObjects.Length; i++)
                {
                    var c = enemyGameObjects[i].GetComponent<Combatant>();
                    if (c != null)
                        this.EnemyCombatantsArr[i] = c;
                    else // THROW ERR if gameObjects are missing combatant component.
                        throw new MissingComponentException(
                            $"{this.gameObject.name} tried to set {c.name} as enemy,"
                            + $" but {c.name} is missing component {base.GetType().Name}");
                    
                }
            }
            // UPDATE scanning status
            this.ScanInProgress = false;
            // RETURN TRUE if enemies found
            return this.HasEnemies();
        }

        /// <summary>
        /// Iterates through the current list of enemies and returns
        /// the nearest one. NULL if none found.
        /// <para>Will not execute if there is a scan in progress.</para>
        /// </summary>
        /// <returns></returns>
        public Combatant NearestEnemy()
        {
            Combatant nearestEnemy = null;

            if (!this.ScanInProgress && this.HasEnemies())
            {
                var nearestIndex = 0;
                var nearestDistance = Vector3.Distance(
                    this.EnemyCombatantsArr[nearestIndex].transform.position, 
                    this.transform.position);

                for (int i = 0; i < this.EnemyCombatantsArr.Length; i++)
                {
                    float distance = Vector3.Distance(this.EnemyCombatantsArr[i].transform.position
                        , this.transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestIndex = i;
                    }
                }

                nearestEnemy = this.EnemyCombatantsArr[nearestIndex];
            }

            return nearestEnemy;
        }

        /// <summary>
        /// Looks for the nearest enemy within weapon-range. Returns FALSE if no valid enemy was found.
        /// <para>Will not execute if there is a scan in progress.</para>
        /// </summary>
        /// <returns></returns>
        public Boolean AquireNewTarget()
        {
            if (!this.ScanInProgress)
            {
                this.currentTarget = null;
                var n = this.NearestEnemy();
                if (n != null && this.RangedWeapon.InRange(n.transform.position))
                    this.currentTarget = n;

                return this.HasTarget();
            }

            return false;
        }

        /// <summary>
        /// Aims and shoots at the given enemy if it is within weapon-range.
        /// Otherwise, it will chase the enemy.
        /// Returns TRUE if the weapon was fired.
        /// <para>Will not execute if there is a scan in progress.</para>
        /// </summary>
        /// <returns></returns>
        // ALGORITHM:
        //   IF Enemy is within weapon range:
        //     ATTACK Enemy
        //   ELSE
        //     CHASE Enemy.
        public Boolean Aggro(Combatant enemy)
        {
            if (this.RangedWeapon.InRange(enemy.transform.position))
            {
                this.ShootAt(enemy);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Aims and shoots at the given enemy. Returns true if the weapon was actually fired.
        /// </summary>
        /// <param name="enemy"></param>
        /// <returns></returns>
        private Boolean ShootAt(Combatant enemy)
        {
            // AIM at the player
            this.AimRangedWeapon(enemy.transform.position);
            // SHOOT at the player
            return this.ShootRangedWeapon();
        }
    }

}