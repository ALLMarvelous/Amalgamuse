using Amalgamuse.Utils;
using HarmonyLib;
using Il2CppAssets.Scripts.Database;

namespace Amalgamuse.Patches;

[HarmonyPatch(typeof(DBConfigCharacter), nameof(DBConfigCharacter.GetCharacterInfoByIndex))]
internal static class GetCharacterInfoByIndex_Patch
{
    private static readonly Logger logger = new("GetCharacterInfoByIndex_Patch");

    private static void Prefix(ref int index)
    {
        logger.Msg("accessed! index: " + index);
        if (!Preferences.IsInitialized || Preferences.CharacterId == -1) return;

        index = Preferences.CharacterId;
    }
}

[HarmonyPatch(typeof(DBConfigCharacter), nameof(DBConfigCharacter.GetCharacterInfoByOrder))]
internal static class GetCharacterInfoByOrder_Patch
{
    private static readonly Logger logger = new("GetCharacterInfoByOrder_Patch");

    private static void Postfix(CharacterInfo __result)
    {
        logger.Msg($"{__result.cosName}: {__result.listIndex}");
    }
}
