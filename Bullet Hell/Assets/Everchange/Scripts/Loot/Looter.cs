using System;
using System.Collections.Generic;
using UnityEngine;
using Combat;
using UI;

namespace Loot
{
    public class Looter : MonoBehaviour
    {
        public Rigidbody2D rb;
        public WeaponWielder wielder;
        public AugmentItem augmentItem;

        private int gold = 0;
        public CurrencyUI currencyUI;
        private void Start()
        {
            if (this.currencyUI == null)
                throw new MissingFieldException(nameof(this.currencyUI));
        }

        public void Awake()
        {
            wielder = this.GetComponent<WeaponWielder>();
        }
        public void PickupLoot(Collider2D lootCollider)
        {
            var loot = lootCollider.GetComponent<GameItem>();
            if (loot != null)
            {
                Debug.Log(loot.GetClassID() + " " + loot.gameObject.name);
                switch (loot.GetClassID())
                {
                    case GameItem.ItemClass.Weapon:
                        var weapon = lootCollider.GetComponent<IWeapon>();
                        weapon.GetGameObject().GetComponent<CircleCollider2D>()
                            .enabled = false;
                        this.GetComponent<WeaponWielder>()
                            .GetQuarterMaster()
                            .PickupWeapon(weapon);
                        break;
                    case GameItem.ItemClass.Consumable:
                        lootCollider.GetComponent<Combatant>().Health += 300;
                        Destroy(lootCollider.gameObject);
                        break;
                    case GameItem.ItemClass.Currency:
                        gold += 10;
                        currencyUI.SetAmount(gold);
                        Destroy(lootCollider.gameObject);
                        break;
                    case GameItem.ItemClass.Chest:
                        lootCollider.GetComponent<Chest>().OpenChest();
                        break;
                    case GameItem.ItemClass.Augment:
                        var Augment = loot.GetComponent<AugmentItem>();
                        Debug.Log("Augment is " + Augment);
                        if (Augment.augmentType == AugmentItem.Augment.shield)
                            wielder.shieldPowerUp = true;
                        if (Augment.augmentType == AugmentItem.Augment.rateOfFire)
                            wielder.rateOfFirePowerUp = true;
                        if (Augment.augmentType == AugmentItem.Augment.piercing)
                            wielder.piercingPowerUp = true;
                        Destroy(lootCollider.gameObject);
                        break;  
                }
            }
        }
        
    }
}