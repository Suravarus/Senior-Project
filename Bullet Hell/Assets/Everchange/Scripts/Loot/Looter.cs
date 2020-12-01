using System;
using System.Collections.Generic;
using UnityEngine;
using Combat;
using UI;
using UnityEngine.SceneManagement;

namespace Loot
{
    public class Looter : MonoBehaviour
    {
        // PROPERTIES
        private WeaponWielder Wielder { get; set; }
        public int Gold;
        public CurrencyUI currencyUI;
        // ACCESSORS
        private Rigidbody2D RigidBody { get; set; }
        // METHODS
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
                        Gold += 10;
                        currencyUI.SetAmount(Gold);
                        Destroy(lootCollider.gameObject);
                        break;
                    case GameItem.ItemClass.Chest:
                        lootCollider.GetComponent<Chest>().OpenChest();
                        break;
                    case GameItem.ItemClass.Augment:
                        var Augment = loot.GetComponent<AugmentItem>();
                        Debug.Log("Augment is " + Augment);
                        if (Augment.augmentType == AugmentItem.Augment.shield)
                            Wielder.shieldPowerUp = true;
                        if (Augment.augmentType == AugmentItem.Augment.rateOfFire)
                            Wielder.rateOfFirePowerUp = true;
                        if (Augment.augmentType == AugmentItem.Augment.piercing)
                            Wielder.piercingPowerUp = true;
                        Destroy(lootCollider.gameObject);
                        break;
                    case GameItem.ItemClass.Portal:
                        SceneManager.LoadScene(2);
                        break;
                    case GameItem.ItemClass.ammo:
                        var ammoDrop = loot.GetComponent<GameItem>();
                        if (ammoDrop.inShop == true && Gold >= ammoDrop.__itemPrice)
                        {
                            Wielder.RangedWeapon.AmmoCount += 50;
                            Gold -= ammoDrop.__itemPrice;
                        }
                        else
                        {
                            Debug.Log("Insufficient funds");
                        }
                        break;
                }
            }
        }
        
        // MONOBEHAVIOUR
        public void Awake()
        {
            Wielder = this.GetComponent<WeaponWielder>();
        }
        private void Start()
        {
            if (this.currencyUI == null)
                throw new MissingFieldException(nameof(this.currencyUI));
        }
    }
}