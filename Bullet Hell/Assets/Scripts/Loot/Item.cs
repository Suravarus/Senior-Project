using UnityEngine;

namespace Loot
{ 
    public class Item : MonoBehaviour
    {
        public Rarity Item_Rarity;
        public enum Rarity
        {
            common,
            uncommon,
            rare
        }
    }

}
