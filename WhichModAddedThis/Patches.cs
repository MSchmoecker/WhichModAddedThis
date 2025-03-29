using System;
using BepInEx.Bootstrap;
using HarmonyLib;
using Jotunn.Utils;

namespace WhichModAddedThis;

[HarmonyPatch]
public static class Patches {
    [HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetTooltip), new Type[] { typeof(ItemDrop.ItemData), typeof(int), typeof(bool), typeof(float), typeof(int) })]
    [HarmonyPostfix, HarmonyPriority(Priority.Last), HarmonyAfter("randyknapp.mods.epicloot")]
    public static void AppendModName(ref string __result, ItemDrop.ItemData item) {
        string prefabName = PrefabName(item);
        IModPrefab modPrefab = ModQuery.GetPrefab(prefabName);
        __result = __result.TrimEnd() + "\n" + GetTooltipModName(modPrefab);
    }

    [HarmonyPatch(typeof(Hud), nameof(Hud.SetupPieceInfo)), HarmonyPostfix]
    public static void SetupPieceInfoPatch(Hud __instance, Piece piece) {
        IModPrefab modPrefab = ModQuery.GetPrefab(piece.name);
        __instance.m_pieceDescription.text = __instance.m_pieceDescription.text.TrimEnd() + GetTooltipModName(modPrefab);
    }

    public static string GetTooltipModName(IModPrefab modPrefab) {
        string color = "orange";

        if (Chainloader.PluginInfos.ContainsKey("randyknapp.mods.epicloot")) {
            color = "#ADD8E6FF";
        }

        string modName = modPrefab?.SourceMod?.Name ?? "Valheim";
        return $"\n<color={color}>{modName}</color>";
    }

    private static string PrefabName(ItemDrop.ItemData item) {
        if (item == null) {
            return string.Empty;
        }

        if (item.m_dropPrefab != null) {
            return item.m_dropPrefab.name;
        }

        return item.m_shared.m_name;
    }
}
