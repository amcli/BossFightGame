namespace Game.Combat
{
    // Anything that can be hit. Implemented by Health.
    public interface IDamageable
    {
        Faction Faction { get; }
        bool IsDead { get; }
        void TakeDamage(in DamageInfo info);
    }
}
