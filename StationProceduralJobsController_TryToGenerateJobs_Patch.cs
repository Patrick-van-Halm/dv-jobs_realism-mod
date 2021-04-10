using HarmonyLib;

namespace DVJobsRealism
{
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable IDE0051 // Remove unused private members
    [HarmonyPatch(typeof(StationProceduralJobsController), "TryToGenerateJobs")]
    internal class StationProceduralJobsController_TryToGenerateJobs_Patch
    {
        private static void Prefix(StationProceduralJobsController __instance)
        {
            if (!Main.enabled)
                return;

            __instance.generationRuleset.jobsCapacity = Main.settings.jobsCapacity;
            __instance.generationRuleset.maxShuntingStorageTracks = Main.settings.maxShuntingStorageTracks;
            __instance.generationRuleset.minCarsPerJob = Main.settings.minCarsPerJob;
            __instance.generationRuleset.maxCarsPerJob = Main.settings.maxCarsPerJob;
        }
    }
}
