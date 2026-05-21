# Data

`ScriptableObject` definitions — the content layer. Designers/you create asset instances of these in `Assets/Content/...`.

## What lives here
- `WeaponDefinition.cs` — stats, sprite, fire rate, projectile prefab reference, fire mode enum.
- `AbilityDefinition.cs` — cooldown, hitbox shape, damage type, VFX prefab.
- `BossDefinition.cs` — boss prefab reference, arena prefab, music, intro dialogue, reward table.
- `RewardDefinition.cs` — single drop entry (item, weight, rarity).
- `LoadoutDefinition.cs` — starting loadout presets.
- Shared enums: `DamageType`, `Rarity`, `WeaponClass`.

## Hard rule
**Never mutate a ScriptableObject at runtime.** In the editor, changes persist between play sessions and create reproducible-only-sometimes bugs. Definitions are read-only blueprints. Mutable runtime state lives in `*Instance` plain C# classes in their respective asmdefs (e.g. `WeaponInstance` in `Combat` or `Player`).

## Depends on
Core (for shared enums and any base interfaces).
