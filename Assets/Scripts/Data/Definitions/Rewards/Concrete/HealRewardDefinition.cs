using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "BossFight/Reward/Heal", fileName = "NewHealReward", order = 303)]
    public class HealRewardDefinition : RewardDefinition
    {
        [Header("Heal")]
        [SerializeField, Min(1)] int amount = 1;
        [SerializeField] bool percentOfMaxHP;

        public int Amount => amount;
        public bool PercentOfMaxHP => percentOfMaxHP;
    }
}
