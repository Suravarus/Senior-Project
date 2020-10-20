
using System;
using UnityEngine;

using Combat;

[Obsolete("Prefabs using this class will soon be deleted. Please use updated Enemy prefabs.")]
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
    // - SEARCH for Player's Rigidbody Component
    // - SET PlayerRB field
    // - CHECK for Combatant Component
    private void Awake()
    {

        // SEARCH for Player's Rigidbody Component -----------------
        var rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // SET PlayerRB field
            this.playerRB = rb;
            
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
    // - IF Player is Alive
    // -   CALCULATE lookDirection
    // -   IF Player is within range:
    // -     AIM at the player
    // -     SHOOT at the player
    void FixedUpdate()
    {
        if (this.playerRB.GetComponent<Combatant>().IsAlive())
        {
            lookDirection = (playerRB.transform.position - transform.position).normalized;
            // ATTACK Player if Player is within weapon range
            if (Mathf.RoundToInt(Vector3.Distance(this.transform.position, this.playerRB.transform.position))
                <= this.combatant.RangedWeapon.range)
            {
                // AIM at the player
                this.combatant.AimRangedWeapon(this.playerRB.transform.position);
                // SHOOT at the player
                this.combatant.ShootRangedWeapon();
            }
        }
    }

}
