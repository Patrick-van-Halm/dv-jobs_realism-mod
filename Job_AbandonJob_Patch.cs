using DV.Logic.Job;
using DV.ServicePenalty;
using HarmonyLib;
using System.Linq;

namespace DVJobsRealism
{
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable IDE0051 // Remove unused private members

    [HarmonyPatch(typeof(Job), "AbandonJob")]
    internal class Job_AbandonJob_Patch
    {
        private static void Postfix(Job __instance)
        {
            SingletonBehaviour<CareerManagerDebtController>.Instance.RegisterDebt(new AbandonJobDebt(__instance));
        }
    }
}
