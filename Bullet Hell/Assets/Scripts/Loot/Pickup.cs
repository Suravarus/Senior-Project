using System;
using System.Collections.Generic;
using UnityEngine;
using Combat;
using UI;
using System.Collections;
namespace Loot
{
    public class Pickup : MonoBehaviour
    {
        public List<Item> inven;
        public int gold = 0;
        public CurrencyUI currencyUI;
        private void Start()
        {
            if (this.currencyUI == null)
                throw new MissingFieldException(nameof(this.currencyUI));
        }

        public void PickupLoot(Collider2D lootCollider)
        {
            if (lootCollider.CompareTag("Items"))
            {
                
                //Item itemType = other.gameObject.GetComponent<itemType>().type;
                //inven.Add(itemType);
               
                Destroy(lootCollider.gameObject);
            }

            if (lootCollider.GetComponent<Weapon>() != null)
            {
                var weapon = lootCollider.GetComponent<Weapon>();
                weapon.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                this.GetComponent<WeaponWielder>().GetQuarterMaster().PickupWeapon(weapon);
            }

            if (lootCollider.CompareTag("Regen"))
            {
                lootCollider.GetComponent<Combatant>().Health += 300;
                Destroy(lootCollider.gameObject);
            }

            if (lootCollider.CompareTag("Coins"))
            {
                gold += 10;
                currencyUI.SetAmount(gold);
                Destroy(lootCollider.gameObject);
            }
        }
        
    }
}