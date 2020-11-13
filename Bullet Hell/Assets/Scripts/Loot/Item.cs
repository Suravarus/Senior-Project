using UnityEngine;

namespace Loot
{ 
    public class Item : MonoBehaviour
    {
        public Rarity Item_Rarity;
        //public string testing;
        public enum Rarity
        {
            common,
            uncommon,
            rare,
        }
    }

}
