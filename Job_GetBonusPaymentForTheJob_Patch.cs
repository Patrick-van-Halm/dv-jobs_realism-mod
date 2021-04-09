using DV.Logic.Job;
using HarmonyLib;

namespace DVJobsRealism
{
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable IDE0051 // Remove unused private members
    [HarmonyPatch(typeof(Job), "GetBonusPaymentForTheJob")]
    internal class Job_GetBonusPaymentForTheJob_Patch
    {
        private static void Postfix(ref float __result)
        {
            if (!Main.enabled && Main.settings.bonusPaymentEnabled)
                return;

            __result = 0;
        }
    }
}
