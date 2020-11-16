﻿using UnityEngine;
using System;

using UI;

namespace Combat.UI
{
    public class WeaponBarUI : MonoBehaviour, IWeaponBarUI
    {
        // UNITY EDITOR ----------//
        public Slot[] _weaponSlots;
        public Slot _ammoSlot;
        // ----------------------//
        private Slot[] WeaponSlots { get; set; }
        private Slot AmmoSlot { get; set; }
        

        public void Awake()
        {
            this.WeaponSlots = this._weaponSlots;
            this.AmmoSlot = this._ammoSlot;
        }

        public void Start()
        {
            
            for (int i = 0; i < this.WeaponSlots.Length; i++)
            {
                this.WeaponSlots[i].SetIndex(i);
            }
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
                    }
                }
            }
        }

        public void SetAmmoCount(Weapon weapon)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Returns the amount of weapon slots available.
        /// </summary>
        /// <returns></returns>
        public int WeaponSlotCount()
        {
            return this.WeaponSlots.Length;
        }
    }
}
