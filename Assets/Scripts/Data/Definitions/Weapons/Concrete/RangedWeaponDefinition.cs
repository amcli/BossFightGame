using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(
        menuName = "BossFight/Weapon/Ranged",
        fileName = "NewRangedWeapon",
        order = 101
    )]
    public class RangedWeaponDefinition : WeaponDefinition
    {
        [Header("Projectile")]
        [SerializeField] GameObject projectilePrefab;
        [SerializeField, Min(0.1f)] float projectileSpeed = 10f;
        [SerializeField, Min(0.1f)] float projectileLifetime = 3f;

        public GameObject ProjectilePrefab => projectilePrefab;
        public float ProjectileSpeed => projectileSpeed;
        public float ProjectileLifetime => projectileLifetime;
   }
}
