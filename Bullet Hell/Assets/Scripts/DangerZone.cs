using Combat;
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
       danger.GetComponent<CompositeCollider2D>().isTrigger = true;

    }

    // Update is called once per frame
    void Update()
    {
      
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            danger.GetComponent<CompositeCollider2D>().isTrigger = true;
            // KILL player
            collision.GetComponent<Combatant>().Health = 0;
            
            Debug.Log("Entered");
        }

        if (collision.tag == "Enemy")
        {
            danger.GetComponent<CompositeCollider2D>().isTrigger = false;
            //GameObject.Find("Player_v2").GetComponent<Combatant>()._health = 5;
            //Debug.Log("Entered");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            danger.GetComponent<CompositeCollider2D>().isTrigger = true;
            Debug.Log("Still HERE");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            danger.GetComponent<CompositeCollider2D>().isTrigger = false;
        }
    }
}
