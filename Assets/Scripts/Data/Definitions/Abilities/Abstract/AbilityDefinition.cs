using UnityEngine;

namespace Game.Data
{
    public abstract class AbilityDefinition : ScriptableObject
    {
        [SerializeField] string displayName;
        [SerializeField, TextArea(2, 5)] string description;
        [SerializeField] Sprite icon;
        [SerializeField, Min(0f)] float cd;
        [SerializeField, Min(0)] int energyCost;
        // For the first template, stick with directional target mode first
        [SerializeField] AbilityTargetMode targetMode = AbilityTargetMode.Directional; 

        public string DisplayName => displayName;
        public string Description => description;
        public Sprite Icon => icon;
        public float CD => cd;
        public int EnergyCost => energyCost;
        public AbilityTargetMode TargetMode => targetMode;


    }
}