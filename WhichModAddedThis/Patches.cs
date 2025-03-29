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
        if (item != null) {
            IModPrefab modPrefab = ModQuery.GetPrefab(PrefabName(item));
            __result = __result.TrimEnd() + "\n" + GetTooltipModName(modPrefab?.SourceMod?.Name ?? "Valheim");
        }
    }

    [HarmonyPatch(typeof(Hud), nameof(Hud.SetupPieceInfo)), HarmonyPostfix]
    public static void SetupPieceInfoPatch(Hud __instance, Piece piece) {
        if (piece) {
            IModPrefab modPrefab = ModQuery.GetPrefab(piece.name);
            __instance.m_pieceDescription.text = __instance.m_pieceDescription.text.TrimEnd() + GetTooltipModName(modPrefab?.SourceMod?.Name ?? "Valheim");
        }
    }

    public static string GetTooltipModName(string modName) {
        string color = "orange";

        if (Chainloader.PluginInfos.ContainsKey("randyknapp.mods.epicloot")) {
            color = "#ADD8E6FF";
        }

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
