using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(
        menuName = "BossFight/Ability/Damage",
        fileName = "NewDamageAbility",
        order = 100
    )]
    public class DamageAbilityDefinition : AbilityDefinition
    {
        [Header("Damage")]
        [SerializeField, Min(1)] int damage = 1;
        [SerializeField] DamageType damageType = DamageType.Physical;
        [Header("Hitbox")]
        [SerializeField] HitboxShape hitboxShape = HitboxShape.Box;
        [SerializeField] Vector2 hitboxSize = new Vector2(1f, 1f);
        [SerializeField] Vector2 hitboxOffset = Vector2.zero;
        [Header("Impact")]
        [SerializeField, Min(0f)] float kbForce;
        [Header("Presentation")]
        [SerializeField] GameObject vfxPrefab;
        [SerializeField] AudioClip sfxClip;

        public int Damage => damage;
        public DamageType DamageType => damageType;
        public HitboxShape HitboxShape => hitboxShape;
        public Vector2 HitboxSize => hitboxSize;
        public Vector2 HitboxOffset => hitboxOffset;
        public float KBForce => kbForce;
        public GameObject VfxPrefab => vfxPrefab;
        public AudioClip SfxClip => sfxClip;
    }
}