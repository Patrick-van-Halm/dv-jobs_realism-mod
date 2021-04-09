using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;
using static UnityModManagerNet.UnityModManager;

namespace DVJobsRealism
{
#if DEBUG
    [EnableReloading]
#endif
    public class Main
    {
        public static ModEntry mod;
        public static Settings settings;
        private static Harmony harmony;
        internal static bool enabled;

        private static bool Load(ModEntry entry)
        {
            harmony = new Harmony(entry.Info.Id);
            mod = entry;

            try { settings = Settings.Load<Settings>(mod); } catch { }
            
            mod.OnToggle = OnToggle;
            mod.OnUnload = Unload;
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            return true;
        }

        private static bool Unload(ModEntry _)
        {
            harmony.UnpatchAll(mod.Info.Id);
            return true;
        }

        private static bool OnToggle(ModEntry entry, bool enabled)
        {
            Main.enabled = enabled;
            return true;
        }
    }
}
