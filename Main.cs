using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static UnityModManagerNet.UnityModManager;

namespace DVJobsRealism
{
    public class Main
    {
        public static ModEntry mod;
        private static Harmony harmony;
        internal static bool enabled;

        private static bool Load(ModEntry entry)
        {
            harmony = new Harmony(entry.Info.Id);
            mod = entry;
            mod.OnToggle = OnToggle;
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            return true;
        }

        private static bool OnToggle(ModEntry entry, bool enabled)
        {
            Main.enabled = enabled;
            return true;
        }
    }
}
