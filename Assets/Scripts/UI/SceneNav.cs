using UnityEngine.SceneManagement;
using Game.Core;

namespace Game.UI
{
    // Scene navigation helper used by the IMGUI screens. Routes through the persistent SceneLoader
    // when the game was launched from Boot; falls back to direct SceneManager loads otherwise
    // (so a screen scene can still be tested in isolation).
    public static class SceneNav
    {
        public static void Load(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName)) return;
            if (Services.IsInitialized && Services.Scenes != null)
                Services.Scenes.LoadAsync(sceneName);
            else
                SceneManager.LoadScene(sceneName);
        }

        public static void Quit()
        {
            UnityEngine.Application.Quit();
        }
    }
}
