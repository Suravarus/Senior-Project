using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    public Transform target;
    //[SerializeField]
    //public float leftLimit;
    //[SerializeField]
    //public float rightLimit;
    //[SerializeField]
    //public float topLimit;
    //[SerializeField]
    //public float bottomLimit;

    //LateUpdates occurrs directly after Update
    void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        ////Set bound
        //transform.position = new Vector3
        //    (
        //        Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
        //        Mathf.Clamp(transform.position.x, bottomLimit, topLimit),
        //        transform.position.z
        //    );
    }


}
