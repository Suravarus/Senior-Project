using System;
using System.Collections.Generic;
using UnityEngine;

using Combat.UI;
using Combat.Varia;
using Loot;
namespace Combat
{
    /// <summary>
    /// Handles the arsenal of weapons for WeaponWielder. 
    /// Also communitcates with the WeaponBarUI component if
    /// it is set via QuarterMaster.WeaponBar.
    /// </summary>
    public class QuarterMaster : IQuarterMaster
    {
        private WeaponWielder Wielder { get; set; }
        private int AssignedIndex { get; set; }
        private List<IWeapon> _arsenal;
        /// <summary>
        /// List of weapons the Combatant can use at any time.
        /// </summary>
        private List<IWeapon> Arsenal
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
                }
                else
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
        /// <param name="wielder"></param>
        /// <param name="weapons">MAX 3</param>
        public QuarterMaster(WeaponWielder wielder,
            IWeapon[] weapons,
            WeaponWrapper weaponWrapper,
            WeaponBarUI abilityBar = null)
        {
            this.Wielder = wielder;
            this.Wrapper = weaponWrapper;
            this.Arsenal = new List<IWeapon>(Combatant.MAX_WEAPONS);
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
                this.WeaponBar.Wielder = this.Wielder;
                this.WeaponBar.SetWeapons(this);
                // SET post-start function for WeaponBar in case
                // it still has not completed it's Start() method.
                this.WeaponBar.PostStart.Add((WeaponBarUI w) =>
                {
                    w.Wielder = this.Wielder;
                    w.SetWeapons(this);
                    // incase UISlot was not set before
                    this.GetAssignedWeapon().UIAmmoSlot = w.GetAmmoSlot();
                });
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

        public IWeapon GetAssignedWeapon()
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
        public void PickupWeapon(IWeapon weapon)
        {
            if (!this.Arsenal.Contains(weapon))
            {
                // IF Arsenal is full and wielder is not disarmed
                if (this.FullArsenal && !this.Wielder.Disarmed())
                {
                    this.GetAssignedWeapon().Wielder = null;
                    DropWeaponAt(this.GetAssignedWeapon(), weapon.GetGameObject().transform.position);
                    this.Arsenal[this.AssignedIndex] = weapon;
                    ActivateWeapon(weapon);

                }
                else
                {
                    if (!this.Wielder.Disarmed()) // Arsenal not full and Wielder is not disarmed
                    {
                        var nullIndex = this.Arsenal.IndexOf(null);
                        DeactivateWeapon(this.GetAssignedWeapon());
                        SwapWeapon(this.AssignedIndex, nullIndex);
                        this.Arsenal[this.AssignedIndex] = weapon;
                        this.ActivateWeapon(weapon);
                    }
                    else
                    {
                        this.Arsenal[this.AssignedIndex] = weapon;
                        this.ActivateWeapon(weapon);
                    }
                }

                if (!this.FullArsenal || !this.Wielder.Disarmed())
                {
                    weapon.Wielder = this.Wielder;
                    this.WeaponBar.SetWeapons(this);
                    if (weapon.GetSpeaker() != null)
                        weapon.GetSpeaker().PlaySound(Audio.WeaponSounds.Sounds.Pickup);
                }
            }
            else
            {
                throw new Exception(
                            $"Trying to pick up a weapon that is already in the {this.Wielder.GetType()}'s Arsenal.");
            }
        }

        private void DropWeaponAt(IWeapon weapon, Vector2 position)
        {
            var variaWeapon = weapon.GetGameObject().GetComponent<VWeapon>();
               
            if (weapon.GetSpeaker() != null)
                weapon.GetSpeaker().PlaySound(Audio.WeaponSounds.Sounds.Drop);

            if (variaWeapon == null)
            {
                weapon.GetGameObject().transform.SetParent(null);
                weapon.GetGameObject().transform.position = position;
            }

            else
            {
                weapon.GetGunBarrel().transform.SetParent(null);
                weapon.GetGunBarrel().transform.position = position;
            }
                
            
            weapon.UIAmmoSlot = null;

            var collider = weapon.GetGameObject().GetComponent<Collider2D>();
            collider.enabled = true;
        }

        /// <summary>
        /// Set's the ranged weapon as the weapon pointed to by the index.
        /// </summary>
        private void ActivateWeapon(IWeapon w)
        {
            var lootRadius = w.GetGameObject().GetComponent<Collider2D>();
            if (lootRadius != null)
                lootRadius.enabled = false;
            else
                Debug.LogWarning($"weapon {w.GetGameObject().name} should have a lootRadius collider");
            w.GetGameObject().SetActive(true);
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

        private void DeactivateWeapon(IWeapon w)
        {
            if (this.Arsenal[this.AssignedIndex] != null)
            {
                w.GetGameObject().SetActive(false);
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
        public IWeapon[] GetArsenal()
        {
            return this.Arsenal.ToArray();
        }
    }
}
