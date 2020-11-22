
namespace Combat.UI
{
    public interface IWeaponBarUI
    {
        void SetWeapons(QuarterMaster quarterMaster);
        void SetAmmoCount(Weapon weapon);

        int WeaponSlotCount();
    }
}
