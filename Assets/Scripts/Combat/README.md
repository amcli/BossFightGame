# Combat

The damage system. Shared by player, enemies, environmental hazards — anything that hits or gets hit.

## What lives here
- `IDamageable.cs` — interface for anything that takes damage.
- `DamageInfo.cs` — struct passed to `IDamageable.TakeDamage(in DamageInfo)`. Holds amount, source, hit point, damage type, knockback.
- `Hitbox.cs` — emits damage. Active during attack windows.
- `Hurtbox.cs` — receives damage. Can be disabled for iframes.
- `Projectile.cs` + `ProjectilePool.cs` — pooled projectile movement and collision.
- `HitDetection.cs` — wraps `Physics2D.OverlapBoxNonAlloc`-style queries used by hitboxes.

## Design rules
- Hitboxes use **manual overlap queries**, not `OnTriggerEnter2D`. Triggers fire on Unity's schedule and skip frames under load; manual queries give frame-perfect control.
- Triggers (`OnTriggerEnter2D`) are still fine for pickups and zone effects.
- Every projectile is pooled. Use `UnityEngine.Pool.ObjectPool<T>` — don't roll a custom one.

## Depends on
Core, Data.
