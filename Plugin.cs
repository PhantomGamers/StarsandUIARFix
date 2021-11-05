using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

namespace StarsandUIARFix
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log;
        private void Awake()
        {
            Log = Logger;

            // Plugin startup logic
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            Harmony.CreateAndPatchAll(typeof(Patches));
        }

    }

    [HarmonyPatch]
    public class Patches
    {
        [HarmonyPatch(typeof(UltimateSurvival.GUISystem.GUIController), nameof(UltimateSurvival.GUISystem.GUIController.Start))]
        [HarmonyPostfix]
        public static void UpdateGUIAR(UltimateSurvival.GUISystem.GUIController __instance)
        {
            Plugin.Log.LogInfo("Updating GUI AR!");
            __instance.m_GUICamera.aspect = 16 / 9f;
        }
    }
}
