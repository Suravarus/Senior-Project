using UnityEngine;

namespace ProcGen
{
    public class RoomPlaceholder : MonoBehaviour
    {
        public enum RoomType
        {
            shop,
            normal,
            chest
        }
        public RoomType roomType;
        public bool __hallLeft;
        public bool __hallTop;
        public bool __hallRight;
        public bool __hallBottom;
    }
}
