using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(
        fileName = "NewCharacterClass", 
        menuName = "BossFight/Character/Class",
        order = 200
    )]
    public class CharacterClassDefinition : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] string displayName;
        [SerializeField, TextArea(2, 5)] string description;
        [SerializeField] Sprite icon;

        [Header("Base Stats")]
        [SerializeField] StatBlock baseStats;

        [Header("Energy")]
        [SerializeField] EnergyTrigger[] energyTriggers;

        [Header("Abilities")]
        [SerializeField] AbilityDefinition basicAbility;
        [SerializeField] AbilityDefinition ultimateAbility;

        public string DisplayName => displayName;
        public string Description => description;
        public Sprite Icon => icon;

        public StatBlock BaseStats => baseStats;
        public IReadOnlyList<EnergyTrigger> EnergyTriggers => energyTriggers;

        public AbilityDefinition BasicAbility => basicAbility;
        public AbilityDefinition UltimateAbility => ultimateAbility;

    }
}
