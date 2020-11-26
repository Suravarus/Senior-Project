using System.Collections.Generic;
using UnityEngine;

namespace ProcGen
{
    public class FloorGenerator : MonoBehaviour
    {
        private void Awake()
        {
            var roomGamgeObjects = Resources.LoadAll("Rooms");
            var rooms = new List<IRoom>();
            
            foreach(GameObject g in roomGamgeObjects)
            {
                var r = g.GetComponent<IRoom>();
                if (r != null)
                    rooms.Add(r);
            }


        }
    }
}
