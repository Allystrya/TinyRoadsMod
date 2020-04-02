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
                                where network.m_halfWidth < 5
                                where network.m_placementStyle == ItemClass.Placement.Manual
                                where network.m_laneTypes.HasFlag(NetInfo.LaneType.Vehicle)
                                where network.m_vehicleTypes.HasFlag(VehicleInfo.VehicleType.Car)
                                where network.m_lanes.Any(lane => lane.m_vehicleType == VehicleInfo.VehicleType.Car)
                                orderby network.name
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
