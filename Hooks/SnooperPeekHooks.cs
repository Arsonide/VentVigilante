﻿using System;
using HarmonyLib;
using UnityEngine;
using VentrixSyncDisks.Implementation.Common;
using VentrixSyncDisks.Implementation.Snooping;

namespace VentrixSyncDisks.Hooks;

public class SnooperPeekHooks
{
    [HarmonyPatch(typeof(Player), "OnDuctSectionChange")]
    public class PlayerOnDuctSectionChangeHook
    {
        [HarmonyPostfix]
        private static void Postfix(Player __instance)
        {
            SnoopHighlighter.RefreshSnoopingState();
        }
    }
    
    [HarmonyPatch(typeof(OutlineController), "SetOutlineActive")]
    public class OutlineControllerSetOutlineActiveHook
    {
        [HarmonyPostfix]
        private static void Postfix(OutlineController __instance, bool val)
        {
            SnoopHighlighter.EnforceOutlineLayer(__instance.actor);
        }
    }
    
    [HarmonyPatch(typeof(OutlineController), "SetAlpha")]
    public class OutlineControllerSetOutlineAlphaHook
    {
        [HarmonyPostfix]
        private static void Postfix(OutlineController __instance, float val)
        {
            SnoopHighlighter.EnforceOutlineAlpha(__instance.actor);
        }
    }
    
    [HarmonyPatch(typeof(Player), "EnterVent")]
    public class PlayerEnterVentHook
    {
        [HarmonyPostfix]
        private static void Postfix(Player __instance, bool restoreTransform = false)
        {
            SnoopHighlighter.RefreshSnoopingState();
        }
    }
    
    [HarmonyPatch(typeof(Player), "ExitVent")]
    public class PlayerExitVentHook
    {
        [HarmonyPostfix]
        private static void Postfix(Player __instance, bool restoreTransform = false)
        {
            SnoopHighlighter.RefreshSnoopingState();
        }
    }
    
    [HarmonyPatch(typeof(Human), "AddMesh", new Type[] {typeof(MeshRenderer), typeof(bool), typeof(bool), typeof(bool), typeof(bool)})]
    public class HumanAddMeshHook
    {
        [HarmonyPostfix]
        private static void Postfix(Human __instance, MeshRenderer newMesh, bool addToOutline = true, bool forceMeshListUpdate = false, bool addToLOD1 = false, bool addToBoth = false)
        {
            SnoopHighlighter.OnActorChangedMeshes(__instance);
        }
    }
    
    [HarmonyPatch(typeof(Human), "RemoveMesh")]
    public class HumanRemoveMeshHook
    {
        [HarmonyPostfix]
        private static void Postfix(Human __instance, MeshRenderer newMesh, bool removeFromOutline = true, bool forceMeshListUpdate = false)
        {
            SnoopHighlighter.OnActorChangedMeshes(__instance);
        }
    }
    
    [HarmonyPatch(typeof(Human), "UpdateMeshList")]
    public class HumanUpdateMeshListHook
    {
        [HarmonyPostfix]
        private static void Postfix(Human __instance)
        {
            SnoopHighlighter.OnActorChangedMeshes(__instance);
        }
    }
    
    [HarmonyPatch(typeof(Actor), "OnRoomChange")]
    public class ActorOnRoomChangeHook
    {
        [HarmonyPostfix]
        private static void Postfix(Actor __instance)
        {
            SnoopHighlighter.OnActorRoomChanged(__instance);
        }
    }
}