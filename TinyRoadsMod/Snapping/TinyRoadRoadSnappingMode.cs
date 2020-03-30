using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyRoadsMod.Snapping
{
    public class TinyRoadRoadSnappingMode : IRoadSnappingMode
    {
        public float GetLengthSnap()
        {
            return 4f;
        }
    }
}
