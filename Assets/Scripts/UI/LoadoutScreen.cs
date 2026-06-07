using UnityEngine;
using Game.Data;
using Game.Run;

namespace Game.UI
{
    // Pick class + weapon + boss, then start the run. Options are assigned in the inspector
    // (the demo builder fills them with the generated assets). Uses simple < name > pickers
    // so it scales to multiple options later.
    public sealed class LoadoutScreen : MonoBehaviour
    {
        [SerializeField] CharacterClassDefinition[] classes;
        [SerializeField] WeaponDefinition[] weapons;
        [SerializeField] BossDefinition[] bosses;
        [SerializeField] string arenaSceneName = "Arena";

        int classIdx, weaponIdx, bossIdx;

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(Screen.width / 2f - 230f, 50f, 460f, Screen.height - 100f));
            GUILayout.Label("CHOOSE YOUR LOADOUT", Title());
            GUILayout.Space(16);

            classIdx = Picker("Class", classes, classIdx, c => c.DisplayName);
            weaponIdx = Picker("Weapon", weapons, weaponIdx, w => w.DisplayName);
            bossIdx = Picker("Boss", bosses, bossIdx, b => b.DisplayName);

            GUILayout.Space(24);

            bool ready = Valid(classes, classIdx) && Valid(weapons, weaponIdx) && Valid(bosses, bossIdx);
            GUI.enabled = ready;
            if (GUILayout.Button("START", GUILayout.Height(50)))
            {
                RunContext.StartNewRun(classes[classIdx], weapons[weaponIdx], bosses[bossIdx]);
                SceneNav.Load(arenaSceneName);
            }
            GUI.enabled = true;
            if (!ready)
                GUILayout.Label("Assign at least one class, weapon, and boss in the inspector.");

            GUILayout.EndArea();
        }

        static int Picker<T>(string label, T[] items, int idx, System.Func<T, string> nameOf) where T : Object
        {
            GUILayout.Space(8);
            if (items == null || items.Length == 0)
            {
                GUILayout.Label($"{label}: (none assigned)");
                return 0;
            }
            idx = Mathf.Clamp(idx, 0, items.Length - 1);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("<", GUILayout.Width(32))) idx = (idx - 1 + items.Length) % items.Length;
            string display = items[idx] != null ? nameOf(items[idx]) : "—";
            GUILayout.Label($"{label}:  {display}", GUILayout.Width(360));
            if (GUILayout.Button(">", GUILayout.Width(32))) idx = (idx + 1) % items.Length;
            GUILayout.EndHorizontal();
            return idx;
        }

        static bool Valid<T>(T[] arr, int i) where T : Object =>
            arr != null && i >= 0 && i < arr.Length && arr[i] != null;

        static GUIStyle Title() => new GUIStyle(GUI.skin.label)
        {
            fontSize = 26,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
        };
    }
}
