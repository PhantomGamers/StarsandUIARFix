using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;

using HarmonyLib;

using UnityEngine;
using UnityEngine.UI;

namespace StarsandUIARFix
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log;

        public static ConfigEntry<CanvasScaler.ScreenMatchMode> ScreenMatchMode;
        public static ConfigEntry<float> MatchWidthOrHeight;
        public static ConfigEntry<bool> ApplyToMenu;

        private void Awake()
        {
            Log = Logger;

            // Plugin startup logic
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            ScreenMatchMode = Config.Bind(
                "General",
                "ScreenMatchMode",
                CanvasScaler.ScreenMatchMode.MatchWidthOrHeight,
                "Scale the canvas area with the width as reference, the height as reference, or something in between."
                );

            MatchWidthOrHeight = Config.Bind(
                "General",
                "MatchWidthOrHeight",
                1f,
                "0 = width, 1 = height. Only used if ScreenMatchMode is set to MatchWidthOrHeight"
                );

            ApplyToMenu = Config.Bind(
                "General",
                "ApplyToMenu",
                false,
                "Whether the screen match mode settings should be applied to the menu. Doesn't seem to be needed in most cases."
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
            var cs = __instance.Canvas.gameObject.GetComponent<CanvasScaler>();
            UpdateCanvasScalerSettings(cs);
        }

        [HarmonyPatch(typeof(MenuScene), nameof(MenuScene.Start))]
        [HarmonyPostfix]
        public static void UpdateMenuScaleFactor(MenuScene __instance)
        {
            if (Plugin.ApplyToMenu.Value)
            {
                var cs = GameObject.Find("Canvas").GetComponent<CanvasScaler>();
                UpdateCanvasScalerSettings(cs);
            }
        }

        private static void UpdateCanvasScalerSettings(CanvasScaler cs)
        {
            cs.screenMatchMode = Plugin.ScreenMatchMode.Value;
            cs.matchWidthOrHeight = Plugin.MatchWidthOrHeight.Value;
            Plugin.Log.LogInfo($"Updated GUI canvas scaler");
        }
    }
}