#region Script Synopsis
    //Base class for all shot scripts attached to bullet/laser prefabs.
    //Manages many common shot properties & actions.
    //Description of events and methods can be found at https://neondagger.com/variabullet2d-scripting-guide/#shot-methods-events
#endregion

using UnityEngine;
using System;

using Combat;
using Utilities;
using Loot;

namespace ND_VariaBULLET
{
    public class ShotBase : MonoBehaviour, IDamager, IAmmo
    {
        public bool PoolBank { get; set; }

        [HideInInspector]
        public float ShotSpeed;

        [Tooltip("Ignores any speed scaling that has been set in GlobalShotManager.")]
        public bool IgnoreGlobalSpeedScale;
        protected float scale;

        [HideInInspector]
        public ParentType ParentToEmitter;

        [Tooltip("Sets damage amount produced when this shot collides with an object that has a ShotCollisionDamage script attached.")]
        public float DamagePerHit = 1;
        public float DMG { get { return DamagePerHit; } }

        [Range(5, 100)]
        [Tooltip("Maximum distance the bullet can travel before being destroyed.")]
        public int maxDistance = 15;

        //This is the distance the bullet has traveled so far.
        protected float distanceTraveled = 0;


        [Tooltip("Sets this shot rotation intitially to that of its emitter.")]
        public bool InheritStartRotation = true;

        [HideInInspector]
        public float scaledSpeed;

        [HideInInspector]
        public Vector2 Trajectory;

        [HideInInspector]
        public Transform Emitter;

        [HideInInspector]
        public float ExitPoint;

        [HideInInspector]
        public FireBase FiringScript;

        [HideInInspector]
        public string sortLayer;

        [HideInInspector]
        public int sortOrder;

        private bool emitterDestroyedFlag;
        private Timer eventCounter = new Timer(0);

        protected SpriteRenderer rend;
        private bool poolOrDestroyTriggered;

        public float Speed { get => this.ShotSpeed; set => this.ShotSpeed = value; }
        public float Damage { get => this.DamagePerHit; set => this.DamagePerHit = value; }
        public IWeapon Weapon { get; set; }
        public IWeaponWielder Shooter { get; set; }
        private Vector3 StartingPosition { get; set; }

        private Vector2 prev = Vector2.zero;
        public virtual void InitialSet()
        {
            eventCounter.Reset();
            poolOrDestroyTriggered = false;

            transform.parent = Emitter;
            transform.gameObject.layer = Emitter.gameObject.layer;
            transform.localPosition = new Vector2(ExitPoint, 0);
                     
            if (InheritStartRotation) //fix for case where parent sprite is flipped
            {
                if (Emitter.transform.lossyScale.x > 0)
                    transform.rotation = Emitter.rotation;
                else
                    transform.rotation = Quaternion.AngleAxis(
                        Mathf.Abs(Emitter.rotation.eulerAngles.z) - 180, Vector3.forward);
            }

            if (ParentToEmitter == ParentType.whileShotHeld)
                FiringScript.OnStoppedFiring.AddListener(UnParent);
            else if (ParentToEmitter == ParentType.never)
                transform.parent = null;

            rend = GetComponent<SpriteRenderer>();
            setSprite(rend);
            this.Weapon = ShotBase.GetWeaponComponent(this.Emitter.transform);
            if (this.Weapon == null)
                throw new NullReferenceException($"Unable to find weapon component.");
            this.Shooter = this.Weapon.Wielder;
            this.StartingPosition = this.transform.position;
            this.maxDistance = Mathf.RoundToInt(this.Weapon.GetRange());
        }

        public virtual void Start()
        {
            //NOT IMPLEMENTED
            //Use InitialSet as default Start/Constructor unless external dependency requires Start()
        }

        public virtual void Update()
        {
            OnOutBounds();
            scale = (IgnoreGlobalSpeedScale) ? 1 : GlobalShotManager.Instance.SpeedScale;
            scaledSpeed = ShotSpeed * scale;
        }

        public virtual void FixedUpdate()
        {
            //This code is meant to prevent bullets from skipping past colliders they're meant to interact with.
            //if (prev != Vector2.zero)
            //{
            //    RaycastHit2D[] results = new RaycastHit2D[1];
            //    int mask1 = 1 << LayerMask.NameToLayer("Enemy");
            //    int mask2 = 1 << LayerMask.NameToLayer("Obstacles");
            //    int combinedMask = mask1 | mask2;
            //    if (0 < Physics2D.LinecastNonAlloc(prev, transform.position, results, combinedMask))
            //    {
            //        Debug.Log("Repool or destroy " + results[0].collider);
            //        RePoolOrDestroy();
            //    }
            //}
            //prev = transform.position;
        }

        protected virtual void setSprite(SpriteRenderer sr)
        {
            FireBullet fb = FiringScript as FireBullet;

            if (fb.SpriteOverride != null)
                sr.sprite = fb.SpriteOverride;

            sr.color = FiringScript.SpriteColor;
            sr.sortingLayerName = sortLayer;
            sr.sortingOrder = sortOrder;
        }

