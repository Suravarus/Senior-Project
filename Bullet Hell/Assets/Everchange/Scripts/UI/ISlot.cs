using UnityEngine;

namespace UI
{
    public interface ISlot
    {
        bool ShowIcon { get; set; }
        bool Active { get; set; }
        void SetIcon(Sprite sprite, Vector2 scaling);
        void SetIndex(int i);
        int GetIndex();
    }
}
