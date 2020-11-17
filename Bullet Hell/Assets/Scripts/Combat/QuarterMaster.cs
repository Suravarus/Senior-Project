using System;
using System.Collections.Generic;
using UnityEngine;

using Combat.UI;
namespace Combat
{
    /// <summary>
    /// Handles the arsenal of weapons for Combatant. 
    /// The Combatant will always be given the first weapon in the arsenal.
    /// Also communitcates with the AbilityBar if that component is attached.
    /// </summary>
    public class QuarterMaster
    {
        private WeaponWielder Wielder { get; set; }
        private int AssignedIndex { get; set; }
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

        private WeaponBarUI WeaponBar { get; set; }
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="combatant"></param>
        /// <param name="weapons"></param>
        public QuarterMaster(WeaponWielder wielder, 
            Weapon[] weapons, 
            WeaponWrapper weaponWrapper,
            WeaponBarUI abilityBar = null)
        {
            this.Wielder = wielder;
            this.Wrapper = weaponWrapper;
            this.Arsenal = new List<Weapon>(Combatant.MAX_WEAPONS);
            foreach (Weapon w in weapons)
            {
                if (w != null)
                {
                    w.wielder = this.Wielder;
                    this.Arsenal.Add(w);
                }
                    
            }

            while (this.Arsenal.Count < this.Arsenal.Capacity)
                this.Arsenal.Add(null);

            if (this.Arsenal.Count > 0)
            {
                this.AssignedIndex = 0;
                this.ActivateWeapon(this.GetAssignedWeapon());
            }

            if (abilityBar != null)
            {
                this.WeaponBar = abilityBar;
                this.WeaponBar.SetWeapons(this);
                try { this.WeaponBar.EquipWeaponAt(0); }
                catch(NullReferenceException n) { }
                // SET post-start function for WeaponBar in case
                // it still has not completed it's Start() method.
                this.WeaponBar.PostStart = (WeaponBarUI w) =>
                {
                    w.SetWeapons(this);
                    w.EquipWeaponAt(0);
                    // incase UISlot was not set before
                    this.GetAssignedWeapon().UIAmmoSlot = w.GetAmmoSlot();
                };
            }
        }

        public Boolean HasWeaponBar()
        {
            return this.WeaponBar != null;
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
                return this.Arsenal.Count >= Combatant.MAX_WEAPONS && !this.Arsenal.Contains(null);
            }    
        }
        
        public Weapon GetAssignedWeapon()
        {
            if (this.AssignedIndex > -1 && this.Arsenal.Count > 0)
                return this.Arsenal[this.AssignedIndex];

            return null;
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
                // IF Arsenal is full and wielder is not disarmed
                if (this.FullArsenal && !this.Wielder.Disarmed())
                {
                    this.GetAssignedWeapon().wielder = null;
                    DropWeaponAt(this.GetAssignedWeapon(), weapon.transform.position);
                    this.Arsenal[this.AssignedIndex] = weapon;
                    ActivateWeapon(weapon);

                } else 
                {
                    if (!this.Wielder.Disarmed()) // Arsenal not full and Wielder is not disarmed
                    {
                        var nullIndex = this.Arsenal.IndexOf(null);
                        DeactivateWeapon(this.GetAssignedWeapon());
                        SwapWeapon(this.AssignedIndex, nullIndex);
                        this.Arsenal[this.AssignedIndex] = weapon;
                        this.ActivateWeapon(weapon);
                    } else
                    {
                        this.Arsenal[this.AssignedIndex] = weapon;
                        this.ActivateWeapon(weapon);
                    }
                }

                if (!this.FullArsenal || !this.Wielder.Disarmed())
                {
                    weapon.wielder = this.Wielder;
                    this.WeaponBar.SetWeapons(this);
                }
            } else
            {
                throw new Exception(
                            $"Trying to pick up a weapon that is already in the {this.Wielder.GetType()}'s Arsenal.");
            }
        }

        private void DropWeaponAt(Weapon weapon, Vector2 position)
        {
            weapon.transform.SetParent(null);
            weapon.transform.position = position;
            weapon.UIAmmoSlot = null;
            weapon.GetComponent<CircleCollider2D>().enabled = true;
        }

        /// <summary>
        /// Set's the ranged weapon as the weapon pointed to by the index.
        /// </summary>
        private void ActivateWeapon(Weapon w)
        {
            w.gameObject.SetActive(true);
            this.Wrapper.WrapWeapon(w);
            this.Wrapper.CalibrateWeapon();

            if (this.HasWeaponBar())
            {
                w.UIAmmoSlot = this.WeaponBar.GetAmmoSlot();
            }
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

        private void DeactivateWeapon(Weapon w)
        {
            if (this.Arsenal[this.AssignedIndex] != null)
            {
                w.gameObject.SetActive(false);
                w.UIAmmoSlot = null;
            }
        }
        
        public void AssignWeaponAt(int i)
        {
            if (i != this.AssignedIndex)
            {
                if (i < Combatant.MAX_WEAPONS) 
                {
                    DeactivateWeapon(GetAssignedWeapon());
                    this.AssignedIndex = i;
                    if (this.Arsenal[i] != null)
                        ActivateWeapon(this.GetAssignedWeapon());
                }
                else
                {
                    throw new IndexOutOfRangeException(
                        $"Tried to assign weapon-index:{i} which is beyond capacity: {Combatant.MAX_WEAPONS}");
                }
            } 
            
        }
        public Weapon[] GetArsenal()
        {
            return this.Arsenal.ToArray();
        }
    }
}
