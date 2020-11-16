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
        public int _index = 0;
        // -----------------------------------//

        private Image IconImage { get; set; }
        private TextMeshProUGUI TextMesh { get; set; }
        private int _pIndex = -1;
        private int Index { get; set; }

        public bool ShowIcon { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public bool Active { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
        }

        public int GetIndex()
        {
            return this.Index;
        }

        public void SetIcon(Sprite sprite, Vector2 scalling)
        {
            if (sprite != null)
                this.IconImage.sprite = sprite;
            this.IconImage.gameObject.GetComponent<RectTransform>().localScale = scalling;
        }

        public void SetIndex(int i)
        {
            this.Index = i;
            this.TextMesh.SetText(this.Index.ToString());
        }
    }
}
