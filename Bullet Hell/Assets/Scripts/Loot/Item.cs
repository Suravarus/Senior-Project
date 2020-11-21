using UnityEngine;

namespace Loot
{ 
    public class Item : MonoBehaviour
    {
        public bool inShop = false;
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
