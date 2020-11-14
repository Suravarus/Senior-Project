using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Loot
{
    public class TestingWeaponSwap : MonoBehaviour
    {
        [HideInInspector]
        public string weapon;
        //public string weapon = GetComponent<Pickup>().weapname;
        //public Rigidbody2D rb;
        // Start is called before the first frame update
        
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            //weapon = FindObjectOfType<Pickup>().weapname;
            //weapon = GetComponent<Pickup>().weapname.ToString();
            //string wname = GetComponent<Pickup>().weapname.ToString();
            Debug.Log(GameObject.Find("PlayerWithWeapon").GetComponent<Pickup>().weapname); 
            if (Input.GetKeyDown("h"))
            {
                
                this.transform.GetChild(0).gameObject.SetActive(false);
                this.transform.Find(weapon).gameObject.SetActive(true);
            }
        }
    }
}