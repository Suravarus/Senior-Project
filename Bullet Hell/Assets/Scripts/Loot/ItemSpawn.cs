using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
namespace Loot
{


    public class ItemSpawn : MonoBehaviour
    {
        // Arrays for items or Weapons
        public Item[] Items;
        public Weapon[] Weapons;
        public Ammo ammo;

        // Drop Rates for ItemRarity in percentage
        [Header("Spawn Rates")]
        [Tooltip("% chance that a weapon is spawned")]
        [Range(0, 100)]
        public int weaponChance;
        [Tooltip("% chance that an augment is spawned")]
        [Range(0, 100)]
        public int augmentChance;
        [Tooltip("% chance that an item is spawned")]
        [Range(0, 100)]
        public int itemChance;
        [Tooltip("Check box if ammo should be spawned")]
        public bool ammoChance;

        public bool getUniqueItem()
        {
            if (weaponChance > 0)
            {
                // roll for item rarity
                int uniqueItemRoll = Random.Range(1, 100);

                if (uniqueItemRoll <= weaponChance)
                {
                    //test = GetComponent<Item>().testing;
                    Debug.Log("Number rolled:" + uniqueItemRoll + "\nWeapon rolled");
                    return true;
                }

                else
                {
                    Debug.Log("Number rolled:" + uniqueItemRoll + "\nAugment rolled");
                    return false;
                }
            }
            else
                return false;
        }

        public void Spawn()
        {
            if (ammoChance == false)
            {
                bool isWeapon = getUniqueItem();
                Item item = null;
                Weapon weapon = null;

                switch (isWeapon)
                {
                    case false:
                        // SELECT a random Item from the Items Array
                        item = this.Items[Random.Range(0, Items.Length)];
                        Debug.Log("Augment spawned");
                        break;

                    case true:
                        // SELECT a random Item from the uncommon Items
                        weapon = this.Weapons[Random.Range(0, Weapons.Length)];
                        // SPAWN Item
                        Debug.Log("Weapon spawned");
                        break;

                    default:
                        Debug.Log("No item spawned");
                        break;
                }


                // Spawn Item
                if (item != null)
                {
                    Instantiate(item, this.transform.position, Quaternion.identity);
                    //item.transform.localScale = new Vector3(4, 4, 1);
                }

                if (weapon != null)
                {
                    Instantiate(weapon, this.transform.position, Quaternion.identity);
                    //item.transform.localScale = new Vector3(4, 4, 1);
                }
            }
            else
            {
                Instantiate(ammo, this.transform.position, Quaternion.identity);
                //item.transform.localScale = new Vector3(4, 4, 1);
            }
        }
    }
}