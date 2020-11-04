using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

using Combat.AI;

public class MOBSpawn : MonoBehaviour
{
    public Tilemap tilemap; //= GetComponent<Tilemap>();
    /// <summary>
    /// Array of AICombatant PreFabs
    /// </summary>
    public AICombatant[] mobs;
    public int EnemyLimit;
    int xPos;
    int yPos;
    public int xMapMin = -10;
    public int xMapMax = 10;
    public int yMapMin = -10;
    public int yMapMax = 10;
    public bool allClear;

    public int EnemyCount { get; set; }


    //private bool SpawnHere;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyDrop());
        //Debug.Log("Tile map size"+wall.cellBounds);
    }

    void Update()
    {

    }
    IEnumerator EnemyDrop()
    {
        while (EnemyCount < EnemyLimit)
        {
            xPos = Random.Range(xMapMin, xMapMax);
            yPos = Random.Range(yMapMin, yMapMax);

            // IF mobs array is not empty
            if (this.mobs.Length > 0)
            {
                // SELECT a random AICombatant from the enem
                var enem = this.mobs[Random.Range(0, this.mobs.Length)];
                // SET AICombatant properties
                enem.EnemyTag = "Player";
                // SPAWN AICombatant
                Instantiate(enem, new Vector3(xPos, yPos, 0), Quaternion.identity);
                yield return new WaitForSeconds(0.1f);
                // UPDATE enemyCount
                EnemyCount += 1;
            }
            else
            {
                // ELSE stop loop
                EnemyCount = EnemyLimit;
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
}