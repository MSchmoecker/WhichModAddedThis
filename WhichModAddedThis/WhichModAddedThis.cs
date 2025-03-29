using BepInEx;
using HarmonyLib;
using Jotunn.Utils;

namespace WhichModAddedThis {
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    internal class WhichModAddedThis : BaseUnityPlugin {
        public const string PluginGUID = "com.maxsch.valheim.WhichModAddedThis";
        public const string PluginName = "WhichModAddedThis";
        public const string PluginVersion = "0.1.0";

        private Harmony harmony;

        private void Awake() {
            harmony = new Harmony(PluginGUID);
            harmony.PatchAll();

            ModQuery.Enable();
        }
    }
}
