using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Destroyalbe : MonoBehaviour
{
    public Tilemap destroyable;
    // Start is called before the first frame update
    void Start()
    {
        destroyable = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            Vector3 hitPos = Vector3.zero;
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPos.x = hit.point.x - 0.01f * hit.normal.x;
                hitPos.y = hit.point.y - 0.01f * hit.normal.y;

                //hitPos.x = hit.normal.x;
                //hitPos.y = hit.normal.y;
                destroyable.SetTile(destroyable.WorldToCell(hitPos), null);

                //hitPos = destroyable.WorldToCell(hit.point);

                //hitPos.x = hit.point.x ;
                //hitPos.y = hit.point.y ;
                //destroyable.SetTile(hitPos, null);
            }
        }
    }
}