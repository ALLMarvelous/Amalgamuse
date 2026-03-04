using HarmonyLib;
using Il2CppAssets.Scripts.GameCore.Managers;

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
