using Combat;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.Tilemaps;
public class DangerZone : MonoBehaviour
{
    public Tilemap danger;
    //GameObject player = GameObject.Find("Player_v2");
    //Combatant playerScript = player.GetComponent<Combatant>();
    //[SerializeField] private int health = GameObject.Find("Player_v2").GetComponent<Combatant>()._health;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Player")
        {
            GameObject.Find("Player_v2").GetComponent<Combatant>()._health = 5;
            Debug.Log("Entered");
        }
    }
}
