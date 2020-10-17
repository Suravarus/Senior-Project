using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class RandomEnemy : MonoBehaviour
{
    public Enemy1[] enemy;
    public Tilemap tilemap; //= GetComponent<Tilemap>();
    public int EnemyLimit;
    int xPos;
    int yPos;
    int enemyCount;
    public int xMapMin = -10;
    public int xMapMax = 10;
    public int yMapMin = -10;
    public int yMapMax = 10;
 

    //private bool SpawnHere;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyDrop());
    }

    void Update()
    {

    }
    IEnumerator EnemyDrop()
    {   //TESTING
    //    foreach (var position in tilemap.cellBounds.allPositionsWithin)
    //    //SpawnHere = false;
    //    {
    //        tilemap.SetColor(position, Color.yellow);
    //        Debug.Log("Hello: " + position);
    //    }

        while (enemyCount < EnemyLimit)
            {

                xPos = Random.Range(xMapMin, xMapMax);
                yPos = Random.Range(yMapMin, yMapMax);

                // get player object
                var player = GameObject.FindObjectOfType<PlayerMovement>();

                // set playerrb in enemy object
                var enem = enemy[Random.Range(0, 1)];
                enem.playerRB = player.rb;
                Vector3Int position =new Vector3Int(xPos , yPos, 0);
            if (tilemap.HasTile(position))
            {
                Debug.Log("no spawn: " + position);
            }
            else
            {
                Instantiate(enem, position, Quaternion.identity);
                yield return new WaitForSeconds(0.1f);
                Debug.Log("NO WALL: " + position);
                enemyCount += 1;
            }
            }
        }
    }

//SAVING PART FOR TEST
//public Tilemap tilemap;
//public List<Vector3> tileWorldLocations;

//// Use this for initialization
//void Start()
//{
//    tileWorldLocations = new List<Vector3>();

//    foreach (var pos in tilemap.cellBounds.allPositionsWithin)
//    {
//        Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
//        Vector3 place = tilemap.CellToWorld(localPlace);
//        if (tilemap.HasTile(localPlace))
//        {
//            tileWorldLocations.Add(place);
//        }
//    }

//    print(tileWorldLocations);
//}