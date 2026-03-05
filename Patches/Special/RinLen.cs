using Amalgamuse.Utils;
using HarmonyLib;
using Il2CppAssets.Scripts.GameCore.Managers;
using Il2CppAssets.Scripts.PeroTools.Commons;
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
public static class RinLenDoubleManager_Patch
{
    private static readonly Logger logger = new("RinLenDoubleManager_Patch");

    static IEnumerable<MethodBase> TargetMethods()
    {
        return AccessTools.GetDeclaredMethods(typeof(RinLenDoubleManager))
                                .Where(m => !m.IsSpecialName && !m.IsConstructor);
    }

    private static void Prefix()
    {
        if (Preferences.CharacterId != 26) return;
        Characters.isFlagged = true;
    }

    private static void Postfix()
    {
        if (Preferences.CharacterId != 26) return;
        Characters.isFlagged = false;
    }
}
