#region Script Synopsis
    //Interface used to represent an object that can apply damage/has a damage rating.
    //Example: ShotBase.DMG referenced by ShotCollisionDamage.OnCollisionEnter2D()
#endregion

namespace ND_VariaBULLET
{
    public interface IDamager
    {
        float DMG { get; }
    }
}