using UnityEngine;

namespace UI
{
    public class UICanvas : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
