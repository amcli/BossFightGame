using UnityEngine;

namespace Game.Core
{
    public sealed class SettingsManager
    {
        readonly SaveSystem save;

        public SettingsManager(SaveSystem save)
        {
            this.save = save;
        }

        SettingsData Data => save.Current.Settings;

        public float MasterVolume
        {
            get => Data.MasterVolume;
            set { Data.MasterVolume = Mathf.Clamp01(value); save.Save(); }
        }

        public float MusicVolume
        {
            get => Data.MusicVolume;
            set { Data.MusicVolume = Mathf.Clamp01(value); save.Save(); }
        }

        public float SfxVolume
        {
            get => Data.SfxVolume;
            set { Data.SfxVolume = Mathf.Clamp01(value); save.Save(); }
        }
    }
}
