using System;

namespace Utilities
{
    /// <summary>
    /// Contains information that the player will see about
    /// the parent object.
    /// </summary>
    public class GameInfo
    {
        // PROPERTIES
        private String _lowercaseName; // ex. Thunder Arrow
        private String _description;

        // ACCESSORS
        public static int DESCRIPTION_CHAR_LIMIT => 100;
        public static int NAME_CHAR_LIMIT => 20;
        private bool Saleable { get; set; }
        /// <summary>
        /// The lowercase name of the gameObject.
        /// </summary>
        private String LowercaseName
        {
            set
            {
                if (!String.IsNullOrEmpty(value))
                    this._lowercaseName = value.ToLower();
                else
                    throw new ArgumentNullException(
                        nameof(this.LowercaseName), "Cannot be null or empty.");
            }

            get { return this._lowercaseName; }
        }
        private float Price { get; set; }
        /// <summary>
        /// Description of this item. Should be a short sentence that 
        /// adheres to the character-limit.
        /// </summary>
        private String Description
        {
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    if(value.Length <= GameInfo.DESCRIPTION_CHAR_LIMIT)
                    {
                        this._description = value;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException(nameof(this.Description)
                            , value
                            , $"Cannot be longer than {GameInfo.DESCRIPTION_CHAR_LIMIT}.");
                    }
                }
                else
                {
                    throw new ArgumentNullException(
                        nameof(this.Description), "Cannot be NULL or empty.");
                }
            }

            get { return this._description; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Name of this item <code>GameInfo.NAME_CHAR_LIMIT</code></param>
        /// <param name="description">Short, one-sentence, description of this item.
        /// <code>GameInfo.DESCRIPTION_CHAR_LIMIT</code>
        /// </param>
        /// <param name="price"></param>
        /// <param name="canSell"></param>
        public GameInfo(String name
            , String description
            , float price = 0f, bool canSell = false)
        {
            this.LowercaseName = name;
            this.Description = description;
            this.Saleable = canSell;
            this.Price = price;
        }

        /// <summary>
        /// The name of this item in lower-case.
        /// </summary>
        /// <returns></returns>
        public string GetLowercaseName() => this.LowercaseName;
        /// <summary>
        /// A short sentence that describes this item.
        /// </summary>
        /// <returns></returns>
        public string GetDescription() => this.Description;
        /// <summary>
        /// What this item will cost in shops. Returns NULL if 
        /// this item cannot be sold
        /// </summary>
        /// <returns></returns>
        public float? GetPrice() => this.Saleable ? this.Price : (float?)null;
        /// <summary>
        /// Returns TRUE if this item can be sold.
        /// </summary>
        /// <returns></returns>
        public bool CanSell() => this.Saleable;
    }
}
