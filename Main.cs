using Il2CppInterop.Runtime.Injection;
using MelonLoader;
using System.Reflection;
using UnityEngine;
using Logger = Amalgamuse.Utils.Logger;
using Main = Amalgamuse.Main;

[assembly: AssemblyVersion($"{Main.MelonVersion}.0")]
[assembly: AssemblyFileVersion($"{Main.MelonVersion}.0")]
[assembly: MelonInfo(typeof(Main), Main.MelonName, Main.MelonVersion, Main.MelonAuthor)]
[assembly: MelonGame("PeroPeroGames", "MuseDash")]
[assembly: MelonColor(255, 127, 146, 253)]
[assembly: MelonOptionalDependencies("CustomAlbums")]

namespace Amalgamuse;

public class Main : MelonMod
{
    public const string MelonName = "Amalgamuse";
    public const string MelonVersion = "0.0.3";
    public const string MelonAuthor = "MARVELOUS";

    private readonly Logger logger = new("Amalgamuse");
    private bool isConflict = false;

    public override void OnInitializeMelon()
    {
        foreach (MelonMod melon in RegisteredMelons)
        {
            if (melon.Info.Name == "FavGirl")
            {
                logger.Error("Conflict! Amalgamuse cannot run alongside FavGirl. Disabling all patches!");
                HarmonyInstance.UnpatchSelf();
                isConflict = true;

                return;
            }
        }
    }

    public override void OnLateInitializeMelon()
    {
        if (isConflict) return;

        Preferences.Initialize();

        ClassInjector.RegisterTypeInIl2Cpp<Interface>();
        GameObject ui = new("Amalgamuse");
        ui.AddComponent<Interface>();

        logger.Msg("Initialized Amalgamuse!", false);
    }
}
