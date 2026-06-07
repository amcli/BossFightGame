using UnityEngine;
using Game.Core;
using Game.Run;

namespace Game.UI
{
    // Shows the run outcome and offers Retry (same loadout) or back to the menu.
    public sealed class ResultsScreen : MonoBehaviour
    {
        [SerializeField] string menuSceneName = "MainMenu";
        [SerializeField] string arenaSceneName = "Arena";

        void OnGUI()
        {
            const float w = 260f, h = 48f;
            float x = (Screen.width - w) / 2f;
            float y = Screen.height * 0.40f;

            bool win = RunContext.Outcome == RunOutcome.Victory;
            GUI.Label(new Rect(0f, y - 90f, Screen.width, 70f), win ? "VICTORY" : "DEFEAT", Title(win));

            if (Services.IsInitialized && Services.Meta != null)
                GUI.Label(new Rect(0f, y - 20f, Screen.width, 24f), $"Gold: {Services.Meta.Currency}", Center());

            if (GUI.Button(new Rect(x, y, w, h), "Retry"))
                SceneNav.Load(arenaSceneName);
            if (GUI.Button(new Rect(x, y + h + 12f, w, h), "Main Menu"))
                SceneNav.Load(menuSceneName);
        }

        static GUIStyle Title(bool win) => new GUIStyle(GUI.skin.label)
        {
            fontSize = 46,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = win ? new Color(0.5f, 1f, 0.5f) : new Color(1f, 0.5f, 0.5f) },
        };

        static GUIStyle Center() => new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
    }
}
