using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
namespace Loot
{
    public class Pickup : MonoBehaviour
    {
        public List<Item> inven;
        public int gold = 0;
        

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
                this.GetComponent<WeaponWielder>().GetQuarterMaster().PickupWeapon(other.GetComponent<Weapon>());
                
            }

            if (other.CompareTag("Regen"))
            {
                other.GetComponent<Combatant>().Health += 300;
                Destroy(other.gameObject);
            }

            if (other.CompareTag("Coins"))
            {
                gold += 10;
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