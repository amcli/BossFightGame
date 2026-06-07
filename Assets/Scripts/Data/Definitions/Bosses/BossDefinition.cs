using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    // Blueprint for a boss. Bare-bones v1: stats + one simple ranged attack + contact damage + drops.
    // Attack behavior is intentionally inline (a few tunable fields) for the first playable. When we
    // add more bosses with distinct patterns, extract a polymorphic BossAttackDefinition (abstract +
    // concretes) the same way abilities/weapons work, and replace these fields with an attack list.
    [CreateAssetMenu(menuName = "BossFight/Boss/Boss", fileName = "NewBoss", order = 400)]
    public class BossDefinition : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] string bossId;                 // stable string id, matches MetaProgression ids
        [SerializeField] string displayName;
        [SerializeField, TextArea(2, 5)] string description;
        [SerializeField] Sprite icon;

        [Header("Visuals")]
        [SerializeField] Sprite bodySprite;             // sprite the boss prefab renders

        [Header("Base Stats")]
        [SerializeField] StatBlock baseStats;           // reuses the shared stat block (HP/DEF/RES/...)

        [Header("Combat")]
        [SerializeField, Min(0)] int contactDamage = 5;        // damage when the boss touches the player
        [SerializeField, Min(0)] int attackDamage = 10;        // damage per projectile
        [SerializeField, Min(0.1f)] float attackInterval = 2f; // seconds between shots
        [SerializeField, Min(0.1f)] float projectileSpeed = 6f;
        [SerializeField, Min(0.1f)] float projectileLifetime = 4f;

        [Header("Rewards (this boss's drops)")]
        [SerializeField] RewardDefinition[] rewards;

        public string BossId => bossId;
        public string DisplayName => displayName;
        public string Description => description;
        public Sprite Icon => icon;
        public Sprite BodySprite => bodySprite;
        public StatBlock BaseStats => baseStats;
        public int ContactDamage => contactDamage;
        public int AttackDamage => attackDamage;
        public float AttackInterval => attackInterval;
        public float ProjectileSpeed => projectileSpeed;
        public float ProjectileLifetime => projectileLifetime;
        public IReadOnlyList<RewardDefinition> Rewards => rewards;
    }
}
