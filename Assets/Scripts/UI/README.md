# UI

All UI presentation. Reads from systems but never writes to them directly — UI publishes UI-intent events that systems handle.

## What lives here
- `HUD/` — in-arena overlay: HP, ammo, ability cooldowns, boss bar.
- `Menus/` — MainMenu, Pause, Settings.
- `Loadout/` — weapon/ability picker.
- `Hub/` — meta-progression spending screens.
- `Results/` — post-run summary.

## Rules
- UI subscribes to `EventBus` events (`PlayerDamaged`, `BossPhaseChanged`) to refresh.
- UI calls `Services.Meta.SpendCurrency(...)` etc. for player intent — never reaches into gameplay components.
- One `UIRoot` Canvas per scene; subscreens are sibling panels toggled by a controller.

## Depends on
Core, Data, Run.