        public void OnEmitterDestroyedDo(Action<ShotBase> action)
        {
            if (Emitter == null)
                action(this);
        }

        public void OnEmitterDestroyedDoOnce(Action<ShotBase> action)
        {
            if (Emitter == null && emitterDestroyedFlag == false)
            {
                action(this);
                emitterDestroyedFlag = true;
            }
        }

        public void OnEventTimerDo(Action<ShotBase> action, int timeLimit)
        {
            if (!eventCounter.Flag)
            {
                eventCounter.Run(timeLimit);
                return;
            }

            action(this);
        }
        
        public void OnEventTimerDoOnce(Action<ShotBase> action, int timeLimit)
        {
            if (eventCounter.Flag)
                return;

            eventCounter.Run(timeLimit);

            if (eventCounter.Flag)
                action(this);
        }

        public void OnEventTimerDoRepeat(Action<ShotBase> action, int timeLimit)
        {
            if (!eventCounter.Flag)
            { eventCounter.Run(timeLimit); return; }
            else
                eventCounter.Reset();

            action(this);
        }

        protected virtual void UnParent()
        {
            FiringScript.OnStoppedFiring.RemoveListener(UnParent);
            transform.parent = null;
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            // FIXME - added try catch as a temporary.
            // This code was causing errors in boss script.
            try
            {
                // GET enemy combatant from the collisionInfo
                var isWeapon = collision.gameObject.GetComponent<Weapon>() != null;
                var isLoot = collision.gameObject.GetComponent<PickupRadius>() != null;
                var isAmmo = collision.gameObject.GetComponent<IAmmo>() != null;
                var isScanner = collision.gameObject.GetComponent<Structures.PlayerScanner>();
                if (!isWeapon && !isAmmo && !isScanner && !isLoot)
                {
                    var collisionCombatant = collision.gameObject.GetComponent<Combatant>();
                    var isOtherCombatant = collisionCombatant != null
                        && collisionCombatant.gameObject.GetInstanceID() != this.Weapon.Wielder.gameObject.GetInstanceID();
                    var isEnemyCombatant = isOtherCombatant && collisionCombatant.CompareTag(this.Weapon.Wielder.EnemyTag);

                    // IF the other collider is not a Combatant, or it's an enemy Combatant
                    if (collisionCombatant == null || isEnemyCombatant)
                    {
                        // Report the collision to the Combatant that shot the ammo.
                        if (this.Shooter != null)
                            this.Shooter.OnAmmoCollision(collision.gameObject.GetInstanceID());
                        // Destroy this gameobject.
                        //if (!this.isVariaPrefab && !piercingPowerUp)
                        //    Destroy(this.gameObject);
                        RePoolOrDestroy();
                    }
                }
            } catch(Exception ex)
            {
                RePoolOrDestroy();
            }
        }

        protected virtual void OnOutBounds()
        {
            if (CalcObject.IsOutBounds(transform))
                RePoolOrDestroy();
        }

        protected virtual void RePoolOrDestroy()
        {
            distanceTraveled = 0;
            if (poolOrDestroyTriggered)
                return;

            IPooler poolingScript;

            if (PoolBank && this is IRePoolable && !(GlobalShotBank.Instance.PoolCount > GlobalShotBank.Instance.PoolMaxSize))
            {
                poolingScript = GlobalShotBank.Instance;
                RePool(poolingScript);

                return;
            }

            if (FiringScript != null && FiringScript is IPooler)
            {
                poolingScript = FiringScript as IPooler;

                if (poolingScript.PoolingEnabled)
                {
                    if (this is IRePoolable)
                        RePool(poolingScript);
                    else
                        Kill(gameObject);
                }
                else
                    Kill(gameObject);
            }
            else
                Kill(gameObject);
        }
     
        public virtual void RePool(IPooler poolingScript) //default re-pool Behavior. Override to to accomodate custom behaviors on repool.
        {
            if (PoolBank && this is IRePoolable && !(GlobalShotBank.Instance.PoolCount > GlobalShotBank.Instance.PoolMaxSize)) //and pool available
                poolingScript.AddToPool(this.gameObject, GlobalShotBank.Instance.transform);
            else
                poolingScript.AddToPool(this.gameObject, Emitter);

            GlobalShotManager.Instance.ActiveBullets--;
            poolOrDestroyTriggered = true;
        }

        protected void Kill(GameObject gO)
        {
            Destroy(gO);
            GlobalShotManager.Instance.ActiveBullets--;
            poolOrDestroyTriggered = true;
        }

        private static IWeapon GetWeaponComponent(Transform em)
        {
            var sb = em.GetComponent<ShotBase>();
            var wp = em.transform.GetComponent<IWeapon>();

            if (sb != null)
                return ShotBase.GetWeaponComponent(sb.Emitter.transform);
            if (wp != null)
                return wp;
            if (em.transform.parent == null)
                return null;
            return ShotBase.GetWeaponComponent(em.transform.parent);

        }

        public Vector3 GetStartingPosition()
        {
            return this.StartingPosition;
        }

        public GameInfo GetGameInfo()
        {
            throw new NotImplementedException();
        }
    }
}