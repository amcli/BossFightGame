using UnityEngine;

namespace Game.UI
{
    // Title screen. Bare-bones IMGUI: Play -> Loadout, Quit.
    public sealed class MainMenuScreen : MonoBehaviour
    {
        [SerializeField] string playSceneName = "Loadout";

        void OnGUI()
        {
            const float w = 260f, h = 50f;
            float x = (Screen.width - w) / 2f;
            float y = Screen.height * 0.34f;

            GUI.Label(new Rect(0f, y - 90f, Screen.width, 70f), "BOSS FIGHT", Title());

            if (GUI.Button(new Rect(x, y, w, h), "Play"))
                SceneNav.Load(playSceneName);
            if (GUI.Button(new Rect(x, y + h + 12f, w, h), "Quit"))
                SceneNav.Quit();
        }

        static GUIStyle Title() => new GUIStyle(GUI.skin.label)
        {
            fontSize = 44,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
        };
    }
}
