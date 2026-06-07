using UnityEngine;
using Game.Core;
using Game.Data;
using Game.Combat;
using Game.Player;
using Game.Bosses;
using Game.Run;

namespace Game.Bootstrap
{
    // Composition root for the Arena scene. Reads RunContext (set by Loadout), spawns and configures
    // the player + boss, draws minimal HP bars, and routes to Results on win/lose. Falls back to
    // serialized default SOs so the Arena scene can also be played directly during development.
    public sealed class ArenaDirector : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] PlayerController playerPrefab;
        [SerializeField] BossController bossPrefab;

        [Header("Fallback selection (used if launched without a RunContext)")]
        [SerializeField] CharacterClassDefinition defaultClass;
        [SerializeField] WeaponDefinition defaultWeapon;
        [SerializeField] BossDefinition defaultBoss;

        [Header("Spawn points")]
        [SerializeField] Vector2 playerSpawn = new Vector2(-3f, 0f);
        [SerializeField] Vector2 bossSpawn = new Vector2(3f, 0f);

        [Header("Flow")]
        [SerializeField] string resultsSceneName = "Results";

        Health playerHealth, bossHealth;
        BossDefinition bossDef;
        bool resolved;

        void Start()
        {
            var cls = RunContext.SelectedClass != null ? RunContext.SelectedClass : defaultClass;
            var weapon = RunContext.SelectedWeapon != null ? RunContext.SelectedWeapon : defaultWeapon;
            bossDef = RunContext.CurrentBoss != null ? RunContext.CurrentBoss : defaultBoss;

            if (cls == null || weapon == null || bossDef == null)
            {
                Debug.LogError("[ArenaDirector] Missing class/weapon/boss. Assign fallbacks in the inspector or enter via the Loadout scene.");
                return;
            }

            var player = Instantiate(playerPrefab, playerSpawn, Quaternion.identity);
            player.Configure(cls, weapon);
            playerHealth = player.Health;
            playerHealth.Died += OnPlayerDied;

            var boss = Instantiate(bossPrefab, bossSpawn, Quaternion.identity);
            boss.Configure(bossDef, player.transform);
            bossHealth = boss.Health;
            bossHealth.Died += OnBossDied;
        }

        void OnBossDied()
        {
            if (resolved) return;
            resolved = true;
            RunContext.Outcome = RunOutcome.Victory;
            GrantRewards();
            if (Services.IsInitialized && Services.Meta != null && !string.IsNullOrEmpty(bossDef.BossId))
                Services.Meta.RecordBossDefeated(bossDef.BossId);
            LoadResults();
        }

        void OnPlayerDied()
        {
            if (resolved) return;
            resolved = true;
            RunContext.Outcome = RunOutcome.Defeat;
            LoadResults();
        }

        void GrantRewards()
        {
            if (bossDef.Rewards == null || !Services.IsInitialized || Services.Meta == null) return;
            foreach (var r in bossDef.Rewards)
            {
                if (r is CurrencyRewardDefinition currency)
                    Services.Meta.AddCurrency(currency.Amount);
                else if (r is WeaponRewardDefinition wr && wr.Weapon != null)
                    Services.Meta.UnlockWeapon(wr.Weapon.name);
                // StatBoost / Heal are applied to the live run/player; wired in a later pass.
            }
        }

        void LoadResults()
        {
            if (Services.IsInitialized && Services.Scenes != null)
                Services.Scenes.LoadAsync(resultsSceneName);
            else
                Debug.Log($"[ArenaDirector] Run over: {RunContext.Outcome} (no SceneLoader; Arena launched directly).");
        }

        // Minimal IMGUI bars + outcome banner so we need no uGUI wiring for the bare-bones build.
        void OnGUI()
        {
            if (playerHealth != null) DrawBar(10f, 10f, "Player", playerHealth);
            if (bossHealth != null) DrawBar(10f, 38f, bossDef != null ? bossDef.DisplayName : "Boss", bossHealth);

            if (resolved)
            {
                var style = new GUIStyle(GUI.skin.label) { fontSize = 32, alignment = TextAnchor.MiddleCenter };
                GUI.Label(new Rect(0f, Screen.height * 0.4f, Screen.width, 50f),
                    RunContext.Outcome == RunOutcome.Victory ? "VICTORY" : "DEFEAT", style);
            }
        }

        static void DrawBar(float x, float y, string label, Health h)
        {
            float frac = h.MaxHP > 0 ? Mathf.Clamp01((float)h.CurrentHP / h.MaxHP) : 0f;
            GUI.Box(new Rect(x, y, 204f, 24f), GUIContent.none);
            GUI.Box(new Rect(x + 2f, y + 2f, 200f * frac, 20f), GUIContent.none);
            GUI.Label(new Rect(x + 8f, y + 3f, 240f, 20f), $"{label}: {h.CurrentHP}/{h.MaxHP}");
        }
    }
}
