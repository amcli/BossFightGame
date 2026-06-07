using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "BossFight/Reward/Weapon", fileName = "NewWeaponReward", order = 300)]
    public class WeaponRewardDefinition : RewardDefinition
    {
        [Header("Weapon")]
        [SerializeField] WeaponDefinition weapon;

        public WeaponDefinition Weapon => weapon;
    }
}
