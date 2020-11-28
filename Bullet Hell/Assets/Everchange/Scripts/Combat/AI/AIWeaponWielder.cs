
using System;
using UnityEngine;
using Combat;

namespace Combat.AI
{
    [RequireComponent(typeof(Pathfinding.Seeker))]
    [RequireComponent(typeof(ShooterAI))]
    public class AIWeaponWielder : WeaponWielder
    {
        public enum AIType
        {
            Humanoid,
            Turret
        }
        // UnityEditor FIELDS -----------------------------------
        [Header("Behavior Parameters")]
        public AIType Type;
        [Tooltip("Should this AI only attack if provoked?.")]
        public Boolean passive = false;
        [Tooltip("If 0, then 2x weapon-range will be used.")]
        [Min(0)]
        public int __aggroRange = 0;
        [Tooltip("If TRUE(default), this combatant will never run out of ammo.")]
        public Boolean infinitAmmo = true;
        [Tooltip("Speed at which this AI should move.")]
        public float speed = 5f;
        [Header("Movement AI")]
        public int nextWaypointDistance;
        [Tooltip("Degrees of inaccuracy")]
        [Min(0)]
        public float inAccuracy = 0f;
        [Header("Physics")]
        public bool collideWithCombatants = false;
        // ------------------------------------------------------

        private Combatant CurrentTarget { get; set; }
        private Boolean ScanInProgress { get; set; }
        private int AggroRange { get; set; }
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
            this.RangedWeapon.InfiniteAmmo = this.infinitAmmo;
            if (!this.collideWithCombatants)
            {
                var Combatants = GameObject.FindObjectsOfType<Combatant>();
                foreach (Combatant c in Combatants)
                {
                    if (c.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
                    {
                        Physics2D.IgnoreCollision(
                            c.gameObject.GetComponent<Collider2D>(), 
                            this.GetComponent<Collider2D>());
                    }
                }
            }

            // SET ShooterAI properties
            this.GetComponent<ShooterAI>().speed = this.speed;
            this.GetComponent<ShooterAI>().nextWaypointDistance = this.nextWaypointDistance;
            Debug.Log($"{this.gameObject.name} disarmed: {this.Disarmed()}");
            if (!this.Disarmed())
            {
                this.GetComponent<ShooterAI>().MinDist = this.RangedWeapon.GetRange() * .5f;
                Debug.Log($"{this.gameObject.name} min: {this.GetComponent<ShooterAI>().MinDist}");
                this.GetComponent<ShooterAI>().MaxDist = this.RangedWeapon.GetRange();
            }
            else
            {
                this.GetComponent<ShooterAI>().MinDist = 1;
                this.GetComponent<ShooterAI>().MaxDist = 1;
            }
            if (this.Type == AIType.Turret) this.Puppeteer.Active = false;

            if (this.__aggroRange <= 0)
                this.AggroRange = 2 * Mathf.RoundToInt(this.RangedWeapon.GetRange());
            else
                this.AggroRange = this.__aggroRange;
        }

        // ALGORITHM:
        // - IF not in combat:
        // -   SET nearest, within-weapon-range, enemy as current target
        // - ELSE:
        // -   AGGRO on currentTarget
        public override void Update()
        {
            base.Update();
            if (!this.InCombat())
            {
                this.Disengage();
                this.AquireNewTarget();
            } else
            {
                this.Engage(this.CurrentTarget);
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
                && this.CurrentTarget != null;
        }

        /// <summary>
        /// Returns TRUE if this AI is targeting a Combatant that is alive. Otherwise,
        ///  it tries to obtain a new target and returns FALSE.
        /// </summary>
        /// <returns></returns>
        public Boolean InCombat()
        {
            return this.HasTarget() && this.CurrentTarget.IsAlive();
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
                this.CurrentTarget = null;
                this.ScanForEnemies();
                var n = this.NearestEnemy();
                if (n != null && this.RangedWeapon.InRange(n.transform.position, this.AggroRange))
                    this.CurrentTarget = n;

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
            // AIM at target
            this.AimWeapon(enemy.GetBodyTransform(Combatant.BodyPart.Chest).position, this.inAccuracy);
            // movement stuff
            this.GetComponent<ShooterAI>().target = enemy.GetBodyTransform(Combatant.BodyPart.Chest);
            
            // IF target is in range
            if (this.RangedWeapon.InRange(enemy.GetBodyTransform(Combatant.BodyPart.Chest).position))
            {
                // DONT charge at target
                this.GetComponent<ShooterAI>().chargeAtTheTarget = false;
                // SHOOT target 
                this.ShootWeapon();
            } else // ELSE
            {
                // CHARGE at target
                this.GetComponent<ShooterAI>().chargeAtTheTarget = true;
            }
            this.AimWeapon(enemy.GetBodyTransform(Combatant.BodyPart.Chest).position);
            return shotsFired;
        }

        public override bool ShootWeapon()
        {
             var shot = base.ShootWeapon();
            if (this.Type == AIType.Turret && shot) 
                this.GetComponent<Animator>().Play("Shoot",0);
            return shot;
        }

        /// <summary>
        /// Stop moving towards target and set target to NULL.
        /// </summary>
        public void Disengage()
        {
            this.GetComponent<ShooterAI>().chargeAtTheTarget = false;
            this.GetComponent<ShooterAI>().target = null;
        }

        /// <summary>
        /// This method is called by Ammo when an "Ammo" object, 
        /// that was shot by this Combatant, collides with another object.
        /// </summary>
        /// <param name="instanceID">The InstanceID of the gameobject the Ammo collided with.</param>
        public override void OnAmmoCollision(int instanceID)
        {
            base.OnAmmoCollision(instanceID);
            if (this.InCombat() && instanceID != this.CurrentTarget.gameObject.GetInstanceID())
            {
                this.GetComponent<ShooterAI>().chargeAtTheTarget = true;
            } else
            {
                this.GetComponent<ShooterAI>().chargeAtTheTarget = false;
            }
        }

        public override void TakeDamage(IAmmo a)
        {
            base.TakeDamage(a);

            // IF not fighting the aggressor
            if (this.CurrentTarget == null 
                || this.CurrentTarget.GetInstanceID() != a.Shooter.GetGameObject().GetComponent<Combatant>().GetInstanceID())
            {
                // TARGET the aggressor

                if (a.Shooter == null)
                    Debug.LogError(this.name);
                a.Shooter.GetGameObject().GetComponent<Combatant>();
                this.CurrentTarget = a.Shooter.GetGameObject().GetComponent<Combatant>();
                Debug.Log($"{this.gameObject.name} charge {this.GetComponent<ShooterAI>().chargeAtTheTarget}");
            }
        }
    }

}