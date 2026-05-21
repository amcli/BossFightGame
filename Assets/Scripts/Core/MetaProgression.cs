using System.Collections.Generic;

namespace Game.Core
{
    public sealed class MetaProgression
    {
        readonly SaveSystem save;

        public MetaProgression(SaveSystem save)
        {
            this.save = save;
        }

        MetaProgressionData Data => save.Current.Meta;

        public int Currency => Data.Currency;
        public IReadOnlyList<string> UnlockedWeaponIds => Data.UnlockedWeaponIds;
        public IReadOnlyList<string> UnlockedBossIds => Data.UnlockedBossIds;
        public IReadOnlyList<string> DefeatedBossIds => Data.DefeatedBossIds;

        public void AddCurrency(int amount)
        {
            if (amount <= 0) return;
            Data.Currency += amount;
            save.Save();
        }

        public bool TrySpendCurrency(int amount)
        {
            if (amount <= 0 || amount > Data.Currency) return false;
            Data.Currency -= amount;
            save.Save();
            return true;
        }

        public void UnlockWeapon(string weaponId)
        {
            if (string.IsNullOrEmpty(weaponId) || Data.UnlockedWeaponIds.Contains(weaponId)) return;
            Data.UnlockedWeaponIds.Add(weaponId);
            save.Save();
        }

        public void UnlockBoss(string bossId)
        {
            if (string.IsNullOrEmpty(bossId) || Data.UnlockedBossIds.Contains(bossId)) return;
            Data.UnlockedBossIds.Add(bossId);
            save.Save();
        }

        public void RecordBossDefeated(string bossId)
        {
            if (string.IsNullOrEmpty(bossId) || Data.DefeatedBossIds.Contains(bossId)) return;
            Data.DefeatedBossIds.Add(bossId);
            save.Save();
        }
    }
}
