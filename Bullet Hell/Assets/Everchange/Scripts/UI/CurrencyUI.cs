using UnityEngine;
using TMPro;

namespace UI
{
    public class CurrencyUI : MonoBehaviour
    {
        // Unity Editor --------------------------//
        /// <summary>
        /// UNITY-EDITOR-USE ONLY
        /// </summary>
        public int _iconWidth = 32;
        /// <summary>
        /// UNITY-EDITOR-USE ONLY
        /// </summary>
        public int _charWidth = 16;
        public TextMeshProUGUI CurrencyText;
        // --------------------------------------//
        private int IconWidth { get; set; }
        private int CharacterWidth { get; set; }
        private int Digits { get; set; }
        private int Amount { get; set; }
        private bool NeedsUpdate { get; set; }
        private RectTransform RectTransform { get; set; }

        public void Awake()
        {
            this.IconWidth = this._iconWidth;
            this.CharacterWidth = this._charWidth;
            this.RectTransform = this.GetComponent<RectTransform>();
        }

        public int GetAmount()
        {
            return this.Amount;
        }
        public void SetAmount(int amount)
        {
            this.Amount = amount;
            this.Digits = 1;
            float result = this.Amount / 10f;

            while(result >= 1)
            {
                this.Digits += 1;
                result /= 10f;
            }
            UpdateUI();
        }

        private void UpdateUI()
        {
            // SET width based on amount of characters
            var textWidth = this.CharacterWidth * this.Digits;
            this.RectTransform.sizeDelta = new Vector2(
                (float)(textWidth + this.IconWidth),
                this.RectTransform.rect.height);
            // SET ui text
            this.CurrencyText.SetText(this.Amount.ToString());
        }
    }
}
