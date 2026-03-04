using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.GameCore.Managers;
using Il2CppFormulaBase;

namespace Amalgamuse.Patches.Special;

[HarmonyPatch(typeof(NeonEggIncubationHandle), nameof(NeonEggIncubationHandle.Awake))]
internal static class NeonEggIncubationHandle_Patch
{
    internal static NeonEggIncubationHandle? instance;

    private static void Postfix(NeonEggIncubationHandle __instance)
    {
        var battleProperty = BattleProperty.instance;
        battleProperty.currentNeonEggForm = Preferences.EggId;

        instance = __instance;
    }
}

[HarmonyPatch(typeof(StageBattleComponent), nameof(StageBattleComponent.GameStart))]
internal static class GameStart_Patch
{
    private static void Postfix()
    {
        if (Preferences.EggId == 3) return;

        var instance = NeonEggIncubationHandle_Patch.instance;
        if (instance == null) return;

        instance.OnFever();
    }
}

[HarmonyPatch(typeof(NeonEggIncubationHandle), nameof(NeonEggIncubationHandle.OnFever))]
internal static class OnFever_Patch
{
    private static bool Prefix()
    {
        return Preferences.EggId != 3;
    }
}