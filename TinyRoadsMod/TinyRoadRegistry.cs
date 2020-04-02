using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TinyRoadsMod
{
    public static class TinyRoadRegistry
    {
        private static readonly HashSet<string> _tinyRoads = new HashSet<string>();

        public static void Initialize()
        {
            if (_tinyRoads.Count != 0)
                return;

            try
            {
                var networks  = Resources.FindObjectsOfTypeAll<NetInfo>();
                var tinyRoads = from network in networks
                                let width  = network.m_halfWidth * 2
                                let roadAi = network.m_netAI as RoadAI // ground level road AI
                                where roadAi != null // is road
                                where roadAi.m_enableZoning // road has zoning (eg. not highways)
                                where width <= Constants.Networks.TinyRoadWidth + Constants.Networks.WidthTolerance // is tiny road (with tolerance)
                                where network.m_placementStyle == ItemClass.Placement.Manual // exclude generated stuff like bus stops
                                where network.m_laneTypes.HasFlag(NetInfo.LaneType.Vehicle) // sanity check
                                where network.m_vehicleTypes.HasFlag(VehicleInfo.VehicleType.Car) // sanity check
                                where network.m_lanes.Any(lane => lane.m_vehicleType.HasFlag(VehicleInfo.VehicleType.Car)) // sanity check
                                orderby network.name // just for debugging
                                select network;

                foreach (var network in tinyRoads)
                {
                    _tinyRoads.Add(network.name);
                }

                Debug.Log($"{Constants.Log.Tag} Roads found:" + String.Join(", ", _tinyRoads.ToArray()));
            }
            catch (Exception e)
            {
                Debug.Log($"{Constants.Log.Tag} Loading:OnLevelLoaded -> Exception: " + e.Message);
            }

        }

        public static bool IsTinyRoad(string name)
        {
            return _tinyRoads.Contains(name);
        }
    }
}
