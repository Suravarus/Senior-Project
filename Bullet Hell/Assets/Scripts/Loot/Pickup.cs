using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public List<string> inven;
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
            string itemType = other.gameObject.GetComponent<itemType>().type;
            inven.Add(itemType);
            Destroy(other.gameObject);
        }
    }
}
