using UnityEngine;
using Game.Data;

namespace Game.Combat
{
    // Integer damage pipeline, split across attacker and defender:
    //   outgoing = floor(base * (1 + offense/100)), min 1   (offense = ATK for physical, INT for magical)
    //   final    = max(1, outgoing - defense)               (defense = DEF for physical, RES for magical)
    // True damage ignores both scaling and reduction.
    public static class DamageMath
    {
        public static int ScaleOutgoing(int baseDamage, DamageType type, int attack, int intelligence)
        {
            float scale = type switch
            {
                DamageType.Physical => 1f + attack / 100f,
                DamageType.Magical  => 1f + intelligence / 100f,
                _                   => 1f, // True
            };
            return Mathf.Max(1, Mathf.FloorToInt(baseDamage * scale));
        }

        public static int ApplyDefense(int amount, DamageType type, int defense, int resistance)
        {
            int reduced = type switch
            {
                DamageType.Physical => amount - defense,
                DamageType.Magical  => amount - resistance,
                _                   => amount, // True
            };
            return Mathf.Max(1, reduced);
        }
    }
}
