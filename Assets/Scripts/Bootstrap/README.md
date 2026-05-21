# Bootstrap

Entry point of the game. Spawns the persistent service root in `Boot.unity`, then loads `MainMenu`.

## What lives here
- `GameBootstrap.cs` — single MonoBehaviour placed in `Boot.unity`. Instantiates the persistent root, wires up `Services`, then triggers `SceneLoader.LoadAsync("MainMenu")`.
- `PersistentRoot.prefab` (later) — the `DontDestroyOnLoad` GameObject holding all service components.

## Rule
Nothing else should reference this asmdef. Bootstrap is the *only* place allowed to know the full graph of services and the only place that decides startup order.

## Depends on
Core, Data, Combat, Player, Bosses, Run, UI — Bootstrap is the composition root, so it sees everything.
