using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
namespace Loot
{
    public class ItemDrop : MonoBehaviour
    {
        // Array for items
        public GameItem[] commonItems; // if shop spawn, put augments in commonItems
        public GameItem[] uncommonItems;
        public GameItem[] rareItems;
        public Weapon[] weapons;
        public bool uniqueItem;
        public bool shopSpawn = false;
        //public string test;

        // Drop Rates for ItemRarity in percentage
        [Header("Drop Rates")]
        [Tooltip("% that an Item should be dropped")]
        [Range (0, 100)]
        public int itemDropRate;
        [Tooltip("If an item is dropped, % that a common item is dropped")]
        [Range(0, 100)]
        public int common;
        [Tooltip("If an item is dropped, % that a uncommon item is dropped")]
        [Range(0, 100)]
        public int uncommon;
        [Tooltip("If an item is dropped, % that a rare item is dropped")]
        [Range(0, 100)]
        public int rare;
        [Tooltip("If isWeapon is true, % that a weapon is dropped")]
        [Range(0, 100)]
        public int weaponChance;

        public void Start()
        {
            if (shopSpawn == true)
            {
                Spawn();
                Debug.Log("Shop spawn function ran");
            }

        }

        public GameItem.Rarity getRarity()
        {
            // roll for item rarity
            int rarityRoll = Random.Range(1, 100);
            if (rarityRoll <= common)
            {
                //test = GetComponent<Item>().testing;
                Debug.Log("Number rolled:" + rarityRoll + "\nCommon item rolled" );
                return GameItem.Rarity.common;
            }
            else if (rarityRoll > common + uncommon && rarityRoll <= 100 - rare)
            {
                Debug.Log("Number rolled:" + rarityRoll + "\nUncommon item rolled");
                return GameItem.Rarity.uncommon;
            }
            else
            {
                Debug.Log("Number rolled:" + rarityRoll + "\nRare item rolled");
                return GameItem.Rarity.rare;
            }
        }

        public bool getUniqueItem()
        {
            int uniqueItemRoll = Random.Range(1, 100);
            if (uniqueItemRoll <= weaponChance)
            {
                Debug.Log("Number rolled " + uniqueItemRoll + "\nWeapon rolled");
                return true;
            }
            else
            {
                Debug.Log("Number rolled " + uniqueItemRoll + "\nAugment rolled");
                return false;
            }
        }

        public void Spawn()
        {

            // Roll for chance of item dropping
            bool dropItem;
            int itemRoll = Random.Range(1, 100);
            if (itemRoll <= itemDropRate)
            {
                dropItem = true;
            }
            else
            {
                dropItem = false;
            }

            //SELECT which item will drop if dropItem is true
            if (dropItem == true)
            {
                GameItem item = null;
                Weapon weapon = null;
                
                // For shop spawns
                if (uniqueItem == true)
                {
                    if (weaponChance + common != 100)
                    {
                        weaponChance = 50;
                        common = 50;
                        Debug.Log("Need to set the rates for spawns, default set to 50/50 for this run");
                    }

                    bool isWeapon = getUniqueItem();

                    if (isWeapon == true)
                    {
                        weapon = this.weapons[Random.Range(0, weapons.Length)];
                    }
                    else
                    {
                        item = this.commonItems[Random.Range(0, commonItems.Length)];
                    }
                }                  
            
                // For enemy drops
                else
                {
                    // set default percentages if rates do not add up to 100%
                    if (common + uncommon + rare != 100)
                    {
                        common = 65;
                        uncommon = 30;
                        rare = 5;
                        Debug.Log("Need to set the rates for drops, default set to 65/30/5 for this run");                        
                    }

                    GameItem.Rarity rarity = getRarity();

                    switch (rarity)
                    {
                        case GameItem.Rarity.common:
                            // SELECT a random Item from the common Items
                            item = this.commonItems[Random.Range(0, commonItems.Length)];
                            // SPAWN Item
                            Debug.Log("Common item spawned");
                            break;


                        case GameItem.Rarity.uncommon:
                            // SELECT a random Item from the uncommon Items
                            item = this.uncommonItems[Random.Range(0, uncommonItems.Length)];
                            // SPAWN Item
                            Debug.Log("UnCommon item spawned");
                            break;


                        case GameItem.Rarity.rare:
                            // SELECT a random Item from the rare Items
                            item = this.rareItems[Random.Range(0, rareItems.Length)];
                            // SPAWN Item
                            Debug.Log("Rare item spawned");
                            break;

                        default:
                            Debug.Log("No item spawned");
                            break;
                    }
                }

                if (item != null)
                {
                    if (shopSpawn == true)
                    {
                        item.inShop = true;
                    }
                    //var vector = new Vector3(this.transform.position.x, this.transform.position.y, 1);
                    Instantiate(item, this.transform.position, this.transform.rotation);
                    
                    //item.transform.localScale = new Vector3(4, 4, 1);
                }
                else if (weapon != null)
                {
                    if (shopSpawn == true)
                    {
                        // FIXME [VENDOR] weapon.inShop does not exist - @JC
                        // the shop should keep track of its inventory.
                        // weapon.inShop = true;
                    }
                    Instantiate(weapon, this.transform.position, this.transform.rotation);
                    weapon.transform.localScale = new Vector3(3, 3, 1);
                }
                else
                    Debug.Log("Nothing was dropped");
            }
            else
                Debug.Log("Number rolled " + itemRoll + "\nNothing Dropped");
        }
    }
}
