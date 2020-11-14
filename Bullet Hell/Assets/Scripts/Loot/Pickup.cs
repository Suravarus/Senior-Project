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
        public string weapname;
        
        //private Inventory inventory;
        // Start is called before the first frame update
        //private void Start()
        //{
        //    inventory = GameObject.FindGameObjectsWithTag("Player").GetComponent<Inventory>();
        //}

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Items"))
            {
                weapname = other.gameObject.name.ToString();
                //Item itemType = other.gameObject.GetComponent<itemType>().type;
                //inven.Add(itemType);
                Debug.Log(weapname);
                Destroy(other.gameObject);
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