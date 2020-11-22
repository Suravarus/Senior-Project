using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public class Chest : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject chest;
        public enum ChestType { Weapon, Item, PowerUp };
        public ChestType dropType;
        List<GameObject> dropPool = new List<GameObject>();
        GameObject[] aPrefab;
        Collider2D touch = null;
        void Start()
        {
            //aPrefab = Resources.LoadAll<GameObject>("Prefabs/Weapon");

            //Debug.Log(dropType);
            switch (dropType)
            {
                case ChestType.Weapon:
                    aPrefab = Resources.LoadAll<GameObject>("Prefabs/Obtainable/Weapon");
                    break;
                case ChestType.Item:
                    aPrefab = Resources.LoadAll<GameObject>("Prefabs/Obtainable/Items");
                    break;
                case ChestType.PowerUp:
                    aPrefab = Resources.LoadAll<GameObject>("Prefabs/Obtainable/PowerUps");
                    break;
            }
            if (aPrefab.Length != 0)
            {
                foreach (GameObject item in aPrefab)
                {
                    dropPool.Add(item);
                }
            }

        }
        public void OpenChest()
         {

                Vector3 chestpos = this.transform.position;
                Vector3 dropPos = new Vector3(chestpos.x, chestpos.y - 1);
                var chestsize = this.transform.localScale;
                Destroy(gameObject);
                chest.transform.localScale = chestsize;
                Instantiate(chest, chestpos, Quaternion.identity);
                int randomItem = Random.Range(0, aPrefab.Length);
                Instantiate(dropPool[randomItem], dropPos, Quaternion.identity);
            }
        }
    

