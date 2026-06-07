using UnityEngine;

namespace Game.Data
{
    // Base for all post-boss rewards. Abstract -> no [CreateAssetMenu]; only concretes are creatable.
    public abstract class RewardDefinition : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] string displayName;
        [SerializeField, TextArea(2, 5)] string description;
        [SerializeField] Sprite icon;
        [SerializeField] ItemRarity rarity = ItemRarity.Common;

        public string DisplayName => displayName;
        public string Description => description;
        public Sprite Icon => icon;
        public ItemRarity Rarity => rarity;
    }
}
