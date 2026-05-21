# Bosses

Per-boss controllers, state machines, and attack patterns.

## Layout
One subfolder per boss:
```
Bosses/
  Common/        ← BossController, BossStateMachine, IState, BossPhase
  Boss_Knight/   ← KnightController, KnightStates, attack components
  Boss_Witch/    ← ...
```

## What lives here
- `BossController.cs` — HP, current phase, holds the state machine. Implements `IDamageable`. Raises `BossDefeated` on death.
- `BossStateMachine.cs` — hand-rolled hierarchical FSM. `IState { void Enter(); void Tick(float dt); void Exit(); }`.
- Per-boss attack components — coroutines or behaviors invoked by states.

## Rule
**Do not use Unity's Animator state machine for gameplay logic.** Animator is for visuals only. Gameplay state lives in code FSMs in this asmdef.

## Depends on
Core, Data, Combat.
