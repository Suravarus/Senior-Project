using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
using UI;
namespace Loot
{
    public class Pickup : MonoBehaviour
    {
        public List<Item> inven;
        public int gold = 0;
        public CurrencyUI CurrencyUI;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Items"))
            {
                
                //Item itemType = other.gameObject.GetComponent<itemType>().type;
                //inven.Add(itemType);
               
                Destroy(other.gameObject);
            }

            if (other.GetComponent<Weapon>() != null)
            {
                var weapon = other.GetComponent<Weapon>();
                weapon.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                this.GetComponent<WeaponWielder>().GetQuarterMaster().PickupWeapon(weapon);
            }

            if (other.CompareTag("Regen"))
            {
                other.GetComponent<Combatant>().Health += 300;
                Destroy(other.gameObject);
            }

            if (other.CompareTag("Coins"))
            {
                gold += 10;
                CurrencyUI.SetAmount(gold);
                Destroy(other.gameObject);
            }

            //if (other.CompareTag("Ammo"))
            //{
            //     += 10;
            //    Destroy(other.gameObject);
            //}
        }
       
    }
}