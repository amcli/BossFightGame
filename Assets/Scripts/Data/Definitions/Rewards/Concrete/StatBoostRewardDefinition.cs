using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "BossFight/Reward/StatBoost", fileName = "NewStatBoostReward", order = 302)]
    public class StatBoostRewardDefinition : RewardDefinition
    {
        [Header("Stat Boost")]
        [SerializeField] StatType stat = StatType.Attack;
        [SerializeField, Min(1)] int amount = 1;

        public StatType Stat => stat;
        public int Amount => amount;
    }
}
