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
        public int Gold = 0;
        public int _keyCount = 1; // FIXME gives player 1 key for free
        public CurrencyUI currencyUI;
        public CurrencyUI keyUI;
        // ACCESSORS
        public int KeyCount
        {
            set
            {
                this._keyCount = value;
                this.keyUI.SetAmount(this._keyCount);
            }
            get => this._keyCount;
        }
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
                    //Weapon
                    case GameItem.ItemClass.Weapon:
                        var weapon = lootCollider.GetComponent<IWeapon>();
                        if (weapon.GetGameObject().GetComponent<GameItem>().inShop == true && Gold >= weapon.GetGameInfo().GetPrice())
                        {
                            Gold -= weapon.GetGameInfo().GetPrice();
                            currencyUI.SetAmount(Gold);
                            weapon.GetGameObject().GetComponent<CircleCollider2D>()
                            .enabled = false;
                            this.GetComponent<WeaponWielder>()
                                .GetQuarterMaster()
                                .PickupWeapon(weapon);
                        }
                        else if (weapon.GetGameObject().GetComponent<GameItem>().inShop == false)
                        {
                            weapon.GetGameObject().GetComponent<CircleCollider2D>()
                            .enabled = false;
                            this.GetComponent<WeaponWielder>()
                                .GetQuarterMaster()
                                .PickupWeapon(weapon);
                        }
                        else
                            Debug.Log("Insufficient funds");
                        break;
                    //Health
                    case GameItem.ItemClass.Consumable:
                        var shopHealth = loot.GetComponent<GameItem>();
                        if (shopHealth.inShop == true && Gold >= shopHealth.__itemPrice)
                        {                            
                            Wielder.Health += 50;
                            Debug.Log("Health Detected");
                            Gold -= shopHealth.__itemPrice;
                            currencyUI.SetAmount(Gold);
                            Destroy(lootCollider.gameObject);
                        }
                        else if (shopHealth.inShop == false)
                        {
                            Wielder.Health += 25;
                            Debug.Log("Health Detected");
                            Destroy(lootCollider.gameObject);
                        }
                        else
                        {
                            Debug.Log("Insufficient funds");
                        }
                        break;
                    //Gold
                    case GameItem.ItemClass.Currency:
                        Gold += 10;
                        currencyUI.SetAmount(Gold);
                        Destroy(lootCollider.gameObject);
                        break;
                    case GameItem.ItemClass.Chest:
                        lootCollider.GetComponent<Chest>().OpenChest();
                        keyUI.SetAmount(_keyCount);
                        break;
                    //Augment
                    case GameItem.ItemClass.Augment:
                        var Augment = loot.GetComponent<AugmentItem>();
                        Debug.Log("Augment is " + Augment);
                        if (Augment.inShop == true && Gold >= Augment.__itemPrice)
                        {
                            if (Augment.augmentType == AugmentItem.Augment.shield)
                                Wielder.shieldPowerUp = true;
                            if (Augment.augmentType == AugmentItem.Augment.rateOfFire)
                                Wielder.rateOfFirePowerUp = true;
                            if (Augment.augmentType == AugmentItem.Augment.piercing)
                                Wielder.piercingPowerUp = true;
                            Gold -= Augment.__itemPrice;
                            currencyUI.SetAmount(Gold);
                            Destroy(lootCollider.gameObject);
                        }
                        else if (Augment.inShop == false)
                        {
                            if (Augment.augmentType == AugmentItem.Augment.shield)
                                Wielder.shieldPowerUp = true;
                            if (Augment.augmentType == AugmentItem.Augment.rateOfFire)
                                Wielder.rateOfFirePowerUp = true;
                            if (Augment.augmentType == AugmentItem.Augment.piercing)
                                Wielder.piercingPowerUp = true;
                            Destroy(lootCollider.gameObject);
                        }
                        else
                            Debug.Log("Insufficient Funds");
                        break;
                    //Ammo
                    case GameItem.ItemClass.ammo:
                        var ammoDrop = loot.GetComponent<GameItem>();
                        if (ammoDrop.inShop == true && Gold >= ammoDrop.__itemPrice)
                        {
                            Wielder.RangedWeapon.AmmoCount += 50;
                            Gold -= ammoDrop.__itemPrice;
                            currencyUI.SetAmount(Gold);
                            Destroy(lootCollider.gameObject);
                        }
                        else
                        {
                            Debug.Log("Insufficient funds" + Gold);
                        }
                        break;
                    case GameItem.ItemClass.key:
                        var keyDrop = loot.GetComponent<GameItem>();
                        if (keyDrop.inShop == true && Gold >= keyDrop.__itemPrice)
                        {
                            Gold -= keyDrop.__itemPrice;
                            currencyUI.SetAmount(Gold);
                            _keyCount += 1;
                            keyUI.SetAmount(_keyCount);
                            Destroy(lootCollider.gameObject);
                        }
                        else
                            Debug.Log("Insufficient Funds");
                        break;
                    case GameItem.ItemClass.Portal:
                        SceneManager.LoadScene(2);
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
            try { 
                keyUI.SetAmount(_keyCount);
                this.currencyUI.SetAmount(Gold);
            }
            catch(Exception ex) { Debug.Log($"looter {ex.Message}"); }
        }
    }
}