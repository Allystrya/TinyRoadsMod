using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyRoadsMod
{
    internal static class Constants
    {
        internal static class Log
        {
            public const string Tag = "[TinyRoads]";
        }

        internal static class Mod
        {
            public const string HarmonyId = "com.github.cmircea.tinyroads";
        }

        internal static class Networks
        {
            public const float TinyRoadWidth     = 8f;
            public const float TinyRoadHalfWidth = TinyRoadWidth / 2;

            public const float WidthTolerance = 0.35f; // SC - Basic Streets 2-way alley is 8m wide but 1-way is 8.3m wide

            public const float TinyRoadLengthSnap = Zoning.CellSize / 2;
        }

        internal static class Zoning
        {
            public const float CellSize = 8;
        }
    }
}
