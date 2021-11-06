using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;

using HarmonyLib;

using UnityEngine;

namespace StarsandUIARFix
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log;

        public static ConfigEntry<float> ScaleFactor;
        public static ConfigEntry<bool> UseScaleFactor;

        private void Awake()
        {
            Log = Logger;

            // Plugin startup logic
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            ScaleFactor = Config.Bind("General",
                                      "ScaleFactor",
                                      1f,
                                      "Scale Factor for the UI"
                                     );

            UseScaleFactor = Config.Bind("General",
                                     "UseScaleFactor",
                                     true,
                                     "If true, uses the ScaleFactor value to fix the UI. Otherwise, it will stretch the UI to your AR from the default 16/9 AR."
                                    );

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
            if(!Plugin.UseScaleFactor.Value)
            {
                Plugin.Log.LogInfo("Updating GUI AR!");
                __instance.m_GUICamera.aspect = 16 / 9f;
            }
            else
            {
                Plugin.Log.LogInfo("Updating GUI scale factor!");
                __instance.Canvas.scaleFactor = Plugin.ScaleFactor.Value;
            }
        }

        [HarmonyPatch(typeof(MenuScene), nameof(MenuScene.Start))]
        [HarmonyPostfix]
        public static void UpdateMenuScaleFactor(MenuScene __instance)
        {
            if (Plugin.UseScaleFactor.Value)
            {
                Plugin.Log.LogInfo("Updating menu scale factor!");
                GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor = Plugin.ScaleFactor.Value;
            }
        }
    }
}
