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
        Collider2D touch = null;
        private void Start()
        {
            if (this.currencyUI == null)
                throw new MissingFieldException(nameof(this.currencyUI));
        }

        private void Update()
        {
            if (GetComponent<PlayerMovement>().Fkey == true && touch != null)
            {
                if (touch.CompareTag("Items"))
                {

                    //Item itemType = other.gameObject.GetComponent<itemType>().type;
                    //inven.Add(itemType);

                    Destroy(touch.gameObject);
                }

                if (touch.GetComponent<Weapon>() != null)
                {
                    var weapon = touch.GetComponent<Weapon>();
                    weapon.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                    this.GetComponent<WeaponWielder>().GetQuarterMaster().PickupWeapon(weapon);
                }

                if (touch.CompareTag("Regen"))
                {
                    touch.GetComponent<Combatant>().Health += 300;
                    Destroy(touch.gameObject);
                }

                if (touch.CompareTag("Coins"))
                {
                    gold += 10;
                    currencyUI.SetAmount(gold);
                    Destroy(touch.gameObject);
                }

                //if (other.CompareTag("Ammo"))
                //{
                //     += 10;
                //    Destroy(other.gameObject);
                //}
            }
        }
        void OnTriggerStay2D(Collider2D other)
        {
            touch = other;
        }
        
    }
}