using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Game.Data;
using Game.Combat;
using Game.Player;
using Game.Bosses;
using Game.Bootstrap;
using Game.UI;

namespace Game.EditorTools
{
    // One-click generator for the bare-bones playable demo. Creates the sample SO assets and
    // player/boss/projectile prefabs from Kenney sprites, then wires the Arena scene so you can
    // press Play on it directly. Re-runnable: it regenerates Assets/Game/Generated each time.
    public static class DemoBuilder
    {
        const string TilesDir = "Assets/Art/Kenny/tiny-dungeon/Tiles";
        const string GameRoot = "Assets/Game";
        const string GenRoot = "Assets/Game/Generated";
        const string PrefabRoot = "Assets/Game/Generated/Prefabs";
        const string ArenaScenePath = "Assets/Scenes/Arena.unity";

        // tile indices chosen for clear readability
        const int KnightTile = 96;
        const int BossTile = 120;
        const int OrbTile = 102;
        const int SwordTile = 104;

        [MenuItem("BossFight/Build Playable Demo")]
        public static void BuildAll()
        {
            try
            {
                ResetGeneratedFolder();

                var knightSprite = LoadTile(KnightTile);
                var bossSprite = LoadTile(BossTile);
                var orbSprite = LoadTile(OrbTile);
                var swordSprite = LoadTile(SwordTile);

                // --- ScriptableObject assets ---
                CreateSword(swordSprite);
                CreateKnightClass(knightSprite);
                var gold = CreateGoldReward();
                CreateBoss(bossSprite, gold);

                // --- prefabs ---
                var projectilePrefab = CreateProjectilePrefab(orbSprite);
                CreatePlayerPrefab(knightSprite);
                CreateBossPrefab(bossSprite, projectilePrefab);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log("[DemoBuilder] Assets + prefabs created. Wiring Arena scene...");

                // --- scene wiring (reloads refs from disk so they survive the asset refresh) ---
                BuildArenaScene();
                BuildFlowScenes(); // MainMenu, Loadout, Results, Boot (leaves Boot open)
                EnsureScenesInBuild(new[] { "Boot", "MainMenu", "Loadout", "Arena", "Results", "Hub" });

                Debug.Log("[DemoBuilder] Done. Boot scene is open — press Play for the full loop " +
                          "(MainMenu -> Loadout -> Arena -> Results).");
                EditorUtility.DisplayDialog("BossFight",
                    "Demo built.\n\nThe Boot scene is open — press Play to run the full loop:\n" +
                    "MainMenu -> Loadout -> Arena -> Results.\n\n" +
                    "In Arena: WASD = move, mouse = aim, left-click = swing.", "OK");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[DemoBuilder] Failed: {e}");
            }
        }

        // ---------------------------------------------------------------- assets

        static MeleeWeaponDefinition CreateSword(Sprite icon)
        {
            var so = ScriptableObject.CreateInstance<MeleeWeaponDefinition>();
            var o = new SerializedObject(so);
            o.FindProperty("displayName").stringValue = "Knight's Sword";
            o.FindProperty("description").stringValue = "A reliable arming sword.";
            o.FindProperty("icon").objectReferenceValue = icon;
            o.FindProperty("rarity").enumValueIndex = (int)ItemRarity.Common;
            o.FindProperty("damage").intValue = 12;
            o.FindProperty("damageType").enumValueIndex = (int)DamageType.Physical;
            o.FindProperty("fireRate").floatValue = 2f;          // 2 swings/sec -> 0.5s cooldown
            o.FindProperty("knockbackForce").floatValue = 4f;
            o.FindProperty("hitboxShape").enumValueIndex = (int)HitboxShape.Box;
            o.FindProperty("hitboxSize").vector2Value = new Vector2(1.3f, 1.3f);
            o.FindProperty("hitboxOffset").vector2Value = Vector2.zero;
            o.ApplyModifiedPropertiesWithoutUndo();
            AssetDatabase.CreateAsset(so, $"{GenRoot}/KnightSword.asset");
            return so;
        }

        static CharacterClassDefinition CreateKnightClass(Sprite icon)
        {
            var so = ScriptableObject.CreateInstance<CharacterClassDefinition>();
            var o = new SerializedObject(so);
            o.FindProperty("displayName").stringValue = "Knight";
            o.FindProperty("description").stringValue = "Sturdy melee fighter.";
            o.FindProperty("icon").objectReferenceValue = icon;
            var s = o.FindProperty("baseStats");
            s.FindPropertyRelative("maxHP").intValue = 120;
            s.FindPropertyRelative("defense").intValue = 8;
            s.FindPropertyRelative("resistance").intValue = 4;
            s.FindPropertyRelative("attack").intValue = 20;
            s.FindPropertyRelative("intelligence").intValue = 0;
            s.FindPropertyRelative("moveSpeed").intValue = 10;
            s.FindPropertyRelative("maxEnergy").intValue = 100;
            o.ApplyModifiedPropertiesWithoutUndo();
            AssetDatabase.CreateAsset(so, $"{GenRoot}/KnightClass.asset");
            return so;
        }

