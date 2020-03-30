using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyRoadsMod.Zoning
{
    public interface IZoneBlocksCreator
    {
        void CreateZoneBlocks(NetInfo info, ushort segmentId, ref NetSegment segment);
    }
}
