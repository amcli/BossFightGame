using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "BossFight/Reward/Currency", fileName = "NewCurrencyReward", order = 301)]
    public class CurrencyRewardDefinition : RewardDefinition
    {
        [Header("Currency")]
        [SerializeField, Min(1)] int amount = 1;

        public int Amount => amount;
    }
}
