# Run

The roguelike run lifecycle — everything that exists from "Start Run" to death/victory.

## What lives here
- `RunState.cs` — current HP, gold-this-run, picked rewards, defeated bosses. Lives on the *Run root* GameObject, destroyed when the run ends.
- `RunManager.cs` — owns the Run root. `StartRun(LoadoutInstance)`, `EndRun(RunResult)`. Decides what scene to load next.
- `BossSelectController.cs` — drives the boss-pick screen between fights (Hades-style door selection).
- `RewardSelectController.cs` — post-boss reward draw from the boss's `RewardDefinition` pool.
- `LoadoutInstance.cs` — runtime container for the player's chosen weapon + abilities for this run.

## Why a separate Run layer
Meta progression (Core) survives forever. Per-arena state (Combat/Player) rebuilds each scene. Run state sits between them — survives scene transitions *within* a run, dies when the run ends. Mixing it with either of the other two creates cleanup bugs on restart.

## Depends on
Core, Data.