        static CurrencyRewardDefinition CreateGoldReward()
        {
            var so = ScriptableObject.CreateInstance<CurrencyRewardDefinition>();
            var o = new SerializedObject(so);
            o.FindProperty("displayName").stringValue = "Gold";
            o.FindProperty("rarity").enumValueIndex = (int)ItemRarity.Common;
            o.FindProperty("amount").intValue = 50;
            o.ApplyModifiedPropertiesWithoutUndo();
            AssetDatabase.CreateAsset(so, $"{GenRoot}/GoldReward.asset");
            return so;
        }

        static BossDefinition CreateBoss(Sprite body, RewardDefinition reward)
        {
            var so = ScriptableObject.CreateInstance<BossDefinition>();
            var o = new SerializedObject(so);
            o.FindProperty("bossId").stringValue = "ember";
            o.FindProperty("displayName").stringValue = "Ember, the Cinder Fiend";
            o.FindProperty("description").stringValue = "A roiling elemental of living fire.";
            o.FindProperty("bodySprite").objectReferenceValue = body;
            var s = o.FindProperty("baseStats");
            s.FindPropertyRelative("maxHP").intValue = 200;
            s.FindPropertyRelative("defense").intValue = 5;
            s.FindPropertyRelative("resistance").intValue = 5;
            s.FindPropertyRelative("attack").intValue = 15;
            s.FindPropertyRelative("intelligence").intValue = 10;
            s.FindPropertyRelative("moveSpeed").intValue = 0;
            s.FindPropertyRelative("maxEnergy").intValue = 0;
            o.FindProperty("contactDamage").intValue = 8;
            o.FindProperty("attackDamage").intValue = 10;
            o.FindProperty("attackInterval").floatValue = 1.8f;
            o.FindProperty("projectileSpeed").floatValue = 5f;
            o.FindProperty("projectileLifetime").floatValue = 4f;
            var rewards = o.FindProperty("rewards");
            rewards.arraySize = 1;
            rewards.GetArrayElementAtIndex(0).objectReferenceValue = reward;
            o.ApplyModifiedPropertiesWithoutUndo();
            AssetDatabase.CreateAsset(so, $"{GenRoot}/EmberBoss.asset");
            return so;
        }

        // ---------------------------------------------------------------- prefabs

        static GameObject CreateProjectilePrefab(Sprite sprite)
        {
            var go = new GameObject("BossProjectile");
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.color = new Color(1f, 0.5f, 0.12f);
            sr.sortingOrder = 5;
            var rb = go.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
            var col = go.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 0.2f;
            go.AddComponent<Projectile>();

            var prefab = PrefabUtility.SaveAsPrefabAsset(go, $"{PrefabRoot}/BossProjectile.prefab");
            Object.DestroyImmediate(go);
            return prefab;
        }

        static GameObject CreatePlayerPrefab(Sprite sprite)
        {
            var go = new GameObject("Player");
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.sortingOrder = 10;
            var rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            var col = go.AddComponent<CircleCollider2D>();
            col.radius = 0.4f;
            var health = go.AddComponent<Health>();
            var ho = new SerializedObject(health);
            ho.FindProperty("faction").enumValueIndex = (int)Faction.Player;
            ho.FindProperty("maxHP").intValue = 120;
            ho.ApplyModifiedPropertiesWithoutUndo();
            go.AddComponent<PlayerController>();

            var prefab = PrefabUtility.SaveAsPrefabAsset(go, $"{PrefabRoot}/Player.prefab");
            Object.DestroyImmediate(go);
            return prefab;
        }

        static GameObject CreateBossPrefab(Sprite sprite, GameObject projectilePrefab)
        {
            var go = new GameObject("Boss");
            go.transform.localScale = Vector3.one * 1.6f;
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.sortingOrder = 10;
            var rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            var col = go.AddComponent<CircleCollider2D>();
            col.radius = 0.45f;
            var health = go.AddComponent<Health>();
            var ho = new SerializedObject(health);
            ho.FindProperty("faction").enumValueIndex = (int)Faction.Enemy;
            ho.FindProperty("maxHP").intValue = 200;
            ho.ApplyModifiedPropertiesWithoutUndo();

            var bc = go.AddComponent<BossController>();
            var bo = new SerializedObject(bc);
            bo.FindProperty("projectilePrefab").objectReferenceValue = projectilePrefab.GetComponent<Projectile>();
            bo.ApplyModifiedPropertiesWithoutUndo();

            var prefab = PrefabUtility.SaveAsPrefabAsset(go, $"{PrefabRoot}/Boss.prefab");
            Object.DestroyImmediate(go);
            return prefab;
        }

