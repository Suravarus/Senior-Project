using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemy : MonoBehaviour
{
    public Rigidbody2D[] enemy;
    public int EnemyLimit;
     int xPos;
     int yPos;
     int enemyCount;
    public int xMapMin;
    public int xMapMax;
    public int yMapMin;
    public int yMapMax;

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
            Instantiate(enemy[Random.Range(0,2)], new Vector3(xPos, yPos, 0), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
        }
    }


}
