using System;
using System.Collections.Generic;

namespace Game.Core
{
    [Serializable]
    public class SaveData
    {
        public const int CurrentSaveVersion = 1;

        public int SaveVersion = CurrentSaveVersion;
        public MetaProgressionData Meta = new MetaProgressionData();
        public SettingsData Settings = new SettingsData();
    }

    [Serializable]
    public class MetaProgressionData
    {
        public int Currency;
        public List<string> UnlockedWeaponIds = new List<string>();
        public List<string> UnlockedBossIds = new List<string>();
        public List<string> DefeatedBossIds = new List<string>();
    }

    [Serializable]
    public class SettingsData
    {
        public float MasterVolume = 1f;
        public float MusicVolume = 1f;
        public float SfxVolume = 1f;
    }
}
