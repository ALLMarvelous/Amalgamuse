using Amalgamuse.Utils;
using HarmonyLib;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.GameCore.Managers;
using Il2CppAssets.Scripts.UI;
using Il2CppAssets.Scripts.UI.GameMain;

namespace Amalgamuse.Patches;

internal static class Characters
{
    internal static bool isFlagged = false;

    internal static bool ShouldOverride(out int charId)
    {
        charId = Preferences.CharacterId;
        return isFlagged && Preferences.IsInitialized && charId != -1;
    }

    internal static bool IsTargetVoice(VoiceType type) =>
        type is VoiceType.Hurt or VoiceType.EmptyHit or VoiceType.Victory or VoiceType.Fail or VoiceType.Start;
}

/// <summary>
/// Main character info switcher. Above flag must be set to true
/// </summary>
[HarmonyPatch(typeof(DBConfigCharacter), nameof(DBConfigCharacter.GetCharacterInfoByIndex))]
internal static class GetCharacterInfoByIndex_Patch
{
    private static Logger logger = new("GetCharacterInfoByIndex_Patch");
    private static void Prefix(ref int index)
    {
        logger.Msg("accessed! index: " + index);

        if (Characters.ShouldOverride(out int overrideId))
            index = overrideId;
    }
}

/// <summary>
/// Could be used to return april fools or cat versions of skins
/// </summary>
[HarmonyPatch(typeof(DBUi), nameof(DBUi.GetSkinIndex))]
internal static class GetSkinIndex_Patch
{
    private static void Prefix(ref int roleIndex)
    {
        if (Characters.ShouldOverride(out int overrideId))
            roleIndex = overrideId;
    }
}

/// <summary>
/// Manages the victory & fail animations
/// </summary>
[HarmonyPatch(typeof(CharCreate), nameof(CharCreate.OnEnable))]
internal static class CharCreate_Patch
{
    private static void Prefix() => Characters.isFlagged = true;
    private static void Postfix() => Characters.isFlagged = false;
}

/// <summary>
/// Manages the home panel character
/// </summary>
[HarmonyPatch(typeof(MuseShow), nameof(MuseShow.OnEnable))]
internal static class MuseShow_Patch
{
    private static void Prefix() => Characters.isFlagged = true;
    private static void Postfix() => Characters.isFlagged = false;
}

[HarmonyPatch(typeof(AbstractGirlManager), nameof(AbstractGirlManager.InstanceGirl))]
internal static class InstanceGirl_Patch
{
    private static void Prefix() => Characters.isFlagged = true;
    private static void Postfix() => Characters.isFlagged = false;
}

/// <summary>
/// Manages the victory & fail voices
/// </summary>
[HarmonyPatch(typeof(VoiceSetting), nameof(VoiceSetting.GetVoice))]
internal static class GetVoice_Patch
{
    private static void Prefix(VoiceType type, ref int index)
    {
        if (Preferences.IsInitialized && Preferences.CharacterId != -1 && Characters.IsTargetVoice(type))
            index = Preferences.CharacterId;
    }
}

/// <summary>
/// Manages the in-game voices and hit effects
/// </summary>
[HarmonyPatch(typeof(VoiceSetting), nameof(VoiceSetting.GetVoiceConfig))]
internal static class GetVoiceConfig_Patch
{
    private static void Prefix(VoiceType type, ref int index)
    {
        if (Preferences.IsInitialized && Preferences.CharacterId != -1 && Characters.IsTargetVoice(type))
            index = Preferences.CharacterId;
    }
}

[HarmonyPatch(typeof(DBConfigCharacter), nameof(DBConfigCharacter.GetCharacterInfoByOrder))]
internal static class GetCharacterInfoByOrder_Patch
{
    private static readonly Logger logger = new("GetCharacterInfoByOrder_Patch");

    private static void Postfix(int order, CharacterInfo __result)
    {
        logger.Msg($"order {order}: {__result.listIndex}");
    }
}
