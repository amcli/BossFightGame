using System;
using System.IO;
using UnityEngine;

namespace Game.Core
{
    public sealed class SaveSystem
    {
        const string FileName = "save.json";

        string FilePath => Path.Combine(Application.persistentDataPath, FileName);

        public SaveData Current { get; private set; }

        public void Load()
        {
            if (!File.Exists(FilePath))
            {
                Current = new SaveData();
                return;
            }

            try
            {
                var json = File.ReadAllText(FilePath);
                var data = JsonUtility.FromJson<SaveData>(json) ?? new SaveData();
                Migrate(data);
                Current = data;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveSystem] Load failed: {e}. Starting fresh.");
                Current = new SaveData();
            }
        }

        public void Save()
        {
            Current ??= new SaveData();
            var json = JsonUtility.ToJson(Current, prettyPrint: true);
            File.WriteAllText(FilePath, json);
        }

        void Migrate(SaveData data)
        {
            // Schema upgrade hook. Example shape for the future:
            // if (data.SaveVersion < 2) { /* upgrade v1 -> v2 */ data.SaveVersion = 2; }
            data.SaveVersion = SaveData.CurrentSaveVersion;
        }
    }
}
