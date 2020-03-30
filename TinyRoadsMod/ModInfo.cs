using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ICities;
using TinyRoadsMod.Redirection;
using TinyRoadsMod.Snapping;
using TinyRoadsMod.Zoning;
using UnityEngine;

namespace TinyRoadsMod
{
    public class ModInfo : LoadingExtensionBase, IUserMod
    {
        public string Name => "Tiny Roads";

        public string Description => "Various quality-of-life improvements for tiny roads & alleys.";

        public void OnEnabled()
        {
            Redirector.PerformRedirections();

            //HarmonyInstance.Create(Constants.HarmonyId)
            //               .PatchAll(Assembly.GetExecutingAssembly());
        }

        public void OnDisabled()
        {
            Redirector.RevertRedirections();

            //HarmonyInstance.Create(Constants.HarmonyId)
            //               .UnpatchAll();
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);

            try
            {
                var networks = Resources.FindObjectsOfTypeAll<NetInfo>();
                var tinyRoads = from network in networks
                                where network.m_halfWidth < 5
                                where network.m_placementStyle == ItemClass.Placement.Manual
                                where network.m_laneTypes.HasFlag(NetInfo.LaneType.Vehicle)
                                where network.m_vehicleTypes.HasFlag(VehicleInfo.VehicleType.Car)
                                where network.m_lanes.Any(lane => lane.m_vehicleType == VehicleInfo.VehicleType.Car)
                                orderby network.name
                                select network;

                var roads = new List<string>();

                foreach (var network in tinyRoads)
                {
                    roads.Add(network.name);
                    RoadZoneBlocksCreationManager.RegisterCustomCreator<TinyRoadZoneBlocksCreator>(network.name);
                    RoadSnappingModeManager.RegisterCustomSnapping<TinyRoadRoadSnappingMode>(network.name);
                }

                Debug.Log($"{Constants.LogTag} Roads found:" + String.Join(", ", roads.ToArray()));
            }
            catch (Exception e)
            {
                Debug.Log($"{Constants.LogTag} Loading:OnLevelLoaded -> Exception: " + e.Message);
            }
        }

        public override void OnLevelUnloading()
        {
            try
            {
                // TODO: De-register snapping & zoning.
            }
            catch (Exception e)
            {
                Debug.Log($"{Constants.LogTag} Loading:OnLevelUnloading -> Exception: " + e.Message);
            }
        }
    }
}
