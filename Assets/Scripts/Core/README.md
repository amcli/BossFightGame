# Core

Persistent services that live for the whole app session. No gameplay logic — only infrastructure that everything else stands on.

## What lives here
- `Services.cs` — static facade so any script can call `Services.Save.Load()` without holding references.
- `EventBus.cs` — typed pub/sub for cross-system signals (`BossDefeated`, `PlayerDamaged`, etc.).
- `SaveSystem.cs` — JSON save/load to `Application.persistentDataPath`. Handles save versioning.
- `MetaProgression.cs` — currency, unlocked weapons/bosses, achievements. Survives runs.
- `SceneLoader.cs` — async scene transitions with fade, additive loading helpers.
- `AudioManager.cs` — music + SFX channels with volume mixing.
- `SettingsManager.cs` — graphics, audio, key remaps. Persists via SaveSystem.

## Rule
No `Find`, no `Resources.Load` at runtime, no scene-specific assumptions. Core code must work whether the game is in MainMenu, Hub, or Arena.

## Depends on
Nothing. This is the foundation.
