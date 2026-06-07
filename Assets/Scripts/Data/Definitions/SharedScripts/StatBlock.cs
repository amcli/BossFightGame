using UnityEngine;

namespace Game.Data
{
    [System.Serializable]
    public struct StatBlock
    {
        [SerializeField, Min(1)] int maxHP;
        [SerializeField, Min(0)] int defense;
        [SerializeField, Min(0)] int resistance;
        [SerializeField, Min(0)] int attack;
        [SerializeField, Min(0)] int intelligence;
        [SerializeField, Min(0)] int moveSpeed;
        [SerializeField, Min(0)] int maxEnergy;

        public int MaxHP => maxHP;
        public int Defense => defense;
        public int Resistance => resistance;
        public int Attack => attack;
        public int Intelligence => intelligence;
        public int MoveSpeed => moveSpeed;
        public int MaxEnergy => maxEnergy;

        public int Get(StatType stat) => stat switch
        {
            StatType.MaxHP => maxHP,
            StatType.Defense => defense,
            StatType.Resistance => resistance,
            StatType.Attack => attack,
            StatType.Intelligence => intelligence,
            StatType.MoveSpeed => moveSpeed,
            StatType.MaxEnergy => maxEnergy,
            _ => 0,
        };
    }
}
