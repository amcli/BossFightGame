using UnityEngine;
using Game.Data;

namespace Game.Combat
{
    // One damage event. Amount is already scaled by the attacker's offense (ATK/INT);
    // the defender applies its own DEF/RES reduction in Health.TakeDamage.
    public readonly struct DamageInfo
    {
        public readonly int Amount;
        public readonly DamageType Type;
        public readonly Vector2 KnockbackDir;   // direction to push the victim (need not be normalized)
        public readonly float KnockbackForce;   // 0 = no knockback
        public readonly Faction SourceFaction;  // who dealt it (for friendly-fire filtering)

        public DamageInfo(int amount, DamageType type, Vector2 knockbackDir, float knockbackForce, Faction sourceFaction)
        {
            Amount = amount;
            Type = type;
            KnockbackDir = knockbackDir;
            KnockbackForce = knockbackForce;
            SourceFaction = sourceFaction;
        }
    }
}
