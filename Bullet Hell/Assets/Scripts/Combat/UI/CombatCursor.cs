using System;
using UnityEngine;

namespace Combat.UI
{
    /// <summary>
    /// Changes the mouse cursor based on wether it's position is within
    /// the player's weapon-range.
    /// <list type="bullet">
    /// <item>This should only be added to the Player character</item>
    /// <item>Requires PlayerMovement Component</item>
    /// <item>Requires Combatant Component</item>
    /// </list>
    /// </summary>
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(Combatant))]
    class CombatCursor : MonoBehaviour
    {
        // UnityEditor Properties -----------------------//
        public Texture2D inRangeCursor;
        public Texture2D outOfRangeCursor;
        // ---------------------------------------------//

        private Combatant playerCombatant;
        private Boolean inRange = false;
        void Start()
        {
            this.playerCombatant = this.GetComponent<Combatant>();
            Cursor.SetCursor(this.outOfRangeCursor, Vector2.zero, CursorMode.Auto);
        }
        void Update()
        {
            var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPosition.z = 1;
            if (this.playerCombatant != null)
            {
                if (this.playerCombatant.RangedWeapon.InRange(cursorPosition))
                {
                    if (!this.inRange)
                    {
                        this.inRange = true;
                        Cursor.SetCursor(this.inRangeCursor, Vector2.zero, CursorMode.Auto);
                    }
                        
                } else if (this.inRange)
                {
                    this.inRange = false;
                    Cursor.SetCursor(this.outOfRangeCursor, Vector2.zero, CursorMode.Auto);
                }
            }
        }
    }
}
