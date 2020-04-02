using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace TinyRoadsMod.Patches
{
    [HarmonyPatch(typeof(RoadAI), nameof(RoadAI.GetLengthSnap))]
    internal static class RoadAI_GetLengthSnap
    {
        internal static void Postfix(RoadAI __instance, ref float __result)
        {
            if (TinyRoadRegistry.IsTinyRoad(__instance.m_info.name))
            {
                __result = Constants.Networks.TinyRoadLengthSnap;
            }
        }
    }
}
