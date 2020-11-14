using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    /// <summary>
    /// Handles the arsenal of weapons for Combatant. 
    /// The Combatant will always be given the first weapon in the arsenal.
    /// </summary>
    public class QuarterMaster
    {
        private WeaponWielder Wielder { get; set; }
        private int EquippedIndex { get { return 0; } }
        private List<Weapon> _arsenal;
        /// <summary>
        /// List of weapons the Combatant can use at any time.
        /// </summary>
        private List<Weapon> Arsenal
        {
            set
            {
                if (value.Capacity <= Combatant.MAX_WEAPONS)
                {
                    this._arsenal = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(this.Arsenal),
                        value.Count,
                        $"count must be <= {Combatant.MAX_WEAPONS}");
                }
            }
            get
            {
                if (this._arsenal.Capacity == Combatant.MAX_WEAPONS)
                {
                    return this._arsenal;
                } else
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(this.Arsenal),
                        this._arsenal.Count,
                        $"count must be <= {Combatant.MAX_WEAPONS}");
                }
            }
        }
        private WeaponWrapper Wrapper { get; set; }

       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="combatant"></param>
        /// <param name="weapons"></param>
        public QuarterMaster(WeaponWielder wielder, Weapon[] weapons, WeaponWrapper weaponWrapper)
        {
            this.Wielder = wielder;
            this.Wrapper = weaponWrapper;
            this.Arsenal = new List<Weapon>(Combatant.MAX_WEAPONS);

            foreach (Weapon w in weapons)
            {
                if (w != null)
                {
                    AddToArsenal(w);
                }
                    
            }

            this.ActivateFirstWeapon();
        }

        public WeaponWrapper GetWeaponWrapper() 
        {
            return this.Wrapper;
        }

        /// <summary>
        /// Returns TRUE if there is no space for additional weapons.
        /// </summary>
        private Boolean FullArsenal 
        {
            get
            {
                return this.Arsenal.Count == Combatant.MAX_WEAPONS;
            }    
        }
        
        public Weapon FirstWeapon()
        {
            return this.Arsenal[this.EquippedIndex];
        }
        /// <summary>
        /// Adds the given weapon to inventory and equips it.
        /// If player has reached weapon limit, the current weapon is destroyed
        /// and replaced by the given weapon.
        /// </summary>
        /// <param name="weapon"></param>
        public void PickupWeapon(Weapon weapon)
        {
            if (!this.Arsenal.Contains(weapon))
            {
                if (this.FullArsenal)
                {
                    DropEquipped(weapon.transform.position);
                } else
                {
                    DeactivateFirstWeapon();
                }
                this.Arsenal.Add(weapon);
                SwapWeapon(this.EquippedIndex, this.Arsenal.Count - 1);
                ActivateFirstWeapon();
            } else
            {
                throw new Exception(
                            $"Trying to pick up a weapon that is already in the {this.Wielder.GetType()}'s Arsenal.");
            }
        }

        private void AddToArsenal(Weapon w)
        {
            w.wielder = this.Wielder;
            this.Arsenal.Add(w);
        }

        private void DropEquipped(Vector2 position)
        {
            this.FirstWeapon().wielder = null;
            this.FirstWeapon().transform.position = position;
            this.Arsenal.RemoveAt(this.EquippedIndex);
        }

        /// <summary>
        /// Set's the ranged weapon as the weapon pointed to by the index.
        /// </summary>
        private void ActivateFirstWeapon()
        {
            this.FirstWeapon().gameObject.SetActive(true);
            this.Wrapper.WrapWeapon(this.FirstWeapon());
            this.Wrapper.CalibrateWeapon();
        }

        private void SwapWeapon(int a, int b)
        {
            if (a != b)
            {
                var weaponA = this.Arsenal[a];

                this.Arsenal[a] = this.Arsenal[b];
                this.Arsenal[b] = weaponA;
            }
        }

        /// <summary>
        /// Deactivates RangedWeapon and set's it to null.
        /// Return false if the combatant was disarmed.
        /// </summary>
        /// <returns></returns>
        private void DeactivateFirstWeapon()
        {
            this.Arsenal[this.EquippedIndex].gameObject.SetActive(false);
        }
    }
}
