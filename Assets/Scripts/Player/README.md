# Player

Everything specific to controlling the player character.

## What lives here
- `PlayerController.cs` — movement, dash, facing direction. Reads from `PlayerInputHandler`.
- `PlayerInputHandler.cs` — wraps the generated `InputSystem_Actions` C# class. Exposes `event Action<Vector2> MoveChanged`, `event Action FirePressed`, etc. Everything else subscribes to this wrapper, never to InputActions directly.
- `WeaponController.cs` — holds the current `WeaponInstance`, dispatches fire input to it, swaps weapons.
- `AbilitySlotController.cs` — array of `AbilityInstance` slots with cooldown ticking.
- `WeaponInstance.cs` / `AbilityInstance.cs` — plain C# classes wrapping a `*Definition` + runtime state (current ammo, cooldown timers).
- `PlayerHealth.cs` — implements `IDamageable`, raises `PlayerDamaged` on the EventBus.

## Why the input wrapper
Lets us swap the input source (AI for replays, recorded inputs for tests, network for future MP) without touching the controller. The controller never knows whether a human or a script is driving it.

## Depends on
Core, Data, Combat.
