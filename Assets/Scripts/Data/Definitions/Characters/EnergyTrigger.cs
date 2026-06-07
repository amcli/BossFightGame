using UnityEngine;

namespace Game.Data
{
    [System.Serializable]
    public struct EnergyTrigger
    {
        [SerializeField] EnergyGainMode gainMode;
        [SerializeField, Min(1)] int triggersToFill;

        public EnergyGainMode GainMode => gainMode;
        public int TriggersToFill => triggersToFill;
    }
}
