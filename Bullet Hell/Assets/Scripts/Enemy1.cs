
using UnityEngine;
using Combat;

public class Enemy1 : MonoBehaviour
{
    public float speed = 5f;

    public Rigidbody2D playerRB; //the players rigid 2d
    private Rigidbody2D enemyRB; //the unit which has the script attached
    private Vector2 lookDirection; //enemy vision direction
    public GameObject rangedWeaponWrapper;
    private Weapon rangedWeapon; // Weapon prefab that should be attached to the mob
    private Combatant combatant;

    // ALGORITHM:
    // - CHECK for Rigidbody Component
    // - CHECK for Combatant Component
    private void Awake()
    {

        // CHECK for Rigidbody Component -----------------
        var rb = this.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // SET enemyRB
            this.enemyRB = rb;
            
        }
        else // IF Rigidbody component does not exist -> THROW ERR
        {
            throw new MissingComponentException(
                $"Missing component: {new Rigidbody2D().GetType().Name}");;
        }

        //  CHECK for Combatant Component
        var cp = this.GetComponent<Combatant>();
        if (cp != null)
        {
            // SET combatant
            this.combatant = cp;
        } else
        {
            // THROW err for missing component.
            throw new MissingComponentException(
                $"Missing component: {new Combatant().GetType().Name}");
        }
    }

    // ALGORITHM:
    // - Give Enemy infinite Ammo
    private void Start()
    {
        //get weapon
        rangedWeapon = this.combatant.GetComponentInChildren<Weapon>();

        //give ammo
        rangedWeapon.infAmmo = true;
    }

    // ALGORITHM:
    // - CALCULATE lookDirection
    // - IF Player is within range:
    // -   AIM at the player
    // -   SHOOT at the player
    void FixedUpdate()
    {
        lookDirection = (playerRB.transform.position - transform.position).normalized;
        // ATTACK Player if Player is within weapon range
        if (Mathf.RoundToInt(Vector3.Distance(this.transform.position, this.playerRB.transform.position))  
            <= this.combatant.RangedWeapon.range + 1) // HACK COMBAT-TEAM[1] +1 to range because is seems shorter otherwise
        {
            // AIM at the player
            this.combatant.AimRangedWeapon(this.playerRB.transform.position);
            // SHOOT at the player
            this.combatant.ShootRangedWeapon();
        }
    }

}
