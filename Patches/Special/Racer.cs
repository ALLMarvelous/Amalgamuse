using HarmonyLib;
using Il2CppAssets.Scripts.GameCore.Managers;

namespace Amalgamuse.Patches.Special;

[HarmonyPatch(typeof(SkillManager), nameof(SkillManager.CheckIsRacerRin))]
internal static class CheckIsRacerRin_Patch
{

    private static bool Prefix(ref bool __result)
    {
        if (Preferences.CharacterId != 27) return true;

        __result = true;
        return false;
    }
}