using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(
        menuName = "BossFight/Weapon/Melee", 
        fileName = "NewMeleeWeapon", 
        order = 100
    )]
    public class MeleeWeaponDefinition : WeaponDefinition
    {
        [Header("Hitbox")]
        [SerializeField] HitboxShape hitboxShape = HitboxShape.Box;
        [SerializeField] Vector2 hitboxSize = new Vector2(1f, 1f);
        [SerializeField] Vector2 hitboxOffset = Vector2.zero;

        public HitboxShape HitboxShape => hitboxShape;
        public Vector2 HitboxSize => hitboxSize;
        public Vector2 HitboxOffset => hitboxOffset;
    }
}