        // ---------------------------------------------------------------- scene

        static void BuildArenaScene()
        {
            // Open the scene FIRST. OpenScene(Single) unloads unused assets, which would destroy any
            // assets loaded before this call (they're only held by C# locals, which the unloader
            // ignores). Loading AFTER the switch keeps the references valid through wiring + save.
            var scene = EditorSceneManager.OpenScene(ArenaScenePath, OpenSceneMode.Single);

            var playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{PrefabRoot}/Player.prefab");
            var bossPrefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{PrefabRoot}/Boss.prefab");
            var cls = AssetDatabase.LoadAssetAtPath<CharacterClassDefinition>($"{GenRoot}/KnightClass.asset");
            var weapon = AssetDatabase.LoadAssetAtPath<MeleeWeaponDefinition>($"{GenRoot}/KnightSword.asset");
            var boss = AssetDatabase.LoadAssetAtPath<BossDefinition>($"{GenRoot}/EmberBoss.asset");
            if (playerPrefab == null || bossPrefab == null || cls == null || weapon == null || boss == null)
            {
                Debug.LogError("[DemoBuilder] Scene wiring aborted; a generated asset failed to load " +
                    $"(player={playerPrefab}, boss={bossPrefab}, class={cls}, weapon={weapon}, bossDef={boss}).");
                return;
            }

            // camera
            var cam = Camera.main;
            if (cam == null)
            {
                var camGO = new GameObject("Main Camera");
                camGO.tag = "MainCamera";
                cam = camGO.AddComponent<Camera>();
                camGO.AddComponent<AudioListener>();
                Debug.LogWarning("[DemoBuilder] No camera in Arena; created a plain one. If the view is pink/empty, this camera may need URP camera data.");
            }
            cam.orthographic = true;
            cam.orthographicSize = 6f;
            cam.transform.position = new Vector3(0f, 0f, -10f);
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.10f, 0.10f, 0.14f);

            // director (replace any existing)
            var existing = Object.FindAnyObjectByType<ArenaDirector>();
            if (existing != null) Object.DestroyImmediate(existing.gameObject);
            var dirGO = new GameObject("ArenaDirector");
            var dir = dirGO.AddComponent<ArenaDirector>();
            var o = new SerializedObject(dir);
            o.FindProperty("playerPrefab").objectReferenceValue = playerPrefab.GetComponent<PlayerController>();
            o.FindProperty("bossPrefab").objectReferenceValue = bossPrefab.GetComponent<BossController>();
            o.FindProperty("defaultClass").objectReferenceValue = cls;
            o.FindProperty("defaultWeapon").objectReferenceValue = weapon;
            o.FindProperty("defaultBoss").objectReferenceValue = boss;
            o.FindProperty("resultsSceneName").stringValue = "Results";
            o.ApplyModifiedPropertiesWithoutUndo();

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);

