using System;
using UnityEngine;

using UI;
namespace Testing
{
    /// <summary>
    /// Adds 1 to the CurrencyUI everytime you press 'E'.
    /// </summary>
    class TestCurrencyUI : MonoBehaviour
    {
        public CurrencyUI CurrencyUI;
        public int startingAmount = 0;
        private bool started = false;

        public void Start()
        {
            if (this.CurrencyUI == null)
            {
                throw new NullReferenceException(
                    $"Please set {nameof(this.CurrencyUI)} or remove {this.GetType()} from gameObject: {this.gameObject.name}");
            }
        }
        void Update()
        {
            if (!started)
            {
                this.CurrencyUI.SetAmount(this.startingAmount);
                this.started = true;
            }
                
            if (Input.GetKeyDown(KeyCode.E))
            {
                this.CurrencyUI.SetAmount(this.CurrencyUI.GetAmount() + 1);
            }
            if (Input.GetKeyDown(KeyCode.Q))
                this.CurrencyUI.SetAmount(this.CurrencyUI.GetAmount() - 1);
        }
    }
}
