using UnityEngine;
using Game.Data;
using Game.Combat;

namespace Game.Bosses
{
    // Bare-bones boss: chases the player, fires a projectile every attackInterval, and deals
    // contact damage on touch. Stats/behavior come from a BossDefinition via Configure().
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Health))]
    public sealed class BossController : MonoBehaviour
    {
        [SerializeField] Projectile projectilePrefab;
        [SerializeField] float baseChaseSpeed = 1.6f;

        Health health;
        Rigidbody2D body;
        Transform target;

        int attack, intelligence;
        int contactDamage, attackDamage;
        float attackInterval = 2f;
        float projectileSpeed = 6f;
        float projectileLifetime = 4f;
        float moveSpeed;

        float fireTimer;
        float contactCooldown;

        public Health Health => health;

        void Awake()
        {
            health = GetComponent<Health>();
            body = GetComponent<Rigidbody2D>();
        }

        public void Configure(BossDefinition def, Transform player)
        {
            target = player;
            var s = def.BaseStats;
            attack = s.Attack;
            intelligence = s.Intelligence;
            moveSpeed = baseChaseSpeed * (1f + s.MoveSpeed / 100f);
            health.Configure(Faction.Enemy, s.MaxHP, s.Defense, s.Resistance);

            contactDamage = def.ContactDamage;
            attackDamage = def.AttackDamage;
            attackInterval = def.AttackInterval;
            projectileSpeed = def.ProjectileSpeed;
            projectileLifetime = def.ProjectileLifetime;
            fireTimer = attackInterval;
        }

        void Update()
        {
            if (health.IsDead || target == null)
            {
                body.linearVelocity = Vector2.zero;
                return;
            }

            Vector2 toTarget = (Vector2)target.position - (Vector2)transform.position;
            Vector2 dir = toTarget.sqrMagnitude > 0.0001f ? toTarget.normalized : Vector2.zero;
            body.linearVelocity = dir * moveSpeed;

            if (contactCooldown > 0f) contactCooldown -= Time.deltaTime;

            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f && projectilePrefab != null)
            {
                fireTimer = attackInterval;
                FireAt(dir == Vector2.zero ? Vector2.down : dir);
            }
        }

        void FireAt(Vector2 dir)
        {
            int outgoing = DamageMath.ScaleOutgoing(attackDamage, DamageType.Physical, attack, intelligence);
            var info = new DamageInfo(outgoing, DamageType.Physical, dir, 0f, Faction.Enemy);
            var proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            proj.Launch(dir, projectileSpeed, projectileLifetime, info);
        }

        void OnCollisionEnter2D(Collision2D collision) => TryContactDamage(collision.collider);
        void OnCollisionStay2D(Collision2D collision) => TryContactDamage(collision.collider);

        void TryContactDamage(Collider2D other)
        {
            if (health.IsDead || contactCooldown > 0f) return;
            var dmg = other.GetComponentInParent<IDamageable>();
            if (dmg == null || dmg.Faction == Faction.Enemy || dmg.IsDead) return;

            int outgoing = DamageMath.ScaleOutgoing(contactDamage, DamageType.Physical, attack, intelligence);
            Vector2 push = (Vector2)other.transform.position - (Vector2)transform.position;
            var info = new DamageInfo(outgoing, DamageType.Physical, push, 2f, Faction.Enemy);
            dmg.TakeDamage(in info);
            contactCooldown = 0.5f;
        }
    }
}