            Debug.Log($"[DemoBuilder] Arena wired & saved -> class={cls.name}, weapon={weapon.name}, " +
                      $"boss={boss.name}, playerPrefab={playerPrefab.name}, bossPrefab={bossPrefab.name}");
        }

        // ---------------------------------------------------------------- flow scenes

        static void BuildFlowScenes()
        {
            BuildScreenScene<MainMenuScreen>("MainMenu", "MainMenuScreen", s =>
            {
                var o = new SerializedObject(s);
                o.FindProperty("playSceneName").stringValue = "Loadout";
                o.ApplyModifiedPropertiesWithoutUndo();
            });

            BuildScreenScene<LoadoutScreen>("Loadout", "LoadoutScreen", s =>
            {
                // load AFTER the scene switch so OpenScene's asset unload can't destroy these
                var knight = AssetDatabase.LoadAssetAtPath<CharacterClassDefinition>($"{GenRoot}/KnightClass.asset");
                var sword = AssetDatabase.LoadAssetAtPath<MeleeWeaponDefinition>($"{GenRoot}/KnightSword.asset");
                var boss = AssetDatabase.LoadAssetAtPath<BossDefinition>($"{GenRoot}/EmberBoss.asset");
                var o = new SerializedObject(s);
                SetSingle(o.FindProperty("classes"), knight);
                SetSingle(o.FindProperty("weapons"), sword);
                SetSingle(o.FindProperty("bosses"), boss);
                o.FindProperty("arenaSceneName").stringValue = "Arena";
                o.ApplyModifiedPropertiesWithoutUndo();
            });

            BuildScreenScene<ResultsScreen>("Results", "ResultsScreen", s =>
            {
                var o = new SerializedObject(s);
                o.FindProperty("menuSceneName").stringValue = "MainMenu";
                o.FindProperty("arenaSceneName").stringValue = "Arena";
                o.ApplyModifiedPropertiesWithoutUndo();
            });

            EnsureBootScene(); // opened last -> stays active for the user to press Play
        }

        static void BuildScreenScene<T>(string sceneName, string goName, System.Action<T> configure)
            where T : MonoBehaviour
        {
            string path = $"Assets/Scenes/{sceneName}.unity";
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(path) == null)
            {
                Debug.LogWarning($"[DemoBuilder] Scene not found, skipped: {path}");
                return;
            }
            var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
            EnsureCamera();

            var existing = Object.FindAnyObjectByType<T>();
            if (existing != null) Object.DestroyImmediate(existing.gameObject);

            var go = new GameObject(goName);
            var comp = go.AddComponent<T>();
            configure?.Invoke(comp);

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log($"[DemoBuilder] Built {sceneName} ({typeof(T).Name}).");
        }

        static void EnsureBootScene()
        {
            const string path = "Assets/Scenes/Boot.unity";
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(path) == null)
            {
                Debug.LogWarning("[DemoBuilder] Boot.unity not found; cannot ensure entry point.");
                return;
            }
            var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
            var boot = Object.FindAnyObjectByType<GameBootstrap>();
            if (boot == null)
            {
                var go = new GameObject("GameBootstrap");
                boot = go.AddComponent<GameBootstrap>();
            }
            var o = new SerializedObject(boot);
            o.FindProperty("firstSceneName").stringValue = "MainMenu";
            o.ApplyModifiedPropertiesWithoutUndo();
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log("[DemoBuilder] Boot ensured (GameBootstrap -> MainMenu).");
        }

        static void EnsureCamera()
        {
            if (Camera.main != null) return;
            var camGO = new GameObject("Main Camera") { tag = "MainCamera" };
            var cam = camGO.AddComponent<Camera>();
            camGO.AddComponent<AudioListener>();
            cam.orthographic = true;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.08f, 0.08f, 0.12f);
            cam.transform.position = new Vector3(0f, 0f, -10f);
        }

        static void SetSingle(SerializedProperty arrayProp, Object item)
        {
            arrayProp.arraySize = 1;
            arrayProp.GetArrayElementAtIndex(0).objectReferenceValue = item;
        }

        static void EnsureScenesInBuild(string[] orderedNames)
        {
            var result = new List<EditorBuildSettingsScene>();
            void AddPath(string path)
            {
                if (AssetDatabase.LoadAssetAtPath<SceneAsset>(path) == null) return;
                if (result.Exists(s => s.path == path)) return;
                result.Add(new EditorBuildSettingsScene(path, true));
            }
            foreach (var n in orderedNames) AddPath($"Assets/Scenes/{n}.unity");
            foreach (var s in EditorBuildSettings.scenes) AddPath(s.path); // keep any extras
            EditorBuildSettings.scenes = result.ToArray();
        }

        // ---------------------------------------------------------------- helpers

        static void ResetGeneratedFolder()
        {
            if (AssetDatabase.IsValidFolder(GenRoot))
            {
                AssetDatabase.DeleteAsset(GenRoot);
                AssetDatabase.Refresh();
            }
            if (!AssetDatabase.IsValidFolder(GameRoot)) AssetDatabase.CreateFolder("Assets", "Game");
            if (!AssetDatabase.IsValidFolder(GenRoot)) AssetDatabase.CreateFolder(GameRoot, "Generated");
            if (!AssetDatabase.IsValidFolder(PrefabRoot)) AssetDatabase.CreateFolder(GenRoot, "Prefabs");
        }

        static Sprite LoadTile(int index)
        {
            string path = $"{TilesDir}/tile_{index:0000}.png";
            if (AssetImporter.GetAtPath(path) is TextureImporter imp)
            {
                bool changed = false;
                if (imp.textureType != TextureImporterType.Sprite) { imp.textureType = TextureImporterType.Sprite; changed = true; }
                if (imp.spriteImportMode != SpriteImportMode.Single) { imp.spriteImportMode = SpriteImportMode.Single; changed = true; }
                if (imp.filterMode != FilterMode.Point) { imp.filterMode = FilterMode.Point; changed = true; }
                if (!Mathf.Approximately(imp.spritePixelsPerUnit, 16f)) { imp.spritePixelsPerUnit = 16f; changed = true; }
                if (imp.textureCompression != TextureImporterCompression.Uncompressed) { imp.textureCompression = TextureImporterCompression.Uncompressed; changed = true; }
                if (changed) imp.SaveAndReimport();
            }
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite == null) Debug.LogError($"[DemoBuilder] Could not load sprite: {path}");
            return sprite;
        }
    }
}
