using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using ICities;

namespace TinyRoadsMod
{
    public sealed class ModInfo : LoadingExtensionBase, IUserMod
    {
        private readonly HarmonyInstance _harmony = HarmonyInstance.Create(Constants.Mod.HarmonyId);

        public string Name => "Tiny Roads";

        public string Description => "Various quality-of-life improvements for tiny roads & alleys.";

        public void OnEnabled()
        {
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public void OnDisabled()
        {
            _harmony.UnpatchAll();
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);

            TinyRoadRegistry.Initialize();
        }
    }
}
