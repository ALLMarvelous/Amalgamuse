using Amalgamuse.Utils;
using Il2CppAssets.Scripts.PeroTools.Commons;
using Il2CppAssets.Scripts.PeroTools.Managers;
using MelonLoader;

namespace Amalgamuse;

internal static class Preferences
{
    internal static int CharacterId
    {
        get
        {
            if (characterId is null) throw new Exception("Preferences not initialized!");
            return characterId.Value;
        }
        set
        {
            if (characterId is null) throw new Exception("Preferences not initialized!");

            if (!ValidateCharacter(value))
            {
                logger.Error($"Attempt to set CharacterId to {value} failed validation.");
                return;
            }

            characterId.Value = value;
            category?.SaveToFile();
        }
    }

    internal static int ElfinId
    {
        get
        {
            if (elfinId is null) throw new Exception("Preferences not initialized!");
            return elfinId.Value;
        }
        set
        {
            if (elfinId is null) throw new Exception("Preferences not initialized!");

            if (!ValidateElfin(value))
            {
                logger.Error($"Attempt to set ElfinId to {value} failed validation.");
                return;
            }

            elfinId.Value = value;
            category?.SaveToFile();
        }
    }

    internal static int EggId
    {
        get
        {
            if (eggId is null) throw new Exception("Preferences not initialized!");
            return eggId.Value;
        }
        set
        {
            if (eggId is null) throw new Exception("Preferences not initialized!");

            if (!ValidateEgg(value))
            {
                logger.Error($"Attempt to set EggId to {value} failed validation.");
                return;
            }

            eggId.Value = value;
            category?.SaveToFile();
        }
    }

    public static bool Debug
    {
        get
        {
            if (debug is null) throw new Exception("Preferences not initialized!");
            return debug.Value;
        }
    }

    internal static bool IsInitialized = false;

    private static MelonPreferences_Category? category;
    private static MelonPreferences_Entry<int>? characterId;
    private static MelonPreferences_Entry<int>? elfinId;
    private static MelonPreferences_Entry<int>? eggId;
    private static MelonPreferences_Entry<bool>? debug;

    private static readonly Logger logger = new("Preferences");

    internal static void Initialize()
    {

        category = MelonPreferences.CreateCategory("Amalgamuse", "Amalgamuse Preferences");
        category.SetFilePath("UserData/Amalgamuse.cfg");

        characterId = category.CreateEntry("CharacterId", -1, "Character ID");
        characterId.Description = "The internal ID of the character to display. See README for ID table.";

        elfinId = category.CreateEntry("ElfinId", -1, "Elfin ID");
        elfinId.Description = "The internal ID of the elfin to display. See README for ID table.";

        eggId = category.CreateEntry("EggId", 3, "Neon Egg ID");
        eggId.Description = "When using Neon Egg, which sprite should be used? 0: R6, 1: Saya, 2: Lin, 3: Neon Egg";

        debug = category.CreateEntry("Debug", false, "Debug");
        debug.Description = "Enable debug logs. These will spam your console and lag your game.";

        category.LoadFromFile();
        category.SaveToFile();

        (bool validCharacter, bool validElfin, bool validEgg) = Validate();

        if (!validCharacter)
        {
            logger.Error("Character ID saved in preferences failed validation, set to -1.");
            CharacterId = -1;
        }

        if (!validElfin)
        {
            logger.Error("Elfin ID in saved preferences failed validation, set to -1.");
            ElfinId = -1;
        }

        if (!validEgg)
        {
            logger.Error("Neon Egg ID in saved preferences failed validation, set to 3.");
            EggId = 3;
        }

        IsInitialized = true;

        logger.Msg("Initialized character ID: " + CharacterId);
        logger.Msg("Initialized elfin ID: " + ElfinId);
        logger.Msg("Initialized neon egg ID: " + EggId);

        logger.Success("Initialized preferences!");
    }

    private static (bool validCharacter, bool validElfin, bool validEgg) Validate() =>
        (ValidateCharacter(), ValidateElfin(), ValidateEgg());

    private static bool ValidateCharacter() => ValidateCharacter(CharacterId);
    private static bool ValidateCharacter(int id)
    {
        var config = Singleton<ConfigManager>.instance;

        // Validate Character: True if -1, otherwise must be >= 0 and exist in DB
        return id == -1 || (id >= 0 && config.GetConfigStringValue("character", id, "cosName") != null);
    }

    private static bool ValidateElfin() => ValidateElfin(ElfinId);
    private static bool ValidateElfin(int id)
    {
        var config = Singleton<ConfigManager>.instance;

        // Validate Elfin: True if -1, otherwise must be >= 0 and exist in DB
        return id == -1 || (id >= 0 && config.GetConfigStringValue("elfin", id, "name") != null);
    }

    private static bool ValidateEgg() => ValidateEgg(EggId);
    private static bool ValidateEgg(int id) =>
        new[] { 0, 1, 2, 3 }.Contains(id);
}
