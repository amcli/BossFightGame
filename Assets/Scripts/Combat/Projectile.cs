using UnityEngine;

namespace Game.Combat
{
    // Simple moving hitbox. Trigger collider passes through bodies and damages the first opposing
    // IDamageable it touches, then despawns. Needs a trigger Collider2D + a Rigidbody2D for trigger callbacks.
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public sealed class Projectile : MonoBehaviour
    {
        Vector2 velocity;
        float lifeRemaining;
        DamageInfo damage;

        public void Launch(Vector2 dir, float speed, float lifetime, in DamageInfo dmg)
        {
            velocity = dir.normalized * speed;
            lifeRemaining = lifetime;
            damage = dmg;
        }

        void Update()
        {
            transform.position += (Vector3)(velocity * Time.deltaTime);
            lifeRemaining -= Time.deltaTime;
            if (lifeRemaining <= 0f) Destroy(gameObject);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            var target = other.GetComponentInParent<IDamageable>();
            if (target == null || target.Faction == damage.SourceFaction || target.IsDead) return;
            target.TakeDamage(in damage);
            Destroy(gameObject);
        }
    }
}
