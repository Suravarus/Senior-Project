using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Loot
{


    public class ItemDrop : MonoBehaviour
    {
        // Array for item rarities
        public Item[] commonItems;
        public Item[] uncommonItems;
        public Item[] rareItems;

        // Drop Rates for ItemRarity in percentage
        [Header("Drop Rates")]
        [Tooltip("% that an Item should be dropped")]
        [Range (1, 100)]
        public int itemDropRate;
        [Tooltip("If an item is dropped, % that a common item is dropped")]
        [Range(1, 100)]
        public int common;
        [Tooltip("If an item is dropped, % that a uncommon item is dropped")]
        [Range(1, 100)]
        public int uncommon;
        [Tooltip("If an item is dropped, % that a rare item is dropped")]
        [Range(1, 100)]
        public int rare;

        public Item.Rarity getRarity()
        {
            // roll for item rarity
            int rarityRoll = Random.Range(1, 100);
            if (rarityRoll <= common)
            {
                Debug.Log("Number rolled:" + rarityRoll + "\nCommon item rolled" );
                return Item.Rarity.common;
            }
            else if (rarityRoll > common + uncommon && rarityRoll <= 100 - rare)
            {
                Debug.Log("Number rolled:" + rarityRoll + "\nUncommon item rolled");
                return Item.Rarity.uncommon;
            }
            else
            {
                Debug.Log("Number rolled:" + rarityRoll + "\nRare item rolled");
                return Item.Rarity.rare;
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
                Item item = null;
                Item.Rarity rarity = getRarity();

                switch (rarity)
                {
                    case Item.Rarity.common:
                        // SELECT a random Item from the common Items
                        item = this.commonItems[Random.Range(0, commonItems.Length)];
                        // SPAWN Item
                        Debug.Log("Common item spawned");
                        break;


                    case Item.Rarity.uncommon:
                        // SELECT a random Item from the uncommon Items
                        item = this.uncommonItems[Random.Range(0, uncommonItems.Length)];
                        // SPAWN Item
                        Debug.Log("UnCommon item spawned");
                        break;


                    case Item.Rarity.rare:
                        // SELECT a random Item from the rare Items
                        item = this.rareItems[Random.Range(0, rareItems.Length)];
                        // SPAWN Item
                        Debug.Log("Rare item spawned");
                        break;

                    default:
                        Debug.Log("No item spawned");
                        break;
                }
                
                if (item != null)
                {
                    Instantiate(item, this.transform.position, Quaternion.identity);
                    item.transform.localScale = new Vector3(4, 4, 1);
                }
            }
        }
    }
}
