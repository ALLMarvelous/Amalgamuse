using Amalgamuse.Utils;
using HarmonyLib;
using Il2CppAssets.Scripts.GameCore.Managers;
using Il2CppFormulaBase;
using System.Reflection;

namespace Amalgamuse.Patches.Special;

[HarmonyPatch(typeof(SkillManager), nameof(SkillManager.CheckIsRinLen))]
internal static class CheckIsRinLen_Patch
{
    private static bool Prefix(ref bool __result)
    {
        if (Preferences.CharacterId != 26) return true;

        __result = true;
        return false;
    }
}


[HarmonyPatch]
public static class SkillManager_Patch
{
    private static readonly Logger logger = new("SkillManager_Patch");

    static IEnumerable<MethodBase> TargetMethods()
    {
        return AccessTools.GetDeclaredMethods(typeof(SkillManager))
                                .Where(m => m.Name.Contains("RinLen"));
    }

    private static void Prefix()
    {
        if (Preferences.CharacterId != 26) return;
        Characters.isFlagged = true;
    }
}

[HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.GameStart))]
internal static class GameStartRinLen_Patch
{
    private static void Prefix()
    {
        if (Preferences.CharacterId != 26) return;
        Characters.isFlagged = false;
    }
}