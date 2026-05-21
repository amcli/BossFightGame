# Tests / EditMode

Unit tests that don't need Play mode — pure C# logic only.

## What goes here
- `EventBusTests` — pub/sub delivery, unsubscribe, generic dispatch.
- `SaveSystemTests` — round-trip serialization, version upgrade hooks.
- `DamageInfoTests` — modifier math, damage type resistances.
- `WeaponInstanceTests` — fire-rate gating, ammo accounting.

## What does NOT go here
Anything that needs MonoBehaviour lifecycle, physics simulation, or a scene. Those go in `Tests/PlayMode/` (add when first needed).

## How to run
Window → General → Test Runner → EditMode tab → Run All.

## Depends on
Whichever asmdefs the tests cover. Update `Game.Tests.EditMode.asmdef` references as you add tests.
