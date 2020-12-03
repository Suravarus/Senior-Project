using UnityEngine;

namespace Structures
{
    public class SpawnPoint : MonoBehaviour
    {
        public void Start()
        {
            var floor = GameObject.FindObjectOfType<Floor>();
            floor.OnSpawnpointReady(this);
        }
    }
}
