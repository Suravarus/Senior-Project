using System;

namespace Combat.Utilities
{
    /// <summary>
    /// Contains information that the player will see about
    /// the parent object.
    /// </summary>
    public class GameInfo
    {
        public static readonly int DESCRIPTION_CHAR_LIMIT = 50;
        public static readonly int NAME_CHAR_LIMIT = 20;
        private String _lowercaseName; // ex. Thunder Arrow
        private String _description;
        
        /// <summary>
        /// The lowercase name of the gameObject.
        /// </summary>
        public String LowercaseName
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

        /// <summary>
        /// Description of this item. Should be a short sentence that 
        /// adheres to the character-limit.
        /// </summary>
        public String Description
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

        public GameInfo(String name, String description)
        {
            this.LowercaseName = name;
            this.Description = description;
        }
    }
}
