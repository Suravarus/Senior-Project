using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAll : MonoBehaviour
{
    public int b;
    GameObject[] enemies;
    // Start is called before the first frame update
    void Start()
    {
        b = GameObject.Find("Random enemy").GetComponent<RandomEnemy>().enemyCount;
    }
    private void Awake()
    {
        //enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }
    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (Input.GetKeyDown("q"))
        {
            foreach (GameObject enemy in enemies)
                GameObject.Destroy(enemy);
            b = 0;
        }
    }
}

