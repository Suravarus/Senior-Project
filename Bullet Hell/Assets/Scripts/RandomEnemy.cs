using System.Collections;
using UnityEngine;

using Combat.AI;

public class RandomEnemy : MonoBehaviour
{
    public AICombatant[] enemy;
    public int EnemyLimit;
     int xPos;
     int yPos;
     int enemyCount;
    public int xMapMin = -10;
    public int xMapMax = 10;
    public int yMapMin = -10;
    public int yMapMax = 10;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyDrop());
    }
    IEnumerator EnemyDrop()
    {
        while (enemyCount < EnemyLimit)
        {
            xPos = Random.Range(xMapMin, xMapMax);
            yPos = Random.Range(yMapMin, yMapMax);

            // get player object
            var player = GameObject.FindObjectOfType<PlayerMovement>();
            
            // set playerrb in enemy object
            var enem = enemy[Random.Range(0, 2)];
            enem.EnemyTag = "Player";
            
            Instantiate(enem, new Vector3(xPos, yPos, 0), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
        }
    }


}
