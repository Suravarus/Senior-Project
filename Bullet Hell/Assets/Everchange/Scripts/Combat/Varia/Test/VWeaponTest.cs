using UnityEngine;
using Input;

namespace Combat.Varia.Test
{
    public class VWeaponTest : MonoBehaviour
    {
        GameControls Controls;
        bool shootPressed = false;
        private void Awake()
        {
            this.Controls = new GameControls();
        }
        private void Start()
        {
            this.Controls.Combat.Shoot.performed += ctx => 
            {
                this.shootPressed = ctx.ReadValueAsButton();
            };
        }

        private void Update()
        {
            if (shootPressed)
            {
                this.GetComponent<VWeapon>().RequestWeaponFire();
            }
        }

        private void OnEnable()
        {
            this.Controls.Enable();
        }
        private void OnDisable()
        {
            if (this.Controls != null) this.Controls.Disable();
        }
    }
}
