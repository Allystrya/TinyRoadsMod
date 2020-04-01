using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using TinyRoadsMod.Zoning;

namespace TinyRoadsMod.Patches
{
    [HarmonyPatch(typeof(RoadAI), nameof(RoadAI.CreateZoneBlocks))]
    internal static class RoadAI_CreateZoneBlocks
    {
        internal static bool Prefix(RoadAI __instance, ushort segment, ref NetSegment data)
        {
            if (TinyRoadRegistry.IsTinyRoad(__instance.m_info.name))
            {
                TinyRoadZoneBlocksCreator.CreateZoneBlocks(__instance.m_info, segment, ref data);

                return false;
            }
            else
            {
                return true; // execute original
            }
        }
    }
}
