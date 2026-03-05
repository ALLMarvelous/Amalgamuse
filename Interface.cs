using UnityEngine;

namespace Amalgamuse;

public class Interface : MonoBehaviour
{
    private bool showMenu = false;
    private Rect windowRect = new(20, 20, 250, 220);

    private string charIdStr = "";
    private string elfinIdStr = "";
    private string eggIdStr = "";

    public Interface(IntPtr ptr) : base(ptr) { }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F6))
        {
            showMenu = !showMenu;

            if (showMenu && Preferences.IsInitialized)
            {
                charIdStr = Preferences.CharacterId.ToString();
                elfinIdStr = Preferences.ElfinId.ToString();
                eggIdStr = Preferences.EggId.ToString();
            }
        }
    }

    private void OnGUI()
    {
        if (!showMenu) return;
        windowRect = GUI.Window(8675309, windowRect, (GUI.WindowFunction)DrawWindow, "Amalgamuse Settings");
    }

    private void DrawWindow(int windowID)
    {
        GUILayout.BeginVertical();

        GUILayout.Label("Character ID:");
        charIdStr = GUILayout.TextField(charIdStr);

        GUILayout.Label("Elfin ID:");
        elfinIdStr = GUILayout.TextField(elfinIdStr);

        GUILayout.Label("Neon Egg ID (0=R6, 1=Saya, 2=Lin, 3=Egg):");
        eggIdStr = GUILayout.TextField(eggIdStr);

        GUILayout.Space(10);

        if (GUILayout.Button("Apply & Save"))
        {
            ApplySettings();
        }

        GUILayout.EndVertical();

        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }

    private void ApplySettings()
    {
        if (!Preferences.IsInitialized) return;

        if (int.TryParse(charIdStr, out int cId)) Preferences.CharacterId = cId;
        if (int.TryParse(elfinIdStr, out int eId)) Preferences.ElfinId = eId;
        if (int.TryParse(eggIdStr, out int eggId)) Preferences.EggId = eggId;
    }
}