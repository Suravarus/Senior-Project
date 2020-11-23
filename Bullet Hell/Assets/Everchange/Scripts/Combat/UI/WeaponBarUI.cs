using System;
using UnityEngine;
using System.Collections.Generic;

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
        public WeaponWielder Wielder { get; set; }
        /// <summary>
        /// If set, this delegate will be called at the end of 
        /// WeaponBarUI.Start().
        /// </summary>
        public delegate void PostStartFunction(WeaponBarUI w);
        public List<PostStartFunction> PostStart = new List<PostStartFunction>();
        

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
            }

            this.SetActive(0);

            foreach (PostStartFunction f in this.PostStart)
                f.Invoke(this);

            if (this.WeaponSlots == null)
                throw new MissingFieldException(nameof(this._weaponSlots));
            if (this.AmmoSlot == null)
                throw new MissingFieldException(nameof(this._ammoSlot));
            if (this.Wielder == null)
                throw new MissingFieldException(nameof(this._weaponWielder));
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
                        var weaponRenderer = arsenal[i].GetGameObject().GetComponent<SpriteRenderer>();
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
            if (this.Wielder.Disarmed())
                this.AmmoSlot.SetText("0");
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

        public void SetKeyBinds(string[] binds)
        {
            for (int i = 0; i < binds.Length; i++)
                this.WeaponSlots[i].SetText(binds[i]);
        }
    }
}
