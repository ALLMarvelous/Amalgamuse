using Amalgamuse.Utils;
using HarmonyLib;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.PeroTools.Nice.Events;
using Il2CppAssets.Scripts.UI.Specials;
using MelonLoader;
using UnityEngine;
using Logger = Amalgamuse.Utils.Logger;

namespace Amalgamuse.Patches;

[HarmonyPatch(typeof(DBConfigElfin), nameof(DBConfigElfin.GetElfinInfoByIndex))]
internal static class GetElfinInfoByIndex_Patch
{
    private static readonly Logger logger = new("GetElfinInfoByIndex_Patch");

    private static void Prefix(ref int index)
    {
        logger.Msg("accessed! index: " + index);
        if (!Preferences.IsInitialized || Preferences.ElfinId == -1) return;

        index = Preferences.ElfinId;
    }
}

[HarmonyPatch(typeof(OnActivate), nameof(OnActivate.OnEnable))]
internal static class OnEnable_Patch
{
    private static int elfinBuffer;
    private static readonly Logger logger = new("OnActivate");

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

        logger.Msg("returned elfin index");
    }
}
