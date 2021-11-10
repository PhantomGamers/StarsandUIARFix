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
        public static ConfigEntry<bool> UseResolutionScaleFactor;

        public static float ResolutionScaleFactor => (float)Screen.currentResolution.width / Screen.currentResolution.height / (16 / 9f);

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

            UseResolutionScaleFactor = Config.Bind("General",
                                     "UseScaleFactor",
                                     true,
                                     "If true, uses the ScaleFactor value calculated from your current resolution instead of the value set in ScaleFactor"
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
            if(Plugin.UseScaleFactor.Value)
            {
                __instance.Canvas.scaleFactor = Plugin.UseResolutionScaleFactor.Value ? Plugin.ResolutionScaleFactor : Plugin.ScaleFactor.Value;
                Plugin.Log.LogInfo($"Changed GUI canvas scale factor to {__instance.Canvas.scaleFactor}");
            }
            else
            {
                __instance.m_GUICamera.aspect = 16 / 9f;
                Plugin.Log.LogInfo($"Changed GUICamera aspect ratio to {__instance.m_GUICamera.aspect}");
            }
        }

        [HarmonyPatch(typeof(MenuScene), nameof(MenuScene.Start))]
        [HarmonyPostfix]
        public static void UpdateMenuScaleFactor(MenuScene __instance)
        {
            if (Plugin.UseScaleFactor.Value)
            {
                var canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                canvas.scaleFactor = Plugin.UseResolutionScaleFactor.Value ? Plugin.ResolutionScaleFactor : Plugin.ScaleFactor.Value;
                Plugin.Log.LogInfo($"Changed GUI canvas scale factor to {canvas.scaleFactor}");
            }
        }
    }
}
