using UnityEngine;

namespace Combat
{
    /// <summary>
    /// Makes sure that weapons are always placed at an appropriate 
    /// distance from the WeaponWielder
    /// </summary>
    public class WeaponWrapper : MonoBehaviour
    {
        private Weapon WrappedWeapon { get; set; }
        private float YAxisOffset { get; set; }
        private Vector2 InitialWeaponPosition
        {
            get
            {
                return new Vector2(
                    this.transform.position.x, 
                    this.transform.position.y + this.YAxisOffset);
            }
        }
        public void Awake()
        {
            // get wrapped weapon
            if (this.WrappedWeapon == null)
            {
                this.WrappedWeapon = this.transform.GetChild(0).GetComponent<Weapon>();
            }
            // determine the offset
            this.YAxisOffset = this.WrappedWeapon.transform.position.y 
                - this.transform.position.y;
        }

        public void Start()
        {
            this.CalibrateWeapon();
        }

        public void Update()
        {
            if (this.WrappedWeapon.flipEnabled)
            {
                // DETERMINE if the weapon is facing left or right
                var target = this.WrappedWeapon.transform.position - this.transform.position;
                var weaponAngle = Vector2.SignedAngle(Vector2.up, target.normalized);
                // IF weapon facing left and has not been flipped
                if (weaponAngle > 0 && !this.WrappedWeapon.IsFlipped())
                {
                    // FLIP
                    this.WrappedWeapon.Flip();
                }
                // ELSE if weapon facing right and it has been flipped
                else if (weaponAngle < 0 && this.WrappedWeapon.IsFlipped())
                {
                    // FLIP it back
                    this.WrappedWeapon.Flip();
                }
            }
        }

        public void CalibrateWeapon()
        {
            // reset weapon wrapper rotation
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            // position weapon
            this.WrappedWeapon.transform.position = this.InitialWeaponPosition;
            this.WrappedWeapon.transform.SetParent(this.transform);
        }

        public void WrapWeapon(Weapon weapon)
        {
            this.WrappedWeapon = weapon;
            this.CalibrateWeapon();
        }
    }
}
