using Game.Data;

namespace Game.Run
{
    // Carries the player's selection and the run result across scene loads
    // (Loadout sets it -> Arena reads it -> Results reads the outcome).
    // Plain static; survives scene loads for the app session.
    public static class RunContext
    {
        public static CharacterClassDefinition SelectedClass { get; set; }
        public static WeaponDefinition SelectedWeapon { get; set; }
        public static BossDefinition CurrentBoss { get; set; }
        public static RunOutcome Outcome { get; set; }

        public static bool IsValid =>
            SelectedClass != null && SelectedWeapon != null && CurrentBoss != null;

        public static void StartNewRun(CharacterClassDefinition cls, WeaponDefinition weapon, BossDefinition boss)
        {
            SelectedClass = cls;
            SelectedWeapon = weapon;
            CurrentBoss = boss;
            Outcome = RunOutcome.Undecided;
        }
    }
}
