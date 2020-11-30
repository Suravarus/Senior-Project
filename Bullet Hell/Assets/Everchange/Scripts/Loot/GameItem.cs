using UnityEngine;
using Utilities;

namespace Loot
{ 
    /// <summary>
    /// All in-game item prefabs should have this component.
    /// </summary>
    [RequireComponent(typeof(PickupRadius))]
    public class GameItem : CompGameInfo
    {
        /// <summary>
        /// All in-game items fall under one of these classes.
        /// </summary>
        public enum ItemClass
        {
            Weapon,
            Currency,
            /// <summary>
            /// One-time use items that the player can 
            /// consume and that produce some type of effect.
            /// </summary>
            Consumable,
            /// <summary>
            /// Similar to Consumables, but these are limited
            /// to producing a combat effect.
            /// </summary>
            Augment,
            /// <summary>
            /// Has loot that the player can access once all requirements
            /// are met.
            /// </summary>
            Chest,
            /// <summary>
            /// Player can stand in the portal to teleport to predetermined
            /// areas.
            /// </summary>
            Portal,
            /// <summary>
            /// Player can use keys to open chests. reduce total key count 
            /// on use.
            /// </summary>
            key,
            /// <summary>
            /// Obtainable ammo to add to a gun's ammo count.
            /// </summary>
            ammo
        }
        //public string testing;
        public enum Rarity
        {
            common,
            uncommon,
            rare,
        }
        // UNITY EDITOR
        public bool inShop = false;
        public Rarity Item_Rarity;
        public ItemClass __classID;
        // ACCESSORS
        private ItemClass ClassID { get; set; }

        // MONOBEHAVIOR
        protected override void Awake()
        {
            base.Awake();
            this.ClassID = this.__classID;
        }

        // METHODS
        /// <summary>
        /// Returns the ItemClass of this object.
        /// </summary>
        /// <returns></returns>
        public ItemClass GetClassID() => this.ClassID;
    }

}
