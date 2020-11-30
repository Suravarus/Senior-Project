using UnityEngine;

namespace Structures
{
    public class SpawnPoint : MonoBehaviour
    {
        public void Start()
        {
            this.SendMessageUpwards(nameof(IFloor.OnSpawnpointReady), this, SendMessageOptions.DontRequireReceiver);
        }
    }
}
