using UnityEngine;
using System;

using UI;

namespace Combat.UI
{
    public class WeaponBarUI : MonoBehaviour, IWeaponBarUI
    {
        // UNITY EDITOR ----------//
        public Slot[] _weaponSlots;
        public Slot _ammoSlot;
        public WeaponWielder _weaponWielder;
        // ----------------------//
        private Slot[] WeaponSlots { get; set; }
        private Slot AmmoSlot { get; set; }
        private WeaponWielder Wielder { get; set; }
        /// <summary>
        /// If set, this delegate will be called at the end of 
        /// WeaponBarUI.Start().
        /// </summary>
        public delegate void PostStartFunction(WeaponBarUI w);
        public PostStartFunction PostStart;
        

        public void Awake()
        {
            this.WeaponSlots = this._weaponSlots;
            this.AmmoSlot = this._ammoSlot;
            this.Wielder = this._weaponWielder;
        }

        public void Start()
        {
            
            for (int i = 0; i < this.WeaponSlots.Length; i++)
            {
                var slot = this.WeaponSlots[i];
                slot.HideIcon();
                slot.SetIndex(i);
                slot.SetText((i + 1).ToString());
            }

            this.PostStart?.Invoke(this);
        }

        public Slot GetAmmoSlot()
        {
            return this.AmmoSlot;
        }

        public void SetWeapons(QuarterMaster quarterMaster)
        {
            var arsenal = quarterMaster.GetArsenal();
            if (arsenal.Length <= this.WeaponSlotCount())
            {
                for (int i = 0; i < arsenal.Length; i++)
                {
                    if (arsenal[i] != null)
                    {
                        var s = this.WeaponSlots[i];
                        var weaponRenderer = arsenal[i].gameObject.GetComponent<SpriteRenderer>();
                        s.SetIcon(weaponRenderer.sprite, weaponRenderer.transform.lossyScale);
                        s.SetIndex(i);
                        s.SetText((i + 1).ToString());
                        Debug.Log($"set-text: {(i + 1).ToString()}");
                    }
                }
            }
        }

        public void SetAmmoCount(Weapon weapon)
        {
            weapon.UIAmmoSlot = this.GetAmmoSlot();
        }

        /// <summary>
        /// Returns the amount of weapon slots available.
        /// </summary>
        /// <returns></returns>
        public int WeaponSlotCount()
        {
            return this.WeaponSlots.Length;
        }

        public void EquipWeaponAt(int i)
        {
            this.Wielder.GetQuarterMaster().AssignWeaponAt(i);
            this.SetActive(i);
        }

        private void SetActive(int index)
        {
            for (int i = 0; i < this.WeaponSlots.Length; i++)
            {
                var s = this.WeaponSlots[i];
                if (i != index) s.Active = false;
                else s.Active = true;
            }
        }
    }
}
