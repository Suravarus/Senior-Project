using UnityEngine;

namespace Combat
{
    /// <summary>
    /// Makes sure that weapons are always placed at an appropriate 
    /// distance from the WeaponWielder
    /// </summary>
    public class WeaponWrapper : MonoBehaviour
    {
        private IWeapon WrappedWeapon { get; set; }
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
                this.WrappedWeapon = this.transform.GetChild(0).GetComponent<IWeapon>();
            }
            // determine the offset
            this.YAxisOffset = this.WrappedWeapon.GetGameObject().transform.position.y 
                - this.transform.position.y;
        }

        public void Start()
        {
            this.CalibrateWeapon();
        }

        public void Update()
        {
            if (this.WrappedWeapon.RequiresFlip())
            {
                // DETERMINE if the weapon is facing left or right
                var target = this.WrappedWeapon
                    .GetGameObject().transform.position - this.transform.position;
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
            Combatant.RotateTo(new Vector2(this.transform.position.x, this.transform.position.y+1), this.transform);
            // position weapon
            this.WrappedWeapon.GetGameObject()
                .transform.position = this.InitialWeaponPosition;
            this.WrappedWeapon.GetGameObject()
                .transform.SetParent(this.transform);
            if (this.WrappedWeapon.IsFlipped())
                this.WrappedWeapon.Flip();
            this.WrappedWeapon.GetGameObject()
                .transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        public void WrapWeapon(IWeapon weapon)
        {
            this.WrappedWeapon = weapon;
            this.CalibrateWeapon();
        }
    }
}
