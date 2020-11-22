using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUpdate : MonoBehaviour
{
    [Tooltip("Update the Graph every this many frames")]
    public int interval = 50;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.frameCount % interval == 0)
        {
            AstarPath.active.Scan();
        }
    }
}
