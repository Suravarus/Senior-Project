using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Combat.UI
{

    public class HealthBar : MonoBehaviour
    {
        // Used to determine how to display text
        public enum TextStyle {
            Number,
            Percent,
            Hidden
        }

        // UNITY EDITOR PROPERTIES -----------------
        public TextStyle showTextAs;
        public Boolean destroyOnDeath = true;
        // -----------------------------------------

        // name of child objects expected by this script
        private enum ChildName
        {
            Background,
            Filler,
            Frame,
            Text
        }

        private int _currentHealth = 0;
        private int CurrentHealth
        {
            set
            {
                if (value > 0)
                    this._currentHealth = value;
                else
                    this._currentHealth = 0;
            }
            get
            {
                return this._currentHealth;
            }
        }
        private int _maxHealth = 0;
        private int MaxHealth
        {
            set
            {
                if (value > 0)
                    this._maxHealth = value;
                else
                    this._maxHealth = 0;
            }
            get
            {
                return this._maxHealth;
            }
        }

        private Boolean NeedsUpdate { set; get; }
        private Image Slider;
        private TextMeshProUGUI TextMeshProUGUI;
        // Check for gameobjects
        void Start()
        {
            // check for required child gameObjects
            foreach (string s in Enum.GetNames(typeof(ChildName))){
                if (this.transform.Find(s) == null)
                {
                    throw new NullReferenceException($"missing gameObject with name {s}");
                }
            }
            // set slider
            var slider = this.transform.Find(ChildName.Filler.ToString());
            this.Slider = slider.GetComponent<Image>();
            // set text mesh
            var text = this.transform.Find(ChildName.Text.ToString());
            this.TextMeshProUGUI = text.GetComponent<TextMeshProUGUI>();
        }

        void FixedUpdate()
        {
            if (this.NeedsUpdate)
            {
                this.UpdateBar();
                this.UpdateText();
                this.NeedsUpdate = false;
            }
        }

        public void UpdateValues(Combatant c)
        {
            if (c.IsAlive())
            {
                if (c.Health <= c.MaxHealth)
                {
                    this.CurrentHealth = c.Health;
                    this.MaxHealth = c.MaxHealth;
                    this.NeedsUpdate = true;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(this.CurrentHealth),
                        c.Health,
                        $"{nameof(this.CurrentHealth)} cannot be greater than {this.MaxHealth}. [{c.Health}, {c.MaxHealth}]");
                }
            } else
            {
                this.CurrentHealth = 0;
                this.MaxHealth = 1;
                this.NeedsUpdate = true;
            }
        }

        private void UpdateBar()
        {
            this.Slider.fillAmount = this.GetHealthAsDecimal();
        }
        private float GetHealthAsDecimal()
        {
            return (float) Math.Round((double)this.CurrentHealth / (double)this.MaxHealth, 2);
        }

        private void UpdateText()
        {
            switch (this.showTextAs)
            {
                case TextStyle.Number:
                    this.TextMeshProUGUI.SetText($"{this.CurrentHealth}/{this.MaxHealth}");
                    break;
                case TextStyle.Percent:
                    int pct = (int)Math.Round(this.GetHealthAsDecimal() * 100, 0);
                    this.TextMeshProUGUI.SetText($"{pct} %");
                    break;
                default:
                    this.TextMeshProUGUI.SetText("");
                    break;

            }
            if (this.CurrentHealth == 0)
            {
                if (this.destroyOnDeath)
                    Destroy(this.gameObject);
                else if (this.showTextAs != TextStyle.Hidden)
                    this.TextMeshProUGUI.SetText("DEAD");
            }
            
        }
    }
}
