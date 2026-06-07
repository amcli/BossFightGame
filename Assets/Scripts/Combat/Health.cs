using System;
using UnityEngine;

namespace Game.Combat
{
    // HP + defensive stats for any combatant. Applies DEF/RES reduction, knockback, and raises
    // local events for UI. Configure() is called by the spawner to inject stats from a definition.
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] Faction faction = Faction.Enemy;
        [SerializeField] int maxHP = 100;
        [SerializeField] int defense;
        [SerializeField] int resistance;

        int currentHP;
        Rigidbody2D body;

        public Faction Faction => faction;
        public int MaxHP => maxHP;
        public int CurrentHP => currentHP;
        public bool IsDead => currentHP <= 0;

        public event Action<int, int> HealthChanged; // (current, max)
        public event Action Died;

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            if (currentHP <= 0) currentHP = maxHP; // not configured -> use serialized default
        }

        public void Configure(Faction f, int maxHp, int def, int res)
        {
            faction = f;
            maxHP = Mathf.Max(1, maxHp);
            defense = Mathf.Max(0, def);
            resistance = Mathf.Max(0, res);
            currentHP = maxHP;
            HealthChanged?.Invoke(currentHP, maxHP);
        }

        public void TakeDamage(in DamageInfo info)
        {
            if (IsDead) return;
            if (info.SourceFaction == faction) return; // no friendly fire

            int dmg = DamageMath.ApplyDefense(info.Amount, info.Type, defense, resistance);
            currentHP = Mathf.Max(0, currentHP - dmg);

            if (info.KnockbackForce > 0f && body != null)
                body.AddForce(info.KnockbackDir.normalized * info.KnockbackForce, ForceMode2D.Impulse);

            HealthChanged?.Invoke(currentHP, maxHP);
            if (currentHP <= 0) Died?.Invoke();
        }

        public void Heal(int amount)
        {
            if (IsDead || amount <= 0) return;
            currentHP = Mathf.Min(maxHP, currentHP + amount);
            HealthChanged?.Invoke(currentHP, maxHP);
        }
    }
}
