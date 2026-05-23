using Game.Core;
using UnityEngine;

namespace Game.Bootstrap
{
    // Entry point. Lives on a GameObject in Boot.unity. Runs once at app start,
    // spawns the PersistentRoot, populates Services, then loads the first real scene.
    // Idempotent: if a second instance somehow appears (editor re-enter, dev reload),
    // it self-destroys.
    [DefaultExecutionOrder(-10000)]
    public sealed class GameBootstrap : MonoBehaviour
    {
        [SerializeField] string firstSceneName = "MainMenu";

        void Awake()
        {
            if (Services.IsInitialized)
            {
                Destroy(gameObject);
                return;
            }

            InitializeServices();
        }

        void Start()
        {
            if (!Services.IsInitialized) return;
            Services.Scenes.LoadAsync(firstSceneName);
        }

        void InitializeServices()
        {
            var rootGO = new GameObject("[PersistentRoot]");
            DontDestroyOnLoad(rootGO);

            var root = rootGO.AddComponent<PersistentRoot>();
            root.Scenes = rootGO.AddComponent<SceneLoader>();
            root.Audio = rootGO.AddComponent<AudioManager>();

            var saveSystem = new SaveSystem();
            saveSystem.Load();

            Services.Events = new EventBus();
            Services.Save = saveSystem;
            Services.Meta = new MetaProgression(saveSystem);
            Services.Settings = new SettingsManager(saveSystem);
            Services.Scenes = root.Scenes;
            Services.Audio = root.Audio;
            Services.IsInitialized = true;

            Debug.Log("[Bootstrap] Services initialized.");
        }
    }
}
