
using System;
using UnityEngine;
using Combat;

namespace Combat.AI
{
    [RequireComponent(typeof(Pathfinding.Seeker))]
    [RequireComponent(typeof(Combat.AI.ShooterAI))]
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

            if (!this.Disarmed())
            {
                this.GetComponent<ShooterAI>().MinDist = this.RangedWeapon.range * .75f;
                this.GetComponent<ShooterAI>().MaxDist = this.RangedWeapon.range;
            }
            else
            {
                this.GetComponent<ShooterAI>().MinDist = 1;
                this.GetComponent<ShooterAI>().MaxDist = 1;
            }
                
        }

        // ALGORITHM:
        // - IF not in combat:
        // -   SET nearest, within-weapon-range, enemy as current target
        // - ELSE:
        // -   AGGRO on currentTarget
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            // FIXME temp ---------------------------------------
            if (this.gameObject.name == "Turret Enemy")
            {
                Debug.LogWarning($"{this.gameObject.name} - incombat:{this.InCombat()}");
                if (this.InCombat())
                {
                    Debug.LogWarning($"{this.gameObject.name} - target-name:{this.currentTarget.gameObject.name}");
                }
            }
            // --------------------------------------------------
            if (!this.InCombat())
            {
                this.Disengage();
                this.AquireNewTarget();
            } else
            {
                this.Engage(this.currentTarget);
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

            // IF enemies found
            if (enemyGameObjects.Length > 0)
            {
                // POPULATE EnemyCombatants Array
                this.EnemyCombatantsArr = new Combatant[enemyGameObjects.Length];
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
        /// Iterates through AICombatant.EnemyCombatantsArr and returns
        /// the nearest one. NULL if none found.
        /// AICombatant.ScanForEnemies() populates EnemyCombatantsArr
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
                this.ScanForEnemies();
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
        public Boolean Engage(Combatant enemy)
        {
            Boolean shotsFired = false;
            // movement stuff
            this.GetComponent<ShooterAI>().target = enemy.GetBodyTransform(Combatant.BodyPart.Head);
            
            // weapon stuff
            if (this.RangedWeapon.InRange(enemy.GetBodyTransform(Combatant.BodyPart.Head).position))
            {
                this.AimRangedWeapon(enemy.GetBodyTransform(Combatant.BodyPart.Head).position);
                //this.GetComponent<ShooterAI>().chargeAtTheTarget = !this.RangedWeapon.LineOfSight(enemy, BodyPart.Head);
                this.ShootRangedWeapon();
            }
            return shotsFired;
        }

        /// <summary>
        /// Stop moving towards target and set target to NULL.
        /// </summary>
        public void Disengage()
        {
            this.GetComponent<ShooterAI>().chargeAtTheTarget = false;
            this.GetComponent<ShooterAI>().target = null;
        }

        // ALGORITHM
        // IF owner has a living enemy AND collided object id != enemy id
        //   THEN SET chargeAtTheTarget = TRUE
        public override void OnAmmoCollision(int instanceID)
        {
            Debug.LogWarning($"OnAmmoCollision reports collision id {instanceID}");
            if (this.InCombat() && instanceID != this.currentTarget.gameObject.GetInstanceID())
            {
                this.GetComponent<ShooterAI>().chargeAtTheTarget = true;
            } else
            {
                this.GetComponent<ShooterAI>().chargeAtTheTarget = false;
            }
        }
    }

}