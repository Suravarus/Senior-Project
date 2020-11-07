using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUpdate : MonoBehaviour
{
    public float time = 1f;
    int count = 0;
    int frames = 50;

    // Update is called once per frame
    void FixedUpdate()
    {
        //frames = (int)(50 * time);
        count++;
        if (count > frames)
        {
            AstarPath.active.Scan();
            count = 0;
        }
    }
}
