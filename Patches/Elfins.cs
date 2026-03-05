using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.PeroTools.Nice.Events;
using System.Reflection;
using Logger = Amalgamuse.Utils.Logger;

namespace Amalgamuse.Patches;

[HarmonyPatch(typeof(DBConfigElfin), nameof(DBConfigElfin.GetElfinInfoByIndex))]
internal static class GetElfinInfoByIndex_Patch
{
    internal static bool isFlagged = false;
    private static readonly Logger logger = new("GetElfinInfoByIndex_Patch");

    private static void Prefix(ref int index)
    {
        logger.Msg("accessed! index: " + index);

        if (!isFlagged) return;
        if (!Preferences.IsInitialized || Preferences.ElfinId == -1) return;

        index = Preferences.ElfinId;
    }
}

[HarmonyPatch(typeof(OnActivate), nameof(OnActivate.OnEnable))]
internal static class OnActivate_Patch
{
    private static int elfinBuffer;
    private static readonly Logger logger = new("OnActivate_Patch");

    private static void Prefix(OnActivate __instance)
    {
        if (__instance.name != "ElfinShow") return;
        if (!Preferences.IsInitialized || Preferences.ElfinId == -1) return;

        elfinBuffer = DataHelper.selectedElfinIndex;
        DataHelper.selectedElfinIndex = Preferences.ElfinId;

        logger.Msg("swapped elfin index");
    }

    private static void Postfix(OnActivate __instance)
    {
        if (__instance.name != "ElfinShow") return;
        if (!Preferences.IsInitialized || Preferences.ElfinId == -1) return;

        DataHelper.selectedElfinIndex = elfinBuffer;

        logger.Msg("restored elfin index");
    }
}

[HarmonyPatch]
internal static class OnBattleStart_Patch
{
    private static readonly Logger logger = new("OnBattleStart_Patch");

    [HarmonyTargetMethods]
    static IEnumerable<MethodBase> TargetMethods()
    {
        return typeof(ElfinCreate).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(m => m.Name == "OnBattleStart");
    }

    private static void Prefix()
    {
        GetElfinInfoByIndex_Patch.isFlagged = true;
        logger.Msg("switched flag on!");
    }

    private static void Postfix()
    {
        GetElfinInfoByIndex_Patch.isFlagged = false;
        logger.Msg("switched flag off!");
    }
}
