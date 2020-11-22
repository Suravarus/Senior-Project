using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace UI
{
    public class Slot : MonoBehaviour, ISlot
    {
        // UNITY EDITOR -----------------------//
        public Image _icon;
        public TextMeshProUGUI _textMesh;
        public Image _indicatorPanel;
        public int _index = 0;
        public Color ActiveColor;
        public Color InactiveColor;
        // -----------------------------------//

        private Image IconImage { get; set; }
        private Image IndicatorPanel { get; set; }
        private TextMeshProUGUI TextMesh { get; set; }
        private int Index { get; set; }

        public bool ShowIcon { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public bool Active 
        {
            set
            {
                if (value == true)
                    this.IndicatorPanel.color = this.ActiveColor;
                else
                    this.IndicatorPanel.color = this.InactiveColor;
            }
            get { return this.IndicatorPanel.color == this.ActiveColor; }
        }

        public void Awake()
        {

            if (this._icon != null)
                this.IconImage = _icon;
            else
                throw new MissingComponentException(
                    $"{typeof(SpriteRenderer)} has not been set in {nameof(this._icon)} field.");

            if (this._textMesh != null)
                this.TextMesh = this._textMesh;
            else
                throw new MissingComponentException(
                    $"{typeof(SpriteRenderer)} has not been set in {nameof(this._textMesh)} field.");
            if (this._indicatorPanel != null)
                this.IndicatorPanel = this._indicatorPanel;
            else
                throw new MissingFieldException(nameof(this._indicatorPanel));
        }

        public int GetIndex()
        {
            return this.Index;
        }

        public void SetIcon(Sprite sprite, Vector2 scalling)
        {
            this.IconImage.gameObject.SetActive(true);
            this.IconImage.sprite = sprite;
            this.IconImage.gameObject.GetComponent<RectTransform>().localScale = scalling;
        }

        public void HideIcon()
        {
            this.IconImage.gameObject.SetActive(false);
        }

        public void SetIndex(int i)
        {
            this.Index = i;
        }

        public void SetText(string s)
        {
            this.TextMesh.SetText(s);
        }

        public void SetText(KeyCode k)
        {
            this.TextMesh.SetText(k.ToString().ToUpper());  
        }
    }
}
