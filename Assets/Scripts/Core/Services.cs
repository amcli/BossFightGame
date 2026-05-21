namespace Game.Core
{
    // Static facade for the persistent services. Populated once by GameBootstrap
    // at app start. Access from anywhere: Services.Save.Load(), Services.Events.Publish(...).
    public static class Services
    {
        public static bool IsInitialized { get; internal set; }

        public static EventBus Events { get; internal set; }
        public static SaveSystem Save { get; internal set; }
        public static MetaProgression Meta { get; internal set; }
        public static SettingsManager Settings { get; internal set; }
        public static SceneLoader Scenes { get; internal set; }
        public static AudioManager Audio { get; internal set; }
    }
}
