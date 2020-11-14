namespace Combat
{
    public interface IWeaponWielder
    {
        int RangedDamage { get; }
        Weapon RangedWeapon { get; }

        void Awake();
        void Die();
        bool Disarmed();
        QuarterMaster GetQuarterMaster();
        WeaponWrapper GetWeaponWrapper();
        bool ShootWeapon();
        void Start();
        void Update();
        void OnAmmoCollision(int id);
    }
}