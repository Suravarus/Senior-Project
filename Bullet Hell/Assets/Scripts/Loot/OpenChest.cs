using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject chest;
    List<GameObject> dropPool = new List<GameObject>();
    GameObject[] aPrefab;
    void Start()
    {
        aPrefab = Resources.LoadAll<GameObject>("Prefabs/Weapon");
        if (aPrefab.Length != 0)
        {
            foreach (GameObject item in aPrefab)
            {
                dropPool.Add(item);
                Debug.Log(item.name);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            Vector3 chestpos = this.transform.position;
            var chestsize = this.transform.localScale;
            Destroy(gameObject);
            chest.transform.localScale = chestsize;
            Instantiate(chest, chestpos, Quaternion.identity);
            for (int i = 0; i < aPrefab.Length; i++)
            {
                int randomItem = Random.Range(0, aPrefab.Length);
                Instantiate(dropPool[randomItem], chestpos, Quaternion.identity);
            }
        }
    }
}
