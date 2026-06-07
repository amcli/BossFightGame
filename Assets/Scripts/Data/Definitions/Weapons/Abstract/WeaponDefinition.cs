using UnityEngine;

namespace Game.Data
{
    public abstract class WeaponDefinition : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] string displayName;
        [SerializeField, TextArea(2, 5)] string description;
        [SerializeField] Sprite icon;
        // This is a placeholder, does not have to be common
        [SerializeField] ItemRarity rarity = ItemRarity.Common;

        [Header("Damage")]
        [SerializeField, Min(1)] int damage = 1;
        [SerializeField] DamageType damageType = DamageType.Physical;
        [SerializeField, Min(0.01f)] float fireRate = 1f;

        [Header("Impact")]
        [SerializeField, Min(0f)] float knockbackForce;

        [Header("Presentation")]
        [SerializeField] GameObject attackVfx;
        [SerializeField] AudioClip attackSfx;

        [Header("Optional Ability")]
        [SerializeField] AbilityDefinition uniqueAbility;

        public string DisplayName => displayName;
        public string Description => description;
        public Sprite Icon => icon;
        public ItemRarity Rarity => rarity;
        public int Damage => damage;
        public DamageType DamageType => damageType;
        public float FireRate => fireRate;
        public float KnockbackForce => knockbackForce;
        public GameObject AttackVfx => attackVfx;
        public AudioClip AttackSfx => attackSfx;
        public AbilityDefinition UniqueAbility => uniqueAbility;
    }
}
