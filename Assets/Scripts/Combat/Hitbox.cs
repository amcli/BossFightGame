using UnityEngine;

namespace Game.Combat
{
    // Stateless melee helper: overlap a box and damage every opposing IDamageable inside it.
    // (Uses OverlapBoxAll for simplicity/compatibility; swap to a NonAlloc buffer later if it shows up in profiling.)
    public static class Hitbox
    {
        public static int OverlapDamage(Vector2 center, Vector2 size, float angleDeg, in DamageInfo info)
        {
            var hits = Physics2D.OverlapBoxAll(center, size, angleDeg);
            int dealt = 0;
            foreach (var c in hits)
            {
                if (c == null) continue;
                var target = c.GetComponentInParent<IDamageable>();
                if (target == null || target.Faction == info.SourceFaction || target.IsDead) continue;
                target.TakeDamage(info);
                dealt++;
            }
            return dealt;
        }
    }
}
