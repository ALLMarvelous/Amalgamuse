using Amalgamuse.Utils;
using MelonLoader;
using System.Reflection;
using Main = Amalgamuse.Main;

[assembly: AssemblyVersion($"{Main.MelonVersion}.0")]
[assembly: AssemblyFileVersion($"{Main.MelonVersion}.0")]
[assembly: MelonInfo(typeof(Main), Main.MelonName, Main.MelonVersion, Main.MelonAuthor)]
[assembly: MelonGame("PeroPeroGames", "MuseDash")]
[assembly: MelonColor(255, 127, 146, 253)]

namespace Amalgamuse;

public class Main : MelonMod
{
    public const string MelonName = "Amalgamuse";
    public const string MelonVersion = "0.0.1";
    public const string MelonAuthor = "MARVELOUS";

    private readonly Logger logger = new("Amalgamuse");

    public override void OnInitializeMelon()
    {
        foreach (MelonMod melon in RegisteredMelons)
        {
            if (melon.Info.Name == "FavGirl")
            {
                logger.Error("Conflict! Amalgamuse cannot run alongside FavGirl. Disabling all patches!");
                HarmonyInstance.UnpatchSelf();

                return;
            }
        }

        Preferences.Initialize();
        logger.Msg("Initialized Amalgamuse!", false);
    }
}
